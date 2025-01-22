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
    public float Speed;
    private float _speed = 8f;
    private bool _isFacingRight = true;

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

    private void Awake()
    {
        _playerInputManager = GetComponent<PlayerInputManager>();
        _speed = Speed;
        _coyoteTime = CoyoteTime;
        _jumpPower = JumpPower;
    }

    private void Update()
    {
        Rb.linearVelocity = new Vector2(_playerInputManager.Movement.x * _speed, Rb.linearVelocity.y);
        if (!_isFacingRight && _playerInputManager.Movement.x > 0f) { Flip(); }
        else if (_isFacingRight && _playerInputManager.Movement.x < 0f) { Flip(); }

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
        /*if(context.performed && IsGrounded()) 
        { rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower); }
        if(context.canceled && rb.linearVelocity.y > 0 ) { new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f); }*/

        //OPTIMIZE: validates if player can jump;
        bool canJump = IsGrounded() || (!IsGrounded() && _lastGroundedTime < _coyoteTime);

        if (_playerInputManager.Jumping && canJump)
        {
            //Rb.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
            Rb.linearVelocity = new Vector2(Rb.linearVelocity.x, _jumpPower);
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
            Rb.linearVelocity = new Vector2(Rb.linearVelocityX, Mathf.Max(Rb.linearVelocity.y, -maxFallSpeed));
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