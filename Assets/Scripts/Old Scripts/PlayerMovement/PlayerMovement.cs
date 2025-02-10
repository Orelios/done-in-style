using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    #region Components
    [Header("Components")]
    [Tooltip("Attach here the Player's Rigidbody2D")]
    public Rigidbody2D Rb;
    [Tooltip("Attach here the Player's GroundCheck")]
    public Transform GroundCheck;
    [Tooltip("Select here the Ground LayerMask for ground detection")]
    public LayerMask GroundLayer;
    [Tooltip("Attach here the Camera Handler")]
    [SerializeField] private CameraHandler cameraHandler;
    #endregion

    #region Horizontal Movement
    [Header("Horizontal Movement Configs")]
    [Tooltip("Insert here the max movement speed of the Player")]
    [SerializeField] private float baseSpeed = 8f;
    [Tooltip("Insert here the acceleration factor of the Player")]
    [SerializeField] private float acceleration;
    [Tooltip("Insert here the deceleration factor of the Player")]
    [SerializeField] private float deceleration;
    [Tooltip("Insert here the velocity power of the Player; this is used mainly for when the Player is changing horizontal directions")]
    [SerializeField] private float velPower;
    [Tooltip("Insert here the friction amount; this helps the deceleration to put the Player to a complete stop faster ")]
    [SerializeField] private float frictionAmount; 
    public float BaseSpeed { get => baseSpeed; set => baseSpeed = value; }
    public float Acceleration { get => acceleration; set => acceleration = value; }
    public float Deceleration { get => deceleration; set => deceleration = value; }
    public float VelPower { get => velPower; set => velPower = value; }
    public float FrictionAmount { get => frictionAmount; set => frictionAmount = value; }
    public float AppliedMovementSpeed { get; private set; }
    public float AppliedAcceleration { get; private set; }
    public bool IsFacingRight = true;
    private Vector2 _velocity; 
    #endregion

    #region Jump
    [Header("Jump Configs")]
    [Tooltip("Insert here the jump power of the Player; this is how high the Player can jump")]
    [SerializeField]  private float jumpPower;
    [Tooltip("Insert here how long Coyote Time will run after the player goes off of a ledge; this is how long the Player can still jump after going off a ledge")]
    [SerializeField, Range(0.05f, 0.25f)] private float coyoteTime = 0.2f;
    [Tooltip("UNIMPLEMENTED: Insert here how long the Player's jump input will buffer; this is how long the Player's jump input is saved when in the air to help time the next jump when landing on the ground")]
    [SerializeField, Range(0.05f, 0.25f)]private float jumpBufferTime = 0.2f;
    public float JumpPower { get => jumpPower; set => jumpPower = value; }
    public float CoyoteTime { get => coyoteTime; set => coyoteTime = value; }
    public float JumpBufferTime { get => jumpBufferTime; set => jumpBufferTime = value; }
    private float _lastGroundedTime = 0f;
    #endregion

    #region Gravity
    [Header("Gravity")]
    [Tooltip("Insert here the base gravity value of the Player; this is how fast the Player will fall down")]
    [SerializeField] private float baseGravity;
    [Tooltip("Insert here the maximum falling speed the Player will have when falling down")]
    [SerializeField] private float maxFallSpeed;
    [Tooltip("Insert here the fall speed multiplier when falling down; this is used to help reach the maximum falling speed faster")]
    [SerializeField] private float fallSpeedMultiplier;
    public float BaseGravity { get => baseGravity; set => baseGravity = value; }
    public float MaxFallSpeed { get => maxFallSpeed; set => maxFallSpeed = value; }
    public float FallSpeedMultiplier { get => fallSpeedMultiplier; set => fallSpeedMultiplier = value; }
    #endregion

    #region Others
    [Header("Debug Configs")]
    [SerializeField] private bool showGizmos = false;
    
    [Header("Test")] 
    [SerializeField] private float maxMovementSpeed;
    public float MaxMovementSpeed => baseSpeed;
    public float AppliedMaxMovementSpeed { get; private set; }

    #endregion
    
    #region Private Variables
    private PlayerInputManager _playerInputManager;
    private PlayerGearSwapper _playerGearSwapper;
    private GearTricks _gearTricks;
    private PlayerVelocitySM _playerVelocitySM;
    private Temp_RankCalculator _rankCalculator;
    #endregion
    
    private void Awake()
    {
        _playerInputManager = GetComponent<PlayerInputManager>();
        _playerGearSwapper = GetComponent<PlayerGearSwapper>();
        _gearTricks = GetComponent<GearTricks>();
        _rankCalculator = FindFirstObjectByType<Temp_RankCalculator>();

        InitializeStateMachine();
    }

    //function to create the velocity State Machine and its States  
    private void InitializeStateMachine()
    {
        //create the State Machine
        _playerVelocitySM = new();

        //declare and create the velocity States
        var atRestState = new AtRestState(this);
        var acceleratingState = new AcceleratingState(this);
        var maxSpeedState = new MaxSpeedState(this);
        
        //create the Transitions between States
        NormalTransition(atRestState, acceleratingState, new FuncPredicate(() => Mathf.Abs(AppliedMovementSpeed) > 0f));
        NormalTransition(acceleratingState, maxSpeedState, new FuncPredicate(() => Mathf.Approximately(Mathf.Abs(AppliedMovementSpeed), maxMovementSpeed)));
        NormalTransition(maxSpeedState, acceleratingState, new FuncPredicate(() => Mathf.Abs(AppliedMovementSpeed) < maxMovementSpeed));
        NormalTransition(acceleratingState, atRestState, new FuncPredicate(() => Mathf.Abs(AppliedMovementSpeed) == 0f));
        
        //assign the default State
        _playerVelocitySM.SetState(atRestState);
    }

    //function to create Transitions between one State to another 
    private void NormalTransition(IState fromState, IState nextState, IPredicate condition)
    {
        _playerVelocitySM.AddNormalTransition(fromState, nextState, condition);
    }

    private void Update()
    {
        _playerVelocitySM.Update();
        if (Rb.linearVelocityY < cameraHandler.YVelocityThreshold && !cameraHandler.IsPanningCoroutineActive)
        {
            cameraHandler.LerpCameraPanning(true);
        }

        if (Rb.linearVelocityY >= 0 && !cameraHandler.IsPanningCoroutineActive)
        {
            cameraHandler.LerpCameraPanning(false);
        }
    }

    private void FixedUpdate()
    {
        _playerVelocitySM.FixedUpdate();
        //Disables movement while dashing
        if (_gearTricks.IsDashing && _playerGearSwapper.CurrentGearEquipped.DaredevilGearType
            == EDaredevilGearType.Skateboard) { return; }

        if (GetComponent<RampPlayer>().isRamping)
        {
            //Jump();
            return;
        }

        // Calculate target speed based on input
        float targetSpeed = _playerInputManager.HorizontalMovement * baseSpeed * _rankCalculator.CurrentStylishRank.MaxSpeedMultiplier;
        float speedDif = targetSpeed - Rb.linearVelocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;

        AppliedMaxMovementSpeed = baseSpeed * _rankCalculator.CurrentStylishRank.MaxSpeedMultiplier;;
        AppliedAcceleration = accelRate * _rankCalculator.CurrentStylishRank.AccelerationMultiplier;
        AppliedMovementSpeed = Mathf.Pow(Mathf.Abs(speedDif) * AppliedAcceleration, velPower);
        AppliedMovementSpeed = Mathf.Clamp(AppliedMovementSpeed, float.MinValue, AppliedMaxMovementSpeed);
        AppliedMovementSpeed  *= Mathf.Sign(speedDif);

        Rb.AddForce(AppliedMovementSpeed * Vector2.right); 

        //Friction
        if(_lastGroundedTime > 0 && Mathf.Abs(_playerInputManager.HorizontalMovement) < 0.01f)
        {
            float amount = Mathf.Min(Mathf.Abs(Rb.linearVelocity.x), Mathf.Abs(frictionAmount));

            amount *= Mathf.Sign(Rb.linearVelocity.x);

            Rb.AddForce(Vector2.right * -amount, ForceMode2D.Impulse); 
        }

        switch (IsFacingRight)
        {
            //Flips player sprite to the direction they are heading to 
            case false when _playerInputManager.HorizontalMovement > 0f:
            case true when _playerInputManager.HorizontalMovement < 0f:
                Flip();
                break;
        }

        //Handles the timer for coyote time
        _lastGroundedTime = IsGrounded() ? 0f : _lastGroundedTime += Time.deltaTime;

        Jump();
        Gravity();
    }
    
    #region Jumping
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(GroundCheck.position, 0.2f, GroundLayer);
    }

    public void Jump()
    {
        //OPTIMIZE: validates if player can jump;
        bool canJump = IsGrounded() || (!IsGrounded() && _lastGroundedTime < coyoteTime);

        if (_playerInputManager.IsJumping && canJump)
        {
            Rb.linearVelocity = new Vector2(Rb.linearVelocity.x, jumpPower * _playerGearSwapper.JumpForceMultiplier);
        }
        else if (!_playerInputManager.IsJumping)
        {
            Rb.linearVelocity = new Vector2(Rb.linearVelocity.x, Rb.linearVelocity.y);
        }
    }
    #endregion

    #region Gravity
    private void Gravity()
    {
        if (Rb.linearVelocity.y < 0)
        {
            Rb.gravityScale = baseGravity * fallSpeedMultiplier;
            Rb.linearVelocity = new Vector2(Rb.linearVelocityX, Mathf.Max(Rb.linearVelocity.y, -(maxFallSpeed - 
                _gearTricks.FallingSpeedModifier)));
        }
        else
        {
            Rb.gravityScale = baseGravity;
        }
    }
    #endregion
    private void Flip() //flips character where player is facing towards
    {
        /*IsFacingRight = !IsFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;*/
        
        IsFacingRight = !IsFacingRight;
        Vector3 flipRotation = new(transform.rotation.x, IsFacingRight == true ? 0 : 180f, transform.rotation.z);
        transform.rotation = Quaternion.Euler(flipRotation);
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos)
        {
            return;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(GroundCheck.position, 0.2f);
    }
}