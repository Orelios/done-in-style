using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;

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

    private bool _isDashing = false;
    public bool IsDashing => _isDashing;
    private float lastDashTime;
    private Vector2 dashDirection;

    private RampPlayer _player;
    private Vector2 dashLastVelocity;
    [SerializeField] private Vector2 _dashMomentumDecay = new Vector2(0.345f, 0.69f);

    [Header("Ground Pound")]
    private bool _isPounding = false;
    public bool IsPounding => _isPounding;
    [SerializeField] private float _groundPoundSpeed = 50f;
    public float poundCooldown = 1f;
    private float lastPoundTime;
    #region DO NOT DELETE
    [SerializeField] private float fallingSpeed;
    [SerializeField] private float slowFallTimer; 
    private float _fallingSpeed; 
    public float FallingSpeedModifier => _fallingSpeed;
    #endregion

    [Header("Double Jump")] 
    [SerializeField] private float doubleJumpPower;
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
    [HideInInspector] public bool canTape = false;
    [SerializeField] private SnapshotEffect _snapshot;

    [Header("Trick Move")]
    [SerializeField] private float trickTime = 1f;
    public bool canTrick = false;
    [SerializeField] private float enableTrickDuration = 2f;
    #region Temp Trick Animation
    private SpriteRenderer spriteRenderer;
    private Color startColor = Color.white;
    private Color trickColor = Color.red;
    private Color enableTrickColor = Color.blue;
    #endregion

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerInputManager = GetComponent<PlayerInputManager>();
        _snapshot = GameObject.Find("UI/Player/SnappingUI").GetComponent<SnapshotEffect>();
        _jumps = maxJumps;
        _player = GetComponent<RampPlayer>();

        #region Temp Trick Animation
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = startColor;
        }
        #endregion
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
            case "d":
                if (context.performed) { GroundPound(); }
                break;
            case "f":
                if (context.performed) { TrickMove(); }
                break;
            default:
                Debug.LogWarning($"Mismatch! Control name {context.control.name} was not recognized");
                break;
        }
    }

    public bool IsDoingTrick()
    {
        //TODO: return the other tricks
        return _isDashing;
    }

    public void AddScoreAndRank()
    {
        if (isSnapping || isTaping || canTape)
        {
            scoreCalculator.AddScore(scorePerTrick, rankCalculator.CurrentStylishRank.ScoreMultiplier);
            rankCalculator.IncreaseStylishPoints();
            //Double score and rank
            scoreCalculator.AddScore(scorePerTrick, rankCalculator.CurrentStylishRank.ScoreMultiplier);
            rankCalculator.IncreaseStylishPoints();
            if (isSnapping)
            {
                _snapshot.TriggerSnapshot();
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
        _isDashing = true;
        lastDashTime = Time.time;

        //AddScoreAndRank();

        // Dash always in the current horizontal direction
        float horizontalDirection = transform.rotation.y == 0 ? 1 : -1;
        dashDirection = new Vector2(horizontalDirection, 0).normalized;

        // Disable gravity during the dash
        Rb.gravityScale = 0;
        Rb.linearVelocity = dashDirection * dashSpeed;
        dashLastVelocity = dashDirection * dashSpeed;
        //Debug.Log(Rb.linearVelocity);

        yield return new WaitForSeconds(dashDuration);

        // End dash
        Rb.gravityScale = _playerMovement.BaseGravity; // Restore gravity
        //Rb.linearVelocity = new(Rb.linearVelocityX / 2f, Rb.linearVelocityY / 2f); // Reset velocity
        StartCoroutine(PreserveMomentum());
        //_isDashing = false;
    }

    IEnumerator PreserveMomentum()
    {
        _playerMovement.Rb.linearVelocity = dashLastVelocity;
        while (!Input.anyKeyDown)
        {
            if (_player.IsColliding) { break; }
            //dashLastVelocity = _playerMovement.Rb.linearVelocity.y;
            //_playerMovement.Rb.linearVelocity -= _dashMomentumDecay;
            Vector2 lastVelocity = _playerMovement.Rb.linearVelocity;
            if (lastVelocity.x > 0)
            {
                _playerMovement.Rb.linearVelocity = new Vector2(lastVelocity.x - _dashMomentumDecay.x, lastVelocity.y - _dashMomentumDecay.y);
            }
            else
            {
                _playerMovement.Rb.linearVelocity = new Vector2(lastVelocity.x + _dashMomentumDecay.x, lastVelocity.y - _dashMomentumDecay.y);
            }

            yield return null;
        }
        _isDashing = false;
        //Debug.Log("Momentum ends");
    }

    private void DoubleJump() 
    {
        if (Time.time < _lastJumpTime + jumpCooldown) { return; }
        else if(_jumps == 0) { _jumps = maxJumps; }

        if (_jumps != 0)
        {
            //AddScoreAndRank();

            if (Time.time >= _lastInBetweenJumpTime + inBetweenJumpCooldown) { _jumps = maxJumps; }

            Rb.linearVelocity = new Vector2(Rb.linearVelocity.x, doubleJumpPower);

            _jumps--;

            _lastInBetweenJumpTime = Time.time; 

            if(_jumps == 0) { _lastJumpTime = Time.time; }
        }
        Debug.Log("PogoStick");
    }

    private void GroundPound()
    {
        if ((Time.time >= lastPoundTime + poundCooldown) && !_playerMovement.IsGrounded())
        {
            StartCoroutine(GroundPoundCoroutine());
        }
        
    }

    private IEnumerator GroundPoundCoroutine()
    {
        _isPounding = true;
        //AddScoreAndRank();

        // Disable gravity during the dash
        Rb.gravityScale = 0;
        //Debug.Log("isPounding = " + _isPounding);
        while (_isPounding)
        {
            if (Input.GetKeyUp(KeyCode.D))
            {
                _isPounding = false;
                //Debug.Log("isPounding = " + _isPounding);
            }
            //Rb.linearVelocity = new Vector2(0f, (-1f * _groundPoundSpeed));
            _playerMovement.Rb.linearVelocity = new Vector2(0f, (-1 * _groundPoundSpeed));
            yield return null;
        }

        // End GroundPound
        Rb.gravityScale = _playerMovement.BaseGravity;
        lastPoundTime = Time.time;
        _isPounding = false;
    }
    
    private void TrickMove()
    {
        if (canTrick && spriteRenderer != null)
        {
            spriteRenderer.color = trickColor;
            canTrick = false;
            Debug.Log("TrickMove");
            AddScoreAndRank();
            StartCoroutine(RevertColorAfterTime());
        }
    }

    private IEnumerator RevertColorAfterTime()
    {
        yield return new WaitForSeconds(trickTime);
        if (spriteRenderer != null)
        {
            spriteRenderer.color = startColor;
        }
    }

    public void EnableTrick()
    {
        if (!canTrick)
        {
            StartCoroutine(EnableTrickCoroutine());
        }
    }

    private IEnumerator EnableTrickCoroutine()
    {
        
        canTrick = true;
        spriteRenderer.color = enableTrickColor;
        yield return new WaitForSeconds(enableTrickDuration);
        if (spriteRenderer.color != trickColor)
        {
            spriteRenderer.color = startColor;
        }
        canTrick = false;
    }
}
