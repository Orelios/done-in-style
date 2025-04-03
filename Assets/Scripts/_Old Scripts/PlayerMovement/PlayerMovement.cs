using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using FMOD.Studio; 
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
    [Tooltip("Insert the fake acceleration that handles how much momentum is retained when switching directions; the higher the value, the less momentum is retained")]
    [SerializeField, Range(1f, 5f)] private float fakeAcceleration;
    public float BaseSpeed { get => baseSpeed; set => baseSpeed = value; }
    public float Acceleration { get => acceleration; set => acceleration = value; }
    public float Deceleration { get => deceleration; set => deceleration = value; }
    public float VelPower { get => velPower; set => velPower = value; }
    public float FrictionAmount { get => frictionAmount; set => frictionAmount = value; }
    public float AppliedMovementSpeed { get;  set; }
    public float AppliedAcceleration { get; private set; }
    public bool IsFacingRight = true;
    private Vector2 _velocity; 
    #endregion

    #region Jump
    [Header("Jump Configs")]
    [Tooltip("Insert here how long Coyote Time will run after the player goes off of a ledge; this is how long the Player can still jump after going off a ledge")]
    [SerializeField, Range(0.05f, 1f)] private float coyoteTime = 0.2f;
    [Tooltip("UNIMPLEMENTED: Insert here how long the Player's jump input will buffer; this is how long the Player's jump input is saved when in the air to help time the next jump when landing on the ground")]
    [SerializeField, Range(0.05f, 0.25f)]private float jumpBufferTime = 0.2f;

    // Jump Settings
    [SerializeField] private float jumpHeight;
    [SerializeField] private float jumpTimeToApex;
    [SerializeField] private float jumpTimeHeight = 0.5f;
    private float _jumpTimeHeight;
    private float _jumpForce;
    private bool _canJump;
    private bool _isJumping; 
    // Jump Hang
    [SerializeField] private float jumpCutGravityMult;
    [SerializeField] private float jumpHangGravityMult;
    [SerializeField] private float jumpHangTimeThreshold;
    [SerializeField] private float jumpHangAccelerationMult;
    [SerializeField] private float jumpHangMaxSpeedMult;

    private float _lastGroundedTime = 0f;
    public float CoyoteTime { get => coyoteTime; set => coyoteTime = value; }
    public float JumpBufferTime { get => jumpBufferTime; set => jumpBufferTime = value; }
    public float JumpHeight { get => jumpHeight; set => jumpHeight = value; }
    public float JumpTimeToApex { get => jumpTimeToApex; set => jumpTimeToApex = value; }
    public float JumpForce { get => _jumpForce; set => _jumpForce = value; }
    public float JumpCutGravityMult { get => jumpCutGravityMult; set => jumpCutGravityMult = value; }
    public float JumpHangGravityMult { get => jumpHangGravityMult; set => jumpHangGravityMult = value; }
    public float JumpHangTimeThreshold { get => jumpHangTimeThreshold; set => jumpHangTimeThreshold = value; }
    public float JumpHangAccelerationMult { get => jumpHangAccelerationMult; set => jumpHangAccelerationMult = value; }
    public float JumpHangMaxSpeedMult { get => jumpHangMaxSpeedMult; set => jumpHangMaxSpeedMult = value; }
    public bool CanJump { get => _canJump; set => _canJump = value; }


    private float _lastJumpTime;
    #endregion

    #region Gravity
    [Header("Gravity")]
    [Tooltip("Insert here the base gravity value of the Player; this is how fast the Player will fall down")]
    [SerializeField] private float baseGravity;
    [Tooltip("Insert here the maximum falling speed the Player will have when falling down")]
    [SerializeField] private float maxFallSpeed;

    [SerializeField] private float fallGravityMult;
    private float _gravityStrength;
    private float _gravityScale;
    public float BaseGravity { get => baseGravity; set => baseGravity = value; }
    public float MaxFallSpeed { get => maxFallSpeed; set => maxFallSpeed = value; }
    public float FallGravityMult { get => fallGravityMult; set => fallGravityMult = value; }
    public float GravityStrenght { get => _gravityStrength; set => _gravityStrength = value; }
    public float GravityScale { get => _gravityScale; set => _gravityScale = value; }




    //public float maxFallSpeed;
    #endregion

    #region Others
    [Header("Debug Configs")]
    [SerializeField] private bool showGizmos = false;
    
    [Header("Test")] 
    [SerializeField] private float maxMovementSpeed;
    public float MaxMovementSpeed => baseSpeed;
    public float AppliedMaxMovementSpeed { get; private set; }
    private Player _player;
    private Quaternion _originalRotation;

    //Audio
    public EventInstance _playerSkatingGround;
    public EventInstance _playerSkatingAir;
    public EventInstance _playerMovement;

    [NonSerialized] public string groundIntensity;
    [NonSerialized] public string airIntensity;
    [NonSerialized] public string wallIntensity;

    #endregion

    #region Private Variables
    private PlayerInputManager _playerInputManager;
    private PlayerTricks _playerTricks;
    private RankCalculator _rankCalculator;
    private Vector2 _groundChecker;
    private RampPlayer _rampPlayer;
    private PlayerRailing _playerRailing;
    #endregion
    
    private void Awake()
    {
        _playerInputManager = GetComponent<PlayerInputManager>();
        _playerTricks = GetComponent<PlayerTricks>();
        _rankCalculator = FindFirstObjectByType<RankCalculator>();
        _rampPlayer = GetComponent<RampPlayer>();
        _playerRailing = GetComponent<PlayerRailing>(); 
        
        _player = GetComponent<Player>();
        _originalRotation = quaternion.identity;
        
        // Calculate gravity strength using the formula (gravity = 2 * jumpHeight / timeToJumpApex^2) 
        _gravityStrength = -(2 * jumpHeight) / (jumpTimeToApex * jumpTimeToApex);

        // Calculate the rigidbody's gravity scale (ie: gravity strength relative to Unity's gravity value, see project settings/Physics2D)
        _gravityScale = _gravityStrength / Physics2D.gravity.y;

        // Calculate jumpForce using the formula (initialJumpVelocity = gravity * timeToJumpApex)
        _jumpForce = Mathf.Abs(_gravityStrength) * jumpTimeToApex;

        /*_playerSkatingGround = AudioManager.instance.CreateInstance(FMODEvents.instance.PlayerSkating);
        _playerSkatingAir = AudioManager.instance.CreateInstance(FMODEvents.instance.PlayerSkatingAir);*/

    }

    private void Start()
    {
        _playerSkatingGround = AudioManager.instance.CreateInstance(FMODEvents.instance.PlayerSkating);
        _playerSkatingAir = AudioManager.instance.CreateInstance(FMODEvents.instance.PlayerSkatingAir);
        _playerMovement = AudioManager.instance.CreateInstance(FMODEvents.instance.PlayerMovement);
        _playerMovement.start();

        groundIntensity = "ground_intensity";
        airIntensity = "air_intensity";
        wallIntensity = "wall_intensity";
    }

    //NEW JUMP STUFF
    //NOTE: OnValidate seems to work as an editor-only function, so better to set up the gravity calculations on Awake or Start
    /*private void OnValidate()
    {
        // Calculate gravity strength using the formula (gravity = 2 * jumpHeight / timeToJumpApex^2) 
        _gravityStrength = -(2 * jumpHeight) / (jumpTimeToApex * jumpTimeToApex);

        // Calculate the rigidbody's gravity scale (ie: gravity strength relative to Unity's gravity value, see project settings/Physics2D)
        _gravityScale = _gravityStrength / Physics2D.gravity.y;

        // Calculate jumpForce using the formula (initialJumpVelocity = gravity * timeToJumpApex)
        _jumpForce = Mathf.Abs(_gravityStrength) * jumpTimeToApex;
    }*/

    private void RotatePlayer()
    {
            var contact = Physics2D.OverlapBox(GroundCheck.position, new(0.9f, 0.1f), 0f, GroundLayer);        
        
        if (IsGrounded())
        {
            /*_player.Sprite.gameObject.transform.rotation = Quaternion.Euler(transform.localEulerAngles.x, 
                transform.localEulerAngles.y, 
                Mathf.Approximately(transform.localEulerAngles.y, 0) ? contact.transform.localEulerAngles.z : -contact.transform.localEulerAngles.z);*/
            
            /*if (Mathf.Abs(currentTilt - previousTilt) > tiltThreshold)
            {
                transform.rotation = Quaternion.Euler(transform.localEulerAngles.x, 
                    transform.localEulerAngles.y, 
                    contact.transform.localEulerAngles.z);
                previousTilt = currentTilt;
            }*/
            
            //var zTilt = Rb.linearVelocityY > 0 ? : ;
            transform.rotation = Quaternion.Euler(transform.localEulerAngles.x, 
                transform.localEulerAngles.y, 
                //Rb.linearVelocityY < 0 ? transform.localEulerAngles.y : -transform.localEulerAngles.y,
                contact.transform.localEulerAngles.z * (IsFacingRight ? 1f : -1f));
        }
        else
        {
            /*_player.Sprite.gameObject.transform.rotation = _originalRotation;*/
            transform.rotation = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y, 0f);
        }
    }
    
    private void FixedUpdate()
    {
        RotatePlayer();
        /*
        switch (IsFacingRight)
        {
            //Flips player sprite to the direction they are heading to 
            case false when _playerInputManager.HorizontalMovement > 0f:
            case true when _playerInputManager.HorizontalMovement < 0f:
                Flip();
                break;
        }
        */
        //Disables movement while dashing

        if (_playerTricks.IsDashing || _playerTricks.IsPounding) { return; }

        if (GetComponent<RampPlayer>().isRamping){return;}

        if (IsGrounded()) { _playerTricks.CanDoubleJump(); }

        HorizontalMovement();
        
        Jump();
        Gravity();
    }

    #region Horizontal Movement
    public void HorizontalMovement()
    {
        if (_playerRailing.IsMovingOnRail && PlayerOnRailing()) { return; }
        // Calculate target speed based on input
        float targetSpeed = _playerInputManager.HorizontalMovement * baseSpeed * _rankCalculator.CurrentStylishRank.MaxSpeedMultiplier;
        float speedDif = targetSpeed - Rb.linearVelocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;

        //if (Mathf.Abs(targetSpeed) < 0.01f) { Rb.linearVelocity = new Vector2(0f, Rb.linearVelocityY); }

        AppliedMaxMovementSpeed = baseSpeed * _rankCalculator.CurrentStylishRank.MaxSpeedMultiplier;
        AppliedAcceleration = accelRate * _rankCalculator.CurrentStylishRank.AccelerationMultiplier;
        AppliedMovementSpeed = Mathf.Pow(Mathf.Abs(speedDif) * AppliedAcceleration, velPower);
        AppliedMovementSpeed = Mathf.Clamp(AppliedMovementSpeed, float.MinValue, AppliedMaxMovementSpeed);
        AppliedMovementSpeed *= Mathf.Sign(speedDif);

        Rb.AddForce(AppliedMovementSpeed * Vector2.right);

        //Friction
        if (IsGrounded() && Mathf.Abs(_playerInputManager.HorizontalMovement) < 0.01f || _lastGroundedTime > 0 && Mathf.Abs(_playerInputManager.HorizontalMovement) < 0.01f)
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

        playerMovementSound();
    }
    #endregion

    #region Jumping
    public bool IsGrounded()
    {
        //return Physics2D.OverlapCircle(GroundCheck.position, 0.2f, GroundLayer);
        //TODO: creates adjustable overlapBox dimensions variable
        return Physics2D.OverlapBox(GroundCheck.position, new(0.9f, 0.1f), 0f, GroundLayer);
    }

    public bool IsSpringBoarding()
    {
        return Physics2D.OverlapBox(GroundCheck.position, new(0.9f, 0.1f), 0f, LayerMask.GetMask("SpringBoard"));
    }

    public bool PlayerOnRailing()
    {
        return Physics2D.OverlapBox(GroundCheck.position, new(0.9f, 0.1f), 0f, LayerMask.GetMask("Railing"));
    }

    public void Jump()
    {
        if (_playerTricks.IsWallRiding && !_playerTricks.CanDestroy) { return; }

        //Handles the timer for coyote time
        _lastGroundedTime = IsGrounded() ? 0f : _lastGroundedTime += Time.deltaTime;

        _canJump = IsGrounded() || (!IsGrounded() && _lastGroundedTime < coyoteTime);
 
        JumpTimeHeightCooldown();

        if (_canJump) {_isJumping = true; }

        if(!_playerInputManager.IsJumping && !CanJump) { _isJumping = false; }

        if (!_playerInputManager.IsJumping)// Jump Cut (increase gravity when the jump button is released early)
        {
            _jumpForce = Mathf.Abs(_gravityStrength) * jumpTimeToApex;
            Rb.gravityScale = _gravityScale * jumpCutGravityMult;
            Rb.linearVelocity = new Vector2(Rb.linearVelocity.x, Mathf.Max(Rb.linearVelocity.y, -maxFallSpeed));
        }
        // Jump
        if (_playerInputManager.IsJumping && _isJumping)
        {
            if(JumpTimeHeightCooldown() >= 0)
            {
                Rb.linearVelocity = new Vector2(Rb.linearVelocity.x, _jumpForce);
            }

            if(JumpTimeHeightCooldown() < 0)
            {
                _isJumping = false;
            }
            //_playerTricks.IsSliding = false;
        }

        //Debug.Log(_isJumping);
        if (!_canJump && Rb.linearVelocity.y > 0 && Rb.linearVelocity.y < jumpHangTimeThreshold) // Jump Hang
        {
            Rb.gravityScale = _gravityScale * jumpHangGravityMult;
            //Rb.linearVelocity = new Vector2(Rb.linearVelocity.x, Mathf.Min(Rb.linearVelocity.y, jumpHangMaxSpeedMult * maxFallSpeed));
            Rb.linearVelocity = new Vector2(Rb.linearVelocity.x * jumpHangAccelerationMult, Mathf.Min(Rb.linearVelocity.y, jumpHangMaxSpeedMult * maxFallSpeed));
        }

        PlayJumpSound();
    }

    private float JumpTimeHeightCooldown()
    {
        if (IsGrounded())
        {
            _jumpTimeHeight = jumpTimeHeight;
        }

        if (_playerInputManager.IsJumping)
        {
            _jumpTimeHeight -= Time.fixedDeltaTime;  
        }

        return _jumpTimeHeight; 
    }
    #endregion

    #region Gravity
    private void Gravity()
    { 

        if (_playerTricks.IsWallRiding && !_playerTricks.CanDestroy) { _playerTricks.WallRiding(); return; }

        if (Rb.linearVelocity.y < 0) // Player is falling
        {
            Rb.gravityScale = _gravityScale * fallGravityMult;

            Rb.linearVelocity = new Vector2(Rb.linearVelocity.x, Mathf.Max(Rb.linearVelocity.y, -maxFallSpeed));
        }
        else //While grounded
        {
            Rb.gravityScale = _gravityScale;
        }
    }
    #endregion
    public void Flip() //flips character where player is facing towards
    {
        if (_player.Railing.IsMovingOnRail && PlayerOnRailing())
        {
            return;
        }
        /*IsFacingRight = !IsFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;*/
        
        IsFacingRight = !IsFacingRight;
        if (IsGrounded() && _rampPlayer.HasExitedRamp)
        {
            if ((IsFacingRight && (Rb.linearVelocityX < 0f)) || (!IsFacingRight && (Rb.linearVelocityX > 0f)))
            {
                Rb.linearVelocity = new Vector2(-Rb.linearVelocityX / fakeAcceleration, Rb.linearVelocityY);
            }
        }

        Vector3 flipRotation = new(transform.rotation.x, IsFacingRight ? 0 : 180f, transform.rotation.z);
        transform.rotation = Quaternion.Euler(flipRotation);
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos)
        {
            return;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(GroundCheck.position, new(0.9f, 0.1f));
    }

    public void PlayJumpSound()
    {
        if (_playerInputManager.IsJumping && IsGrounded())
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.PlayerJump, this.transform.position);
        } 
    }
    
    private void playerMovementSound()
    {
        if(Rb.linearVelocityX != 0 && IsGrounded())
        {
            PLAYBACK_STATE playbackState;
            _playerMovement.getPlaybackState(out playbackState);
            //_playerSkatingGround.start();

            if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
            {
                //_playerSkatingGround.start();
            }
            else if (playbackState.Equals(PLAYBACK_STATE.PLAYING))
            {
                //_playerSkatingGround.setPaused(false);
                _playerMovement.setParameterByName(groundIntensity, 1);
                //Debug.Log("Set Parameter");
            }
            
            
        }
        else if (Rb.linearVelocityX == 0 || !IsGrounded() || !_playerTricks.IsWallRiding)
        {
            //_playerSkatingGround.setPaused(true);
            _playerMovement.setParameterByName(groundIntensity, 0);
        }

        if (Rb.linearVelocityX != 0 && !IsGrounded())
        {
            PLAYBACK_STATE playbackState;
            _playerSkatingAir.getPlaybackState(out playbackState);

            _playerMovement.setParameterByName(airIntensity, 1);
            if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
            {
                //_playerSkatingAir.start();
            }
            else if (playbackState.Equals(PLAYBACK_STATE.PLAYING))
            {
                //_playerSkatingAir.setPaused(false);

            }

        }
        else if (Rb.linearVelocityX == 0 || IsGrounded() || !_playerTricks.IsWallRiding)
        {
            _playerMovement.setParameterByName(airIntensity, 0);
            //_playerSkatingAir.setPaused(true);
        }
    }
}