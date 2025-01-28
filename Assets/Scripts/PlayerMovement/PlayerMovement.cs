using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInputManager _playerInputManager;

    [Header("Components")]
    public Rigidbody2D Rb;
    public Transform GroundCheck;
    public LayerMask GroundLayer;

    [Header("Horizontal Movement Configs")]
    [SerializeField] private float baseSpeed = 8f;
    [SerializeField] private float acceleration;
    [SerializeField] private float deceleration;
    [SerializeField] private float velPower;
    [SerializeField] private float frictionAmount; 
    public float BaseSpeed { get => baseSpeed; set => baseSpeed = value; }
    public float Acceleration { get => acceleration; set => acceleration = value; }
    public float Deceleration { get => deceleration; set => deceleration = value; }
    public float VelPower { get => velPower; set => velPower = value; }
    public float FrictionAmount { get => frictionAmount; set => frictionAmount = value; }
    private bool _isFacingRight = true;
    private Vector2 _velocity; 

    [Header("Jump Configs")]
    [SerializeField]  float jumpPower;
    [SerializeField, Range(0.05f, 0.25f)] private float coyoteTime = 0.2f;
    [SerializeField, Range(0.05f, 0.25f)]private float jumpBufferTime = 0.2f;
    public float JumpPower { get => jumpPower; set => jumpPower = value; }
    public float CoyoteTime { get => coyoteTime; set => coyoteTime = value; }
    public float JumpBufferTime { get => jumpBufferTime; set => jumpBufferTime = value; }
    private float _lastGroundedTime = 0f;

    [Header("Debug Configs")]
    [SerializeField] private bool showGizmos = false;

    [Header("Gravity")]
    [SerializeField] private float baseGravity;
    [SerializeField] private float maxFallSpeed;
    [SerializeField] private float fallSpeedMultiplier;
    public float BaseGravity { get => baseGravity; set => baseGravity = value; }
    public float MaxFallSpeed { get => maxFallSpeed; set => maxFallSpeed = value; }
    public float FallSpeedMultiplier { get => fallSpeedMultiplier; set => fallSpeedMultiplier = value; }
    
    private PlayerGearSwapper _playerGearSwapper;
    private GearTricks _gearTricks;
    
    private void Awake()
    {
        _playerInputManager = GetComponent<PlayerInputManager>();
        _playerGearSwapper = GetComponent<PlayerGearSwapper>();
        _gearTricks = GetComponent<GearTricks>();
    }

    private void FixedUpdate()
    {
        //Disables movement while dashing
        if (_gearTricks.IsDashing && _playerGearSwapper.CurrentGearEquipped.DaredevilGearType
            == EDaredevilGearType.Skateboard) { return; }

        // Calculate target speed based on input
        float targetSpeed = _playerInputManager.HorizontalMovement * baseSpeed * _playerGearSwapper.HorizontalMovementMultiplier;

        float speedDif = targetSpeed - Rb.linearVelocity.x;

        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;

        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);

        Rb.AddForce(movement * Vector2.right); 

        //Friction
        if(_lastGroundedTime > 0 && Mathf.Abs(_playerInputManager.HorizontalMovement) < 0.01f)
        {
            float amount = Mathf.Min(Mathf.Abs(Rb.linearVelocity.x), Mathf.Abs(frictionAmount));

            amount *= Mathf.Sign(Rb.linearVelocity.x);

            Rb.AddForce(Vector2.right * -amount, ForceMode2D.Impulse); 
        }
        
        //Flips player sprite to the direction they are heading to 
        if (!_isFacingRight && _playerInputManager.HorizontalMovement > 0f) { Flip(); }
        else if (_isFacingRight && _playerInputManager.HorizontalMovement < 0f) { Flip(); }

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

        if (_playerInputManager.Jumping && canJump)
        {
            Rb.linearVelocity = new Vector2(Rb.linearVelocity.x, jumpPower * _playerGearSwapper.JumpForceMultiplier);
        }
        else if (!_playerInputManager.Jumping)
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
        _isFacingRight = !_isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
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