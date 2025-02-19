using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    // Gravity Settings
    public float gravityStrength;
    public float gravityScale;
    public float fallGravityMult;
    public float maxFallSpeed;
    public float fastFallGravityMult;
    public float maxFastFallSpeed;

    // Jump Settings
    public float jumpHeight;
    public float jumpTimeToApex;
    public float jumpForce;

    // Jump Hang
    public float jumpCutGravityMult;
    public float jumpHangGravityMult;
    public float jumpHangTimeThreshold;
    public float jumpHangAccelerationMult;
    public float jumpHangMaxSpeedMult;

    // Assists
    public float coyoteTime;
    public float jumpInputBufferTime;

    private Rigidbody2D rb;
    private bool isGrounded;
    private float lastJumpTime;
    private PlayerInputManager _playerInputManager; 

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _playerInputManager = GetComponent<PlayerInputManager>();
    }

    // Unity Callback, called when the inspector updates
    private void OnValidate()
    {
        // Calculate gravity strength using the formula (gravity = 2 * jumpHeight / timeToJumpApex^2) 
        gravityStrength = -(2 * jumpHeight) / (jumpTimeToApex * jumpTimeToApex);

        // Calculate the rigidbody's gravity scale (ie: gravity strength relative to Unity's gravity value, see project settings/Physics2D)
        gravityScale = gravityStrength / Physics2D.gravity.y;

        // Calculate jumpForce using the formula (initialJumpVelocity = gravity * timeToJumpApex)
        jumpForce = Mathf.Abs(gravityStrength) * jumpTimeToApex;
    }

    private void Update()
    {
        isGrounded = CheckGrounded();
        HandleJumping();
    }

    private void HandleJumping()
    {
        // Jump
        if (isGrounded && _playerInputManager.IsJumping)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        // Coyote time (grace period for jumping after falling off a platform)
        if (!isGrounded && Time.time - lastJumpTime < coyoteTime && _playerInputManager.IsJumping)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        // Jump Cut (increase gravity when the jump button is released early)
        if (!_playerInputManager.IsJumping && rb.linearVelocity.y > 0f)
        {
            rb.gravityScale = gravityScale * jumpCutGravityMult;
        }
        else
        {
            rb.gravityScale = gravityScale;
        }

        // Jump Hang
        if (rb.linearVelocity.y < jumpHangTimeThreshold)
        {
            rb.gravityScale = gravityScale * jumpHangGravityMult;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Min(rb.linearVelocity.y, jumpHangMaxSpeedMult * maxFallSpeed));
        }
    }

    private bool CheckGrounded()
    {
        // This function would check if the player is grounded using a collider, raycast, or other method.
        // You can implement it based on your needs (e.g., `Physics2D.OverlapCircle()` for detecting ground).
        return Physics2D.OverlapCircle(transform.position, 0.1f, LayerMask.GetMask("Ground"));
    }
}
