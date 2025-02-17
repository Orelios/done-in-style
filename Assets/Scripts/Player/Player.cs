using System;
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

    private StateMachine _playerVelocitySM;
    private StateMachine _playerActionSM;
    
    private void Awake()
    {
        _playerSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        _playerRb = GetComponent<Rigidbody2D>();
        _playerCollider = GetComponent<Collider2D>();
        _playerAnimator = transform.GetChild(0).GetComponent<Animator>();
        
        _playerInputManager = GetComponent<PlayerInputManager>();        
        _playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        _playerVelocitySM?.Update();
    }

    private void FixedUpdate()
    {
        _playerActionSM?.FixedUpdate();
    }

    private void InitializeActionStateMachine()
    {
        _playerActionSM = new();

        var horizontalMovementState = new HorizontalMovementState(this);
        var jumpState = new JumpState(this);
        
        AnyTransition(_playerActionSM, jumpState, new FuncPredicate(() => _playerMovement.IsGrounded()));
    }
    
    private void NormalTransition(StateMachine stateMachine, IState currentState, IState nextSate, IPredicate condition) => stateMachine.AddNormalTransition(currentState, nextSate, condition);
    private void AnyTransition(StateMachine stateMachine, IState nextState, IPredicate condition) => stateMachine.AddAnyTransition(nextState, condition);
}
