using System;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    //TODO: set up all the common variables/components/functions needed for all Player scripts here
    private SpriteRenderer _playerSprite;
    public SpriteRenderer Sprite => _playerSprite;
    private Rigidbody2D _playerRb;
    public Rigidbody2D Rigidbody => _playerRb;
    private Collider2D _playerCollider;
    public Collider2D Collider => _playerCollider;
    private Animator _playerAnimator;
    public Animator Animator => _playerAnimator;
    
    private PlayerInputManager _playerInputManager;
    public PlayerInputManager InputManager => _playerInputManager;
    private PlayerMovement _playerMovement;
    public PlayerMovement Movement => _playerMovement;
    private PlayerTricks _playerTricks;
    public PlayerTricks Tricks => _playerTricks;
    /*private PlayerRailGrind _playerRailGrind;
    public PlayerRailGrind RailGrind => _playerRailGrind;*/
    private PlayerRailing _playerRailing;
    public PlayerRailing Railing => _playerRailing;
    private PlayerHealth _playerHealth;
    public PlayerHealth Health => _playerHealth;
    private PlayerInvulnerability _playerInvulnerability;
    public PlayerInvulnerability Invulnerability => _playerInvulnerability;

    private StateMachine _playerVelocitySM;
    private StateMachine _playerActionSM;
    public StateMachine ActionSM => _playerActionSM;
    
    public Type CurrentSuperState => CurrentSubState.BaseType;
    public Type CurrentSubState => _playerActionSM.GetCurrentState().GetType();

    [Header("Animation Names")] 
    [SerializeField] private string idling;
    public string IdlingAnimationName => idling;
    [SerializeField] private string skating;
    public string SkatingAnimationName => skating;
    [SerializeField] private string jumping;
    public string JumpingAnimationName => jumping;
    [SerializeField] private string blendTreeYVelocity;
    public string BlendTreeYVelocityName => blendTreeYVelocity;
    [SerializeField] private string dashing;
    public string DashingAnimationName => dashing;
    [SerializeField] private string falling;
    public string FallingAnimationName => falling;
    [SerializeField] private string sliding;
    public string SlidingAnimationName => sliding;
    [SerializeField] private string railGrinding;
    public string RailGrindingAnimationName => railGrinding;
    [SerializeField] private string wallRiding;
    public string WallRidingAnimationName => wallRiding;
    [SerializeField] private string hurt;
    public string HurtAnimationName => hurt;
    
    private void OnEnable()
    {
        _playerSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        _playerRb = GetComponent<Rigidbody2D>();
        _playerCollider = GetComponent<Collider2D>();
        _playerAnimator = transform.GetChild(0).GetComponent<Animator>();
        _playerHealth = GetComponent<PlayerHealth>();
        _playerInvulnerability = GetComponent<PlayerInvulnerability>();
        
        _playerInputManager = GetComponent<PlayerInputManager>();        
        _playerMovement = GetComponent<PlayerMovement>();
        _playerTricks = GetComponent<PlayerTricks>();
        //_playerRailGrind = GetComponent<PlayerRailGrind>();
        _playerRailing = GetComponent<PlayerRailing>();
        
        InitializeActionStateMachine();
    }

    private void Update()
    {
        _playerActionSM?.Update();
    }

    private void FixedUpdate()
    {
        _playerActionSM?.FixedUpdate();
    }

    private void InitializeActionStateMachine()
    {
        _playerActionSM = new();
        
        var idlingState = new IdlingState(this);
        var skatingState = new SkatingState(this);        
        
        var risingState = new RisingState(this);
        var fallingState = new FallingState(this);
        
        var dashingState = new DashingState(this);
        var poundingState = new PoundingState(this);
        var slidingState = new SlidingState(this);
        var grindingState = new GrindingState(this);
        var wallRidingState = new WallRidingState(this);
        var hurtState = new HurtState(this);
        
        #region Idling State
        NormalTransition(_playerActionSM, idlingState, skatingState, new FuncPredicate(() => Mathf.Abs(_playerRb.linearVelocityX) > 0.1f));
        NormalTransition(_playerActionSM, idlingState, risingState, new FuncPredicate(() => _playerRb.linearVelocityY > 0.1f && !_playerMovement.IsGrounded()));
        NormalTransition(_playerActionSM, idlingState, fallingState, new FuncPredicate(() => _playerRb.linearVelocityY < 0.1f && !_playerMovement.IsGrounded()));
        NormalTransition(_playerActionSM, idlingState, dashingState, new FuncPredicate(() => _playerTricks.IsDashing));
        NormalTransition(_playerActionSM, idlingState, slidingState, new FuncPredicate(() => _playerTricks.IsSliding));
        #endregion
        
        #region Skating State
        NormalTransition(_playerActionSM, skatingState, idlingState, new FuncPredicate(() => Mathf.Abs(_playerRb.linearVelocityX) < 0.1f));
        NormalTransition(_playerActionSM, skatingState, risingState, new FuncPredicate(() => _playerRb.linearVelocityY > 0.1f && !_playerMovement.IsGrounded()));
        NormalTransition(_playerActionSM, skatingState, fallingState, new FuncPredicate(() => _playerRb.linearVelocityY < 0.1f && !_playerMovement.IsGrounded()));
        NormalTransition(_playerActionSM, skatingState, dashingState, new FuncPredicate(() => _playerTricks.IsDashing));
        NormalTransition(_playerActionSM, skatingState, slidingState, new FuncPredicate(() => _playerTricks.IsSliding));
        #endregion
        
        #region Rising State
        NormalTransition(_playerActionSM, risingState, fallingState, new FuncPredicate(() => _playerRb.linearVelocityY < 0.1f));
        NormalTransition(_playerActionSM, risingState, dashingState, new FuncPredicate(() => _playerTricks.IsDashing));
        NormalTransition(_playerActionSM, risingState, poundingState, new FuncPredicate(() => _playerTricks.IsPounding));
        NormalTransition(_playerActionSM, risingState, wallRidingState, new FuncPredicate(() => _playerTricks.IsWallRiding));
        #endregion
        
        #region Falling State
        NormalTransition(_playerActionSM, fallingState, risingState, new FuncPredicate(() => _playerRb.linearVelocityY > 0.1f));
        NormalTransition(_playerActionSM, fallingState, idlingState, new FuncPredicate(() => Mathf.Abs(_playerRb.linearVelocityX) < 0.1f && _playerMovement.IsGrounded()));
        NormalTransition(_playerActionSM, fallingState, skatingState, new FuncPredicate(() => Mathf.Abs(_playerRb.linearVelocityX) > 0.1f && _playerMovement.IsGrounded() && !_playerMovement.PlayerOnRailing()));
        NormalTransition(_playerActionSM, fallingState, dashingState, new FuncPredicate(() => _playerTricks.IsDashing));
        NormalTransition(_playerActionSM, fallingState, poundingState, new FuncPredicate(() => _playerTricks.IsPounding));
        NormalTransition(_playerActionSM, fallingState, grindingState, new FuncPredicate(() =>  _playerMovement.PlayerOnRailing()));
        NormalTransition(_playerActionSM, fallingState, wallRidingState, new FuncPredicate(() => _playerTricks.IsWallRiding));
        #endregion

        #region Dashing State
        NormalTransition(_playerActionSM, dashingState, skatingState, new FuncPredicate(() => !_playerTricks.IsDashing && _playerMovement.IsGrounded() && Mathf.Abs(_playerRb.linearVelocityX) > 0.1f && _playerRailing.IsMovingOnRail));
        NormalTransition(_playerActionSM, dashingState, idlingState, new FuncPredicate(() => !_playerTricks.IsDashing && _playerMovement.IsGrounded() && Mathf.Abs(_playerRb.linearVelocityX) < 0.1f));
        NormalTransition(_playerActionSM, dashingState, risingState, new FuncPredicate(() => !_playerTricks.IsDashing &&  _playerRb.linearVelocityY > 0.1f));
        NormalTransition(_playerActionSM, dashingState, fallingState, new FuncPredicate(() => !_playerTricks.IsDashing && _playerRb.linearVelocityY < 0.1f));
        NormalTransition(_playerActionSM, dashingState, grindingState, new FuncPredicate(() => !_playerTricks.IsDashing && _playerRailing.IsMovingOnRail));
        NormalTransition(_playerActionSM, dashingState, poundingState, new FuncPredicate(() => !_playerTricks.IsDashing && !_playerMovement.IsGrounded() && !_playerTricks.IsWallRiding));
        NormalTransition(_playerActionSM, dashingState, wallRidingState, new FuncPredicate(() => !_playerTricks.IsDashing && !_playerMovement.IsGrounded() && _playerTricks.IsWallRiding));
        #endregion

        #region Pounding State
        NormalTransition(_playerActionSM, poundingState, idlingState, new FuncPredicate(() => !_playerTricks.IsPounding && (_playerMovement.IsGrounded() || _playerMovement.IsSpringBoarding()) && Mathf.Abs(_playerRb.linearVelocityX) < 0.1f));
        NormalTransition(_playerActionSM, poundingState, skatingState, new FuncPredicate(() => !_playerTricks.IsPounding && (_playerMovement.IsGrounded() || _playerMovement.IsSpringBoarding()) && Mathf.Abs(_playerRb.linearVelocityX) > 0.1f));
        NormalTransition(_playerActionSM, poundingState, grindingState, new FuncPredicate(() => !_playerTricks.IsPounding && _playerRailing.IsMovingOnRail));
        #endregion

        #region Sliding State
        NormalTransition(_playerActionSM, slidingState, idlingState, new FuncPredicate(() => !_playerTricks.IsSliding && Mathf.Abs(_playerMovement.Rb.linearVelocityX) < 0.1f));
        NormalTransition(_playerActionSM, slidingState, skatingState, new FuncPredicate(() => !_playerTricks.IsSliding && Mathf.Abs(_playerMovement.Rb.linearVelocityX) >0.1f));
        NormalTransition(_playerActionSM, slidingState, risingState, new FuncPredicate(() => !_playerMovement.IsGrounded() && _playerMovement.Rb.linearVelocityY > 0.1f));
        NormalTransition(_playerActionSM, slidingState, fallingState, new FuncPredicate(() => !_playerMovement.IsGrounded() && _playerMovement.Rb.linearVelocityY < 0.1f));
        #endregion

        #region Grinding State
        NormalTransition(_playerActionSM, grindingState, risingState, new FuncPredicate(() => !_playerRailing.IsMovingOnRail && _playerRb.linearVelocityY > 0.1f));
        NormalTransition(_playerActionSM, grindingState, fallingState, new FuncPredicate(() => !_playerRailing.IsMovingOnRail && _playerRb.linearVelocityY < 0.1f));
        #endregion
        
        #region Wall Riding State
        NormalTransition(_playerActionSM, wallRidingState, fallingState, new FuncPredicate(() => (!_playerTricks.IsWallRiding) && _playerRb.linearVelocityY < 0.1f));
        NormalTransition(_playerActionSM, wallRidingState, risingState, new FuncPredicate(() => (!_playerTricks.IsWallRiding) && _playerRb.linearVelocityY > 0.1f));
        #endregion

        #region Hurt State
        AnyTransition(_playerActionSM, hurtState, new FuncPredicate(() => _playerInvulnerability.IsHit));
        NormalTransition(_playerActionSM, hurtState, idlingState, new FuncPredicate(() => !_playerInvulnerability.IsHit && Mathf.Abs(_playerRb.linearVelocityX) < 0.1f));
        NormalTransition(_playerActionSM, hurtState, skatingState, new FuncPredicate(() => !_playerInvulnerability.IsHit && Mathf.Abs(_playerRb.linearVelocityX) > 0.1f));
        NormalTransition(_playerActionSM, hurtState, risingState, new FuncPredicate(() => !_playerInvulnerability.IsHit && _playerRb.linearVelocityY > 0.1f));
        NormalTransition(_playerActionSM, hurtState, risingState, new FuncPredicate(() => !_playerInvulnerability.IsHit && _playerRb.linearVelocityY < 0.1f));
        #endregion
        
        _playerActionSM.SetState(idlingState);
    }
    
    private void NormalTransition(StateMachine stateMachine, IState currentState, IState nextState, IPredicate condition) => stateMachine.AddNormalTransition(currentState, nextState, condition);
    private void AnyTransition(StateMachine stateMachine, IState nextState, IPredicate condition) => stateMachine.AddAnyTransition(nextState, condition);
}
