using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

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
    public float Speed { get => baseSpeed; set => baseSpeed = value; }
    private bool _isFacingRight = true;
    private Vector2 _velocity; 

    [Header("Jump Configs")]
    public float JumpPower;
    private float _jumpPower;
    [Range(0.05f, 0.25f)] public float CoyoteTime = 0.2f;
    private float _coyoteTime = 0.2f;
    [Range(0.05f, 0.25f)] public float JumpBufferTime = 0.2f;
    private float _jumpBufferTime = 0.2f;
    private float _lastGroundedTime = 0f;

    [Header("Debug Configs")]
    [SerializeField] private bool showGizmos = false;

    [Header("Gravity")]
    [SerializeField] private float baseGravity;
    [SerializeField] private float maxFallSpeed;
    [SerializeField] private float fallSpeedMultiplier;
    public float BaseGravity => baseGravity;
    
    private PlayerGearSwapper _playerGearSwapper;
    private GearTricks _gearTricks;

    /*
    [Header("Temp stuff")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    private bool isDashing = false;
    private float lastDashTime;
    private Vector2 dashDirection;
    */
    
    private void Awake()
    {
        _playerInputManager = GetComponent<PlayerInputManager>();
        _playerGearSwapper = GetComponent<PlayerGearSwapper>();
        _gearTricks = GetComponent<GearTricks>();
        _coyoteTime = CoyoteTime;
        _jumpPower = JumpPower;
    }

    private void Update()
    {
        //Disables movement while dashing
        if (_gearTricks.IsDashing && _playerGearSwapper.CurrentGearEquipped.DaredevilGearType 
            == EDaredevilGearType.Skateboard) { return; }
        /*
        //Makes player move depending on where they are facing
        Rb.linearVelocity = new Vector2(_playerInputManager.HorizontalMovement * baseSpeed * 
            _playerGearSwapper.HorizontalMovementMultiplier, Rb.linearVelocity.y);
        */
        //Flips player sprite to the direction they are heading to 
        if (!_isFacingRight && _playerInputManager.HorizontalMovement > 0f) { Flip(); }
        else if (_isFacingRight && _playerInputManager.HorizontalMovement < 0f) { Flip(); }

        //Handles the timer for coyote time
        _lastGroundedTime = IsGrounded() ? 0f : _lastGroundedTime += Time.deltaTime;

        Jump();
        Gravity();
    }

    private void FixedUpdate()
    {
        //Disables movement while dashing
        if (_gearTricks.IsDashing && _playerGearSwapper.CurrentGearEquipped.DaredevilGearType
            == EDaredevilGearType.Skateboard) { return; }

        // Calculate target speed based on input
        float targetSpeed = _playerInputManager.HorizontalMovement * baseSpeed * _playerGearSwapper.HorizontalMovementMultiplier;

        // Smoothly adjust the current velocity towards the target speed
        if (Mathf.Abs(_playerInputManager.HorizontalMovement) > 0.1f)
        {
            // Accelerate towards the target speed
            _velocity.x = Mathf.MoveTowards(_velocity.x, targetSpeed, acceleration * Time.fixedDeltaTime);
        }
        else
        {
            // Decelerate to 0 when no input is provided
            _velocity.x = Mathf.MoveTowards(_velocity.x, 0, deceleration * Time.fixedDeltaTime);
        }

        // Apply the calculated velocity to the Rigidbody2D
        Rb.linearVelocity = new Vector2(_velocity.x, Rb.linearVelocity.y);
    }
    /*
    public void Dash(InputAction.CallbackContext context)
    {
        if (context.performed && Time.time >= lastDashTime + dashCooldown)
        {
            StartCoroutine(DashingCoroutine());
        }
         
    }
    private IEnumerator DashingCoroutine()
    {
        isDashing = true;
        lastDashTime = Time.time;

        // Dash always in the current horizontal direction
        float horizontalDirection = Mathf.Sign(transform.localScale.x);
        dashDirection = new Vector2(horizontalDirection, 0).normalized;

        // Disable gravity during the dash
        Rb.gravityScale = 0;
        Rb.linearVelocity = dashDirection * dashSpeed;
        Debug.Log(Rb.linearVelocity);

        yield return new WaitForSeconds(dashDuration);

        // End dash
        Rb.gravityScale = baseGravity; // Restore gravity
        Rb.linearVelocity = Vector2.zero; // Reset velocity
        isDashing = false;
    }
    */
    #region Jumping
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(GroundCheck.position, 0.2f, GroundLayer);
    }

    public void Jump()
    {
        /*if(context.performed && IsGrounded()) 
        { rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower); }
        if(context.canceled && rb.linearVelocity.y > 0 ) { new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f); }*/

        //OPTIMIZE: validates if player can jump;
        bool canJump = IsGrounded() || (!IsGrounded() && _lastGroundedTime < _coyoteTime);

        if (_playerInputManager.Jumping && canJump)
        {
            //Rb.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
            Rb.linearVelocity = new Vector2(Rb.linearVelocity.x, _jumpPower * _playerGearSwapper.JumpForceMultiplier);
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