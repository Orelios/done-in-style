using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem; 
public class PlayerMovement: MonoBehaviour
{
    public Rigidbody2D rb;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private float speed = 8f;
    private float horizontal;
    private float jumppower = 5f;
    private bool isFacingRight = true;

    private void Update()
    {
        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y); 
        if(!isFacingRight && horizontal > 0f) { Flip(); }
        else if(isFacingRight && horizontal < 0f) { Flip(); } 
    }


    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer); 
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if(context.performed && IsGrounded()) 
        { rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumppower); }
        if(context.canceled && rb.linearVelocity.y > 0 ) { new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f); }
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
}
