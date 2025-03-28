using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;
using FMOD.Studio;
using System.Collections.Generic;

public class PlayerTricks : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    private PlayerInputManager _playerInputManager;

    public EventInstance _playerSkatingWallRide;

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

    private RampPlayer _rampPlayer;
    private Vector2 dashLastVelocity;
    [SerializeField] private Vector2 _dashMomentumDecay = new Vector2(0.345f, 0.69f);
    private Coroutine dashCor, preserveMomentumCor;

    [Header("Ground Pound")]
    private bool _isPounding = false;
    public bool IsPounding => _isPounding;
    [SerializeField] private float _groundPoundSpeed = 50f;
    [SerializeField, Range(0.1f, 1f)] private float momentumRetainFactor; 
    public float poundCooldown = 1f;
    private float lastPoundTime;
    private VFXManager _vfx;
    public Coroutine PoundingCor; 
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

    [SerializeField] private bool canDoubleJump; 
    private bool _canDestroy = false;
    public bool CanDestroy => _canDestroy;
    [SerializeField] private float _canDetroyDuration = 0.5f;

    [Header("Snapping and Taping")]
    public float tapingDuration = 5f;
    [HideInInspector] public bool isSnapping = false;
    [HideInInspector] public bool isTaping = false;
    [HideInInspector] public bool canTape = false;
    [SerializeField] private SnapshotEffect _snapshot;

    [Header("Wall Ride")]
    [SerializeField] private float wallRidingGravity; 
    private bool _isWallRiding;
    private Wall _wall;
    public bool IsWallRiding { get => _isWallRiding; set => _isWallRiding = value; }

    [Header("Sliding")]
    [SerializeField] private float baseColliderOffsetY = -0.02f;
    [SerializeField] private float baseColliderSizeY = 1.94f;
    [SerializeField] private float colliderOffsetY;
    [SerializeField] private float colliderSizeY; 
    private bool _isSliding; 

    public float ColliderOffsetY { get => colliderOffsetY; set => colliderOffsetY = value; }
    public float ColliderSizeY { get => colliderSizeY; set => colliderSizeY = value; }
    public bool IsSliding { get => _isSliding; set => _isSliding = value; }

    [Header("Trick Move")]
    [SerializeField] private float trickTime = 1f;
    public bool canTrick = false;
    [SerializeField] private float enableTrickDuration = 2f;
    private GameObject _trickObject;
    private bool _destroyedObject = false;

    [SerializeField] private GameObject trickSprite;
    [SerializeField] private List<Sprite> trickSprites = new List<Sprite>();
    private Color invisible = new Color(1f, 1f, 1f, 0f);
    private Color visible = new Color(1f, 1f, 1f, 1f);
    private int lastTrickSpriteIndex = -1, randomSpriteIndex;
    private SpriteRenderer playerSprite;
    //private Color startColor = Color.white;
    //private Color trickColor = Color.red;
    //private Color enableTrickColor = Color.white;

    private Player _player;
    private PlayerRailing _playerRailing;
    private Coroutine enableTrickCor, revertSpriteCor;

    [Header("VFX")]
    [SerializeField] private float jumpPadTimer = 0.2f;
    public bool isOnJumpPad = false;

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerInputManager = GetComponent<PlayerInputManager>();
        _snapshot = GameObject.Find("UI/Player/SnappingUI").GetComponent<SnapshotEffect>();
        _jumps = maxJumps;
        canDoubleJump = true; 
        _rampPlayer = GetComponent<RampPlayer>();
        _vfx = GetComponentInChildren<VFXManager>();
        _wall = GetComponent<Wall>();
        
        _player = GetComponent<Player>();
        _playerRailing = GetComponent<PlayerRailing>();

        lastDashTime = Time.time - dashCooldown;
        lastPoundTime = Time.time - poundCooldown;

        //_playerSkatingWallRide = AudioManager.instance.CreateInstance(FMODEvents.instance.PlayerSkatingWallRide);
        #region Temp Trick Animation

        //_player = GetComponent<Player>();
        playerSprite = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        if (playerSprite != null)
        {
            //playerSprite.color = startColor;
        }
        #endregion


    }
    /*
    public void Trick(InputAction.CallbackContext context)
    {
        switch (context.control.name)
        {
            case "a":
                if (context.performed) { Dash(); }
                break;
            case "upArrow":
                if (context.performed) { DoubleJump(); }
                break;
            case "s":
                if (context.performed) { GroundPound(); }
                break;
            case "space":
                if (context.performed) { TrickMove(); }
                break;
            case "downArrow":
                if (context.performed) 
                {
                    if (!_playerMovement.IsGrounded()) { _isPressingDown = true; }
                    else if (_playerMovement.IsGrounded() && !_player.RailGrind.IsOnRail) { _isSliding = true; Sliding(); }
                }
                else if (context.canceled) 
                {
                    _playerSkatingWallRide.stop(STOP_MODE.ALLOWFADEOUT);
                    if (!_playerMovement.IsGrounded()) { _isPressingDown = false; }
                    else if (_playerMovement.IsGrounded()) { _isSliding = false; Sliding(); }
                }
                break; 
            default:
                Debug.LogWarning($"Mismatch! Control name {context.control.name} was not recognized");
                break;
        }
    }
    */
    #region Tricks
    public void DashInput(InputAction.CallbackContext context)
    {
        if (context.performed) { Dash(); }
    }

    public void DoubleJumpInput(InputAction.CallbackContext context)
    {
        if (context.performed) { DoubleJump(); }
    }
    public void GroundPoundInput(InputAction.CallbackContext context)
    {
        if (context.performed) { GroundPound(); }
    }
    public void TrickMoveInput(InputAction.CallbackContext context)
    {
        if (context.performed) { TrickMove(); }
    }
    public void SlidingInput(InputAction.CallbackContext context)
    {
        if (context.performed) {if (_playerMovement.IsGrounded() && !_player.Railing.IsMovingOnRail)
            { _isSliding = true; Sliding(); }}

        if (context.canceled){if (_playerMovement.IsGrounded()) 
            { _isSliding = false; Sliding(); }}
    }
    #endregion
    #region Scoring
    public bool IsDoingTrick()
    {
        //TODO: return the other tricks
        return _isDashing;
    }

    public void AddScoreAndRank()
    {
        if (isSnapping || isTaping || canTape)
        {
            scoreCalculator.IncreaseScoreInstant(scorePerTrick, rankCalculator.CurrentStylishRank.ScoreMultiplier);
            rankCalculator.IncreaseStylishPoints();
            //Double score and rank
            scoreCalculator.IncreaseScoreInstant(scorePerTrick, rankCalculator.CurrentStylishRank.ScoreMultiplier);
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
            scoreCalculator.IncreaseScoreInstant(scorePerTrick, rankCalculator.CurrentStylishRank.ScoreMultiplier);
            rankCalculator.IncreaseStylishPoints();
        }
    }
    #endregion

    #region Dash
    private void Dash() 
    {
        if (Time.time >= lastDashTime + dashCooldown)
        {
            dashCor = StartCoroutine(DashCoroutine());
            AudioManager.instance.PlayOneShot(FMODEvents.instance.PlayerDash, this.transform.position);
        }
    }

    private IEnumerator DashCoroutine()
    {
        _vfx.CallDashVFX();
        _isDashing = true;
        if (IsDashing)
        {
            _isPounding = false;
            StopPounding();
        }
        //_vfx.CallDashVFX();
        lastDashTime = Time.time;

        switch (_playerMovement.IsFacingRight)
        {
            //Flips player sprite to the direction they are heading to 
            case false when _playerInputManager.HorizontalMovement > 0f:
            case true when _playerInputManager.HorizontalMovement < 0f:
                _playerMovement.Flip();
                break;
        }

        // Dash always in the current horizontal direction
        //float horizontalDirection = transform.rotation.y == 0 ? 1 : -1;
        int horizontalDirection = _playerMovement.IsFacingRight? 1 : -1;
        dashDirection = new Vector2(horizontalDirection, 0).normalized;

        // Disable gravity during the dash
        Rb.gravityScale = 0;
        Rb.linearVelocity = dashDirection * dashSpeed;
        dashLastVelocity = dashDirection * dashSpeed;
        //Debug.Log(Rb.linearVelocity);

        yield return new WaitForSeconds(dashDuration);

        // End dash
        _vfx.CallDashVFX();
        Rb.gravityScale = _playerMovement.BaseGravity; // Restore gravity
        //Rb.linearVelocity = new(Rb.linearVelocityX / 2f, Rb.linearVelocityY / 2f); // Reset velocity
        preserveMomentumCor = StartCoroutine(PreserveMomentum());
        _playerMovement.JumpForce = _playerMovement.Rb.linearVelocityY; 
        _isDashing = false;
    }

    IEnumerator PreserveMomentum()
    {
        _playerMovement.Rb.linearVelocity = dashLastVelocity;
        while (!Input.anyKeyDown)
        {
            if (_rampPlayer.IsColliding) { break; }
            if (_playerMovement.IsGrounded()) { break; }
            if (IsWallRiding) { break; }
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
        //_isDashing = false;
        //playerSprite.color = trickColor;
        //StartCoroutine(RevertColorAfterTime());
        //Debug.Log("Momentum ends");
    }
    #endregion

    #region DoubleJump
    private void DoubleJump() 
    {
        if (!_playerMovement.IsGrounded() || IsWallRiding)
        {
            if (!canDoubleJump) { return; }
            _isPounding = false;
            StopPounding();
            if (dashCor != null)
            {
                StopCoroutine(dashCor);
                dashCor = null;
                Rb.gravityScale = _playerMovement.BaseGravity; // Restore gravity
                _playerMovement.JumpForce = _playerMovement.Rb.linearVelocityY;
                _isDashing = false;
            }
  
            //AddScoreAndRank();
            StartCoroutine(DoubleJumpDestroy());

            _vfx.CallDoubleJumpVFX();

            Rb.linearVelocity = new Vector2(Rb.linearVelocity.x, doubleJumpPower);

            canDoubleJump = false; 
           
            AudioManager.instance.PlayOneShot(FMODEvents.instance.PlayerJump, this.transform.position);
            //Debug.Log("PogoStick");
        }
    }

    public void CanDoubleJump()
    {
        canDoubleJump = true;
        
    }

    private IEnumerator DoubleJumpDestroy()
    {
        _canDestroy = true;
        _isWallRiding = false; 
        yield return new WaitForSeconds(_canDetroyDuration);
        _canDestroy = false;
        if (_wall != null && _wall._canWallRide)
        {
            _isWallRiding = true;
        }
    }
    #endregion

    #region GroundPound
    private void GroundPound()
    {
        if ((Time.time >= lastPoundTime + poundCooldown) && !_playerMovement.IsGrounded())
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.PlayerGroundPound, this.transform.position);
            PoundingCor = StartCoroutine(GroundPoundCoroutine());
        }
        
    }

    private IEnumerator GroundPoundCoroutine()
    {
        var velocityX = Rb.linearVelocityX;
        _isPounding = true;
        _vfx.CallGroundPoundDiveVFX();
        //AddScoreAndRank();

        // Disable gravity during the dash
        Rb.gravityScale = 0;
        //Debug.Log("isPounding = " + _isPounding);
        while (_isPounding && !_playerMovement.IsGrounded())
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
        _vfx.CallGroundPoundLandVFX();
        Rb.gravityScale = _playerMovement.BaseGravity;
        Rb.linearVelocity = new Vector2(velocityX * momentumRetainFactor, Rb.linearVelocityY);
        lastPoundTime = Time.time;
        _isPounding = false;
    }

    public void StopPounding()
    {
        if(PoundingCor != null)
        {
            StopCoroutine(PoundingCor);
            PoundingCor = null;

            var velocityX = Rb.linearVelocityX;
            _vfx.CallGroundPoundLandVFX();
            Rb.gravityScale = _playerMovement.BaseGravity;
            Rb.linearVelocity = new Vector2(velocityX * momentumRetainFactor, Rb.linearVelocityY);
            lastPoundTime = Time.time;
            _isPounding = false;
        }
       
    }
    #endregion

    #region WallRiding

    public void WallRiding()
    {
        if (_isWallRiding) 
        {

            //_playerMovement._playerSkatingGround.setPaused(true);
            //_playerMovement._playerSkatingAir.setPaused(true);

            PLAYBACK_STATE playbackState;
            _playerSkatingWallRide.getPlaybackState(out playbackState);

            _playerMovement._playerMovement.setParameterByName(_playerMovement.groundIntensity, 0);
            _playerMovement._playerMovement.setParameterByName(_playerMovement.airIntensity, 0);
            _playerMovement._playerMovement.setParameterByName(_playerMovement.wallIntensity, 1);

            _playerMovement.Rb.gravityScale = wallRidingGravity;

            Rb.linearVelocity = new Vector2(Rb.linearVelocity.x * _playerMovement.JumpHangAccelerationMult,
                Mathf.Min(Rb.linearVelocity.y, _playerMovement.JumpHangMaxSpeedMult * _playerMovement.MaxFallSpeed));

            Rb.linearVelocityX = Mathf.Clamp(Rb.linearVelocityX, -_playerMovement.AppliedMaxMovementSpeed * 2,
                _playerMovement.AppliedMaxMovementSpeed *2);

            if (!_wall._hasGivenScore)
            {
                AddScoreAndRank();
                _wall._hasGivenScore = true;
                if (_wall.graffiti != null)
                {
                    _wall.graffiti.StartGraffiti();
                }
            }
            DisableCanTrick();
        }

        
    }

    public void PlayWallRideLanding()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.PlayerLanding, this.transform.position);
    }

    public void GetWall(Wall wall)
    {
        _wall = wall;
    }

    public void ResetWallRideValues() 
    {  
        if (!_isWallRiding)
        {
            //Debug.Log("I should stop." + _playerSkatingWallRide.ToString());
            //_playerSkatingWallRide.setPaused(true);
            _playerMovement._playerMovement.setParameterByName(_playerMovement.wallIntensity, 0);
            _playerMovement.Rb.gravityScale = _playerMovement.GravityScale;
            //if (!_wall.hasTricked) { EnableTrick(_wall.gameObject); }
        }
    }
    #endregion

    #region Sliding
    public void Sliding()
    {
        if (_playerMovement.IsGrounded() && _isSliding)
        {
            Rb.gravityScale = 0;
            gameObject.GetComponent<CapsuleCollider2D>().offset = new Vector2(0, colliderOffsetY); 
            gameObject.GetComponent<CapsuleCollider2D>().size = new Vector2(1, colliderSizeY);
            Rb.gravityScale = _playerMovement.GravityScale;
        }

        if (!_isSliding)
        {
            gameObject.GetComponent<CapsuleCollider2D>().offset = new Vector2(0, baseColliderOffsetY);
            gameObject.GetComponent<CapsuleCollider2D>().size = new Vector2(1, baseColliderSizeY);
        }
    }
    #endregion

    #region TrickMove
    private void TrickMove()
    {
        if (_destroyedObject && canTrick && _playerMovement.IsGrounded() != true && playerSprite != null)
        {
            //Debug.Log("trick stop cor");
            _destroyedObject = false;
            //playerSprite.color = trickColor;
            
            AudioManager.instance.PlayOneShot(FMODEvents.instance.PlayerTrick, this.transform.position);
            AddScoreAndRank();
            revertSpriteCor = StartCoroutine(RevertSpriteAfterTime());
        }
        else if (canTrick && _playerMovement.IsGrounded() != true && playerSprite != null)
        {
            //Debug.Log("trick stop cor");
            //playerSprite.color = trickColor;
            
            if (_trickObject.TryGetComponent<Ramp>(out var ramp))
            {
                AudioManager.instance.PlayOneShot(FMODEvents.instance.PlayerTrick, this.transform.position);
                ramp.hasTricked = true;
                AddScoreAndRank();
            }
            else if (_trickObject.TryGetComponent<JumpPad>(out var jumpPad))
            {
                AudioManager.instance.PlayOneShot(FMODEvents.instance.PlayerTrick, this.transform.position);
                jumpPad.hasTricked = true;
                AddScoreAndRank();
            }
            else if (_trickObject.TryGetComponent<JumpPadVariation>(out var jumpPadVar))
            {
                AudioManager.instance.PlayOneShot(FMODEvents.instance.PlayerTrick, this.transform.position);
                jumpPadVar.hasTricked = true;
                AddScoreAndRank();
            }
            else if (_trickObject.TryGetComponent<RailsParent>(out var railing))
            {
                AudioManager.instance.PlayOneShot(FMODEvents.instance.PlayerTrick, this.transform.position);
                railing.hasTricked = true;
                AddScoreAndRank();
            }
            else if (_trickObject.TryGetComponent<Wall>(out var wall))
            {
                AudioManager.instance.PlayOneShot(FMODEvents.instance.PlayerTrick, this.transform.position);
                wall.hasTricked = true;
                AddScoreAndRank();
            }

            revertSpriteCor = StartCoroutine(RevertSpriteAfterTime());
        }
    }

    private IEnumerator RevertSpriteAfterTime()
    {
        /*
        yield return new WaitForSeconds(trickTime);
        if (playerSprite != null)
        {
            playerSprite.color = visible;
            trickSprite.GetComponent<SpriteRenderer>().color = invisible;
        }
        */
        do
        {
            randomSpriteIndex = Random.Range(0, trickSprites.Count);
        } while (randomSpriteIndex == lastTrickSpriteIndex);
        
        trickSprite.GetComponent<SpriteRenderer>().sprite = trickSprites[randomSpriteIndex];
        trickSprite.GetComponent<SpriteRenderer>().color = visible;
        playerSprite.color = invisible;
        DisableCanTrick();
        canTrick = false;
        lastTrickSpriteIndex = randomSpriteIndex;
        //Debug.Log("randomSpriteIndex = " + randomSpriteIndex);

        float trickTimeLeft = trickTime;
        while (trickTimeLeft > 0)
        {
            trickTimeLeft -= Time.deltaTime;
            //Debug.Log("trickTimeLeft = " + trickTimeLeft);
            if (_playerMovement.IsGrounded() || IsWallRiding || _playerRailing.IsMovingOnRail) 
            {
                playerSprite.color = visible;
                trickSprite.GetComponent<SpriteRenderer>().color = invisible;
                break; 
            }
            yield return null;
        }
        playerSprite.color = visible;
        trickSprite.GetComponent<SpriteRenderer>().color = invisible;
        //Debug.Log("Trick Ended");
    }


    public void EnableTrickDestroyed()
    {
        _trickObject = null;
        _destroyedObject = true;
        if (enableTrickCor != null)
        {
            StopCoroutine(enableTrickCor);
            enableTrickCor = null;
            //Debug.Log("destroyed stop cor");
        }
        enableTrickCor = StartCoroutine(EnableTrickCoroutine());
    }

    public void EnableTrick(GameObject gameObject)
    {
        if (gameObject.TryGetComponent<Ramp>(out _) || gameObject.TryGetComponent<Wall>(out _) || gameObject.TryGetComponent<RailsParent>(out _))
        {
            _trickObject = gameObject;
            if (enableTrickCor != null)
            {
                StopCoroutine(enableTrickCor);
                enableTrickCor = null;
                //Debug.Log(_trickObject.gameObject.name + " stopped trick cor");
            }
            enableTrickCor = StartCoroutine(EnableTrickCoroutine());
        }
        else if (!canTrick)
        {
            _trickObject = gameObject;
            //StopCoroutine(EnableTrickCoroutine());
            enableTrickCor = StartCoroutine(EnableTrickCoroutine());
        }
    }

    private IEnumerator EnableTrickCoroutine()
    {
        
        canTrick = true;
        //playerSprite.color = enableTrickColor;
        //yield return new WaitForSeconds(enableTrickDuration);
        float enableTrickTimer = enableTrickDuration;
        while (enableTrickTimer > 0f)
        {
            enableTrickTimer -= Time.deltaTime;
            yield return null;
        }
        //Debug.Log("enableTrickDuration ended");
        _trickObject = null;
        canTrick = false;
    }

    public void DisableCanTrick()
    {
        canTrick = false;
        if (enableTrickCor != null)
        {
            StopCoroutine(enableTrickCor);
            enableTrickCor = null;
            //Debug.Log("disabled canTrick");
        }

    }
    #endregion

    #region SpringBoard
    public void CallJumpPadTimer()
    {
        StopCoroutine(JumpPadTimer());
        StartCoroutine(JumpPadTimer());
    }
    public IEnumerator JumpPadTimer()
    {
        /*
        float time = Time.time;
        while (Time.time < time + jumpPadTimer)
        {
            
            yield return null;
        }
        */
        yield return new WaitForSeconds(jumpPadTimer);
        isOnJumpPad = false;
    }
    #endregion
}
