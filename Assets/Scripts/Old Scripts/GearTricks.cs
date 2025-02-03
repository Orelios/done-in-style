using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GearTricks : MonoBehaviour
{
    private PlayerGearSwapper _playerGearSwapper;
    private OLD_PlayerMovement _playerMovement;
    private PlayerInputManager _playerInputManager;

    [Header("Components")]
    public Rigidbody2D Rb;
    [SerializeField] private TEMP_ScoreCalculator scoreCalculator;
    [SerializeField] private Temp_RankCalculator rankCalculator;

    [Header("Score")]
    [SerializeField] private int scorePerTrick;

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
        _playerMovement = GetComponent<OLD_PlayerMovement>();
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
        
        //add score
        scoreCalculator.AddScore(scorePerTrick, rankCalculator.CurrentStylishRank.ScoreMultiplier);
        rankCalculator.IncreaseStylishPoints();

        // Dash always in the current horizontal direction
        float horizontalDirection = transform.rotation.y == 0 ? 1 : -1;
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
        
        //add score
        scoreCalculator.AddScore(scorePerTrick, rankCalculator.CurrentStylishRank.ScoreMultiplier);
        rankCalculator.IncreaseStylishPoints();

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
            //add score
            scoreCalculator.AddScore(scorePerTrick, rankCalculator.CurrentStylishRank.ScoreMultiplier);
            rankCalculator.IncreaseStylishPoints();
            
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
