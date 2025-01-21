using UnityEngine;
using UnityEngine.InputSystem; 

public class PlayerMovement: MonoBehaviour
{
    [Header("Components")]
    public Rigidbody2D Rb;
    public Transform GroundCheck;
    public LayerMask GroundLayer;

    [Header("Horizontal Movement Configs")]
    public float Speed; 
    private float _speed = 8f;
    private float _horizontal;
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

    private void Awake()
    {
        _speed = Speed;
        _coyoteTime = CoyoteTime;
        _jumpPower = JumpPower; 
    }

    private void Update()
    {
        Rb.linearVelocity = new Vector2(_horizontal * _speed, Rb.linearVelocity.y); 
        if(!_isFacingRight && _horizontal > 0f) { Flip(); }
        else if(_isFacingRight && _horizontal < 0f) { Flip(); } 
        
        //Handles the timer for coyote time
        _lastGroundedTime = IsGrounded() ?  0f : _lastGroundedTime += Time.deltaTime;
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(GroundCheck.position, 0.2f, GroundLayer); 
    }

    public void Jump(InputAction.CallbackContext context)
    {
        /*if(context.performed && IsGrounded()) 
        { rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower); }
        if(context.canceled && rb.linearVelocity.y > 0 ) { new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f); }*/

        //OPTIMIZE: validates if player can jump;
        bool canJump = IsGrounded() || (!IsGrounded() && _lastGroundedTime < _coyoteTime);
        
        if (context.performed && canJump)
        {
            Rb.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
        }
    }
    private void Flip()
    {
        _isFacingRight = !_isFacingRight; 
        Vector3 localScale = transform.localScale;
        localScale.x *= -1; 
        transform.localScale = localScale;
    }

    public void Move(InputAction.CallbackContext context)
    {
        _horizontal = context.ReadValue<Vector2>().x; 
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
