using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GearTricks : MonoBehaviour
{
    private PlayerGearSwapper _playerGearSwapper;
    private PlayerMovement _playerMovement;
    private PlayerInputManager _playerInputManager;

    [Header("Components")]
    public Rigidbody2D Rb;

    [Header("Skateboard")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    private bool isDashing = false;
    public bool IsDashing => isDashing;
    private float lastDashTime;
    private Vector2 dashDirection;

    [Header("RollerBlade")]
    [SerializeField] private float fallingSpeed;
    [SerializeField] private float slowFallTimer; 
    private float _fallingSpeed; 
    public float FallingSpeedModifier => _fallingSpeed; 

    [Header("PogoStick")]
    [SerializeField] private int maxJumps;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float inBetweenJumpCooldown; 
    private float _lastJumpTime;
    private float _lastInBetweenJumpTime; 
    private float _jumps; 


    private void Awake()
    {
        _playerGearSwapper = GetComponent<PlayerGearSwapper>();
        _playerMovement = GetComponent<PlayerMovement>();
        _playerInputManager = GetComponent<PlayerInputManager>();
        _jumps = maxJumps; 
    }

    public void Trick(InputAction.CallbackContext context)
    {
        if(_playerGearSwapper.CurrentGearEquipped.DaredevilGearType == EDaredevilGearType.Skateboard) 
        { if (context.performed) { Skateboard(); } }
        else if (_playerGearSwapper.CurrentGearEquipped.DaredevilGearType == EDaredevilGearType.RollerBlades)
        { if (context.performed) { RollerBlade(); } }
        else if (_playerGearSwapper.CurrentGearEquipped.DaredevilGearType == EDaredevilGearType.PogoStick)
        {if (context.performed) { PogoStick(); } }
    }

    private void Skateboard() 
    {
        if (Time.time >= lastDashTime + dashCooldown)
        {
            StartCoroutine(DashCoroutine());
        }
        Debug.Log("Skateboard");
    }

    private IEnumerator DashCoroutine()
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
        Rb.gravityScale = _playerMovement.BaseGravity; // Restore gravity
        Rb.linearVelocity = Vector2.zero; // Reset velocity
        isDashing = false;
    }

    private void RollerBlade() 
    {
        StartCoroutine(SlowFallCoroutine());
        //Debug.Log("RollerBlades");
    }

    private IEnumerator SlowFallCoroutine()
    {
        _fallingSpeed = fallingSpeed; 

        yield return new WaitForSeconds(slowFallTimer);

        _fallingSpeed = 0;
        Debug.Log("working");
    }
    private void PogoStick() 
    {
        if (Time.time < _lastJumpTime + jumpCooldown) { return; }
        else if(_jumps == 0) { _jumps = maxJumps; }

        if (_jumps != 0)
        {
            if(Time.time >= _lastInBetweenJumpTime + inBetweenJumpCooldown) { _jumps = maxJumps; }

            Rb.linearVelocity = new Vector2(Rb.linearVelocity.x, _playerMovement.JumpPower *
            _playerGearSwapper.JumpForceMultiplier);

            _jumps--;

            _lastInBetweenJumpTime = Time.time; 

            if(_jumps == 0) { _lastJumpTime = Time.time; }
        }
        Debug.Log("PogoStick");
    }
}
