using UnityEngine;
using UnityEngine.InputSystem; 

public class PlayerMovement: MonoBehaviour
{
    [Header("Components")]
    public Rigidbody2D rb;
    public Transform groundCheck;
    public LayerMask groundLayer;

    [Header("Horizontal Movement Configs")]
    [SerializeField] private float speed = 8f;
    private float horizontal;
    private bool isFacingRight = true;
    
    [Header("Jump Configs")]
    [SerializeField] private float jumpPower = 5f;
    [SerializeField, Range(0.05f, 0.25f)] private float coyoteTime = 0.2f;
    [SerializeField, Range(0.05f, 0.25f)] private float jumpBufferTime = 0.2f;
    private float lastGroundedTime = 0f;
    
    [Header("Debug Configs")]
    [SerializeField] private bool showGizmos = false;
    
    private void Update()
    {
        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y); 
        if(!isFacingRight && horizontal > 0f) { Flip(); }
        else if(isFacingRight && horizontal < 0f) { Flip(); } 
        
        //Handles the timer for coyote time
        lastGroundedTime = IsGrounded() ?  0f : lastGroundedTime += Time.deltaTime;
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer); 
    }

    public void Jump(InputAction.CallbackContext context)
    {
        /*if(context.performed && IsGrounded()) 
        { rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower); }
        if(context.canceled && rb.linearVelocity.y > 0 ) { new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f); }*/

        //OPTIMIZE: validates if player can jump;
        bool canJump = IsGrounded() || (!IsGrounded() && lastGroundedTime < coyoteTime);
        
        if (context.performed && canJump)
        {
            rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }
    }
    private void Flip()
    {
        isFacingRight = !isFacingRight; 
        Vector3 localScale = transform.localScale;
        localScale.x *= -1; 
        transform.localScale = localScale;
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x; 
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos)
        {
            return;
        }
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, 0.2f);
    }
}
