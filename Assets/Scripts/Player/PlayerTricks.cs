using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTricks : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    private PlayerInputManager _playerInputManager;

    [Header("Components")]
    public Rigidbody2D Rb;
    [SerializeField] private ScoreCalculator scoreCalculator;
    [SerializeField] private RankCalculator rankCalculator;

    [Header("Score")]
    [SerializeField] private int scorePerTrick;

    [Header("Dash")]
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

    [Header("Double Jump")]
    [SerializeField] private int maxJumps;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float inBetweenJumpCooldown; 
    private float _lastJumpTime;
    private float _lastInBetweenJumpTime; 
    private float _jumps;

    [Header("Snapping and Taping")]
    public float tapingDuration = 5f;
    [HideInInspector] public bool isSnapping = false;
    [HideInInspector] public bool isTaping = false;


    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerInputManager = GetComponent<PlayerInputManager>();
        _jumps = maxJumps; 
    }

    public void Trick(InputAction.CallbackContext context)
    {
        switch (context.control.name)
        {
            case "a":
                if (context.performed) { Dash(); }
                break;
            case "s":
                if (context.performed) { DoubleJump(); }
                break;
            default:
                Debug.LogWarning($"Mismatch! Control name {context.control.name} was not recognized");
                break;
        }
    }

    private void AddScoreAndRank()
    {
        if (isSnapping || isTaping)
        {
            scoreCalculator.AddScore(scorePerTrick, rankCalculator.CurrentStylishRank.ScoreMultiplier);
            rankCalculator.IncreaseStylishPoints();
            //Double score and rank
            scoreCalculator.AddScore(scorePerTrick, rankCalculator.CurrentStylishRank.ScoreMultiplier);
            rankCalculator.IncreaseStylishPoints();
            if (isSnapping)
            {
                isSnapping = false;
            }
        }
        else
        {
            //if not Snapping or Taping, add score and rank only once
            scoreCalculator.AddScore(scorePerTrick, rankCalculator.CurrentStylishRank.ScoreMultiplier);
            rankCalculator.IncreaseStylishPoints();
        }
    }

    private void Dash() 
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
        //scoreCalculator.AddScore(scorePerTrick, rankCalculator.CurrentStylishRank.ScoreMultiplier);
        //rankCalculator.IncreaseStylishPoints();
        AddScoreAndRank();

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

    private void DoubleJump() 
    {
        if (Time.time < _lastJumpTime + jumpCooldown) { return; }
        else if(_jumps == 0) { _jumps = maxJumps; }

        if (_jumps != 0)
        {
            //add score
            //scoreCalculator.AddScore(scorePerTrick, rankCalculator.CurrentStylishRank.ScoreMultiplier);
            //rankCalculator.IncreaseStylishPoints();
            AddScoreAndRank();

            if (Time.time >= _lastInBetweenJumpTime + inBetweenJumpCooldown) { _jumps = maxJumps; }

            Rb.linearVelocity = new Vector2(Rb.linearVelocity.x, _playerMovement.JumpPower);

            _jumps--;

            _lastInBetweenJumpTime = Time.time; 

            if(_jumps == 0) { _lastJumpTime = Time.time; }
        }
        Debug.Log("PogoStick");
    }
}
