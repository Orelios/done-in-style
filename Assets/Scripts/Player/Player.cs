using System;
using Unity.VisualScripting;
using UnityEngine;

[DefaultExecutionOrder(-1)]
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
    private PlayerRailGrind _playerRailGrind;
    public PlayerRailGrind RailGrind => _playerRailGrind;

    private StateMachine _playerVelocitySM;
    private StateMachine _playerActionSM;
    public StateMachine ActionSM => _playerActionSM;
    
    public Type CurrentSuperState => CurrentSubState.BaseType;
    public Type CurrentSubState => _playerActionSM.GetCurrentState().GetType();

    private void Awake()
    {
        _playerSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        _playerRb = GetComponent<Rigidbody2D>();
        _playerCollider = GetComponent<Collider2D>();
        _playerAnimator = transform.GetChild(0).GetComponent<Animator>();
        
        _playerInputManager = GetComponent<PlayerInputManager>();        
        _playerMovement = GetComponent<PlayerMovement>();
        _playerTricks = GetComponent<PlayerTricks>();
        _playerRailGrind = GetComponent<PlayerRailGrind>();
        
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
        
        NormalTransition(_playerActionSM, idlingState, skatingState, new FuncPredicate(() => Mathf.Abs(_playerRb.linearVelocityX) > 0.1f));
        NormalTransition(_playerActionSM, idlingState, risingState, new FuncPredicate(() => _playerRb.linearVelocityY > 0f && !_playerMovement.IsGrounded()));
        NormalTransition(_playerActionSM, idlingState, fallingState, new FuncPredicate(() => _playerRb.linearVelocityY < 0f && !_playerMovement.IsGrounded()));
        NormalTransition(_playerActionSM, idlingState, dashingState, new FuncPredicate(() => _playerTricks.IsDashing));
        
        NormalTransition(_playerActionSM, skatingState, idlingState, new FuncPredicate(() => Mathf.Abs(_playerRb.linearVelocityX) < 0.1f));
        NormalTransition(_playerActionSM, skatingState, risingState, new FuncPredicate(() => _playerRb.linearVelocityY > 0f && !_playerMovement.IsGrounded()));
        NormalTransition(_playerActionSM, skatingState, fallingState, new FuncPredicate(() => _playerRb.linearVelocityY < 0f && !_playerMovement.IsGrounded()));
        NormalTransition(_playerActionSM, skatingState, dashingState, new FuncPredicate(() => _playerTricks.IsDashing));
        
        NormalTransition(_playerActionSM, risingState, fallingState, new FuncPredicate(() => _playerRb.linearVelocityY < 0f));
        NormalTransition(_playerActionSM, risingState, dashingState, new FuncPredicate(() => _playerTricks.IsDashing));
        
        NormalTransition(_playerActionSM, fallingState, risingState, new FuncPredicate(() => _playerRb.linearVelocityY > 0f));
        NormalTransition(_playerActionSM, fallingState, idlingState, new FuncPredicate(() => Mathf.Abs(_playerRb.linearVelocityX) < 0.1f && _playerMovement.IsGrounded()));
        NormalTransition(_playerActionSM, fallingState, skatingState, new FuncPredicate(() => Mathf.Abs(_playerRb.linearVelocityX) > 0.1f && _playerMovement.IsGrounded()));
        NormalTransition(_playerActionSM, fallingState, dashingState, new FuncPredicate(() => _playerTricks.IsDashing));
        
        NormalTransition(_playerActionSM, dashingState, skatingState, new FuncPredicate(() => !_playerTricks.IsDashing && _playerMovement.IsGrounded() && Mathf.Abs(_playerRb.linearVelocityX) > 0.1f));
        NormalTransition(_playerActionSM, dashingState, idlingState, new FuncPredicate(() => !_playerTricks.IsDashing && _playerMovement.IsGrounded() && Mathf.Abs(_playerRb.linearVelocityX) < 0.1f));
        NormalTransition(_playerActionSM, dashingState, fallingState, new FuncPredicate(() => !_playerTricks.IsDashing && _playerRb.linearVelocityY < 0f));
        
        _playerActionSM.SetState(idlingState);
    }
    
    private void NormalTransition(StateMachine stateMachine, IState currentState, IState nextState, IPredicate condition) => stateMachine.AddNormalTransition(currentState, nextState, condition);
    private void AnyTransition(StateMachine stateMachine, IState nextState, IPredicate condition) => stateMachine.AddAnyTransition(nextState, condition);
}
