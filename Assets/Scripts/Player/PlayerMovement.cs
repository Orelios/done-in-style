using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerConfigsSO playerConfig;
    [SerializeField] private Collider2D bodyCollider;
    [SerializeField] private Collider2D feetCollider;
    private Rigidbody2D _rb;
    private PlayerInputManager _playerInputManager;
    
    private Vector2 _movementVelocity;
    private bool _isFacingRight;
    
    private RaycastHit2D _groundHit;
    private RaycastHit2D _headHit;
    private bool _isGrounded;
    private bool _isHeadBumped;
    
    public float VerticalVelocity { get; private set; }
    private bool _isJumping;
    private bool _isFalling;
    private bool _isFastFalling;
    private float _fastFallTime;
    private float _fastFallReleaseSpeed;

    private float _apexPoint;
    private float _timePastApexThreshold;
    private bool _isPastApexThreshold;

    private float _jumpBufferTimer;
    private bool _jumpReleasedDuringBuffer;

    private float _coyoteTimer;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerInputManager = GetComponent<PlayerInputManager>();
        _isFacingRight = true;
    }

    private void Update()
    {
        CountTimers();
        JumpChecks();
    }

    private void FixedUpdate()
    {
        CollisionChecks();
        Jump();
        
        Move(_isGrounded ? playerConfig.GroundAcceleration : playerConfig.AirAcceleration, 
            _isGrounded ? playerConfig.GroundDeceleration : playerConfig.AirDeceleration,
            _playerInputManager.HorizontalMovement);
    }

    #region Movement

    private void Move(float acceleration, float deceleration, float moveInput)
    {
        if (moveInput != 0)
        {
            TurnCheck(moveInput);

            Vector2 targetVelocity = new Vector2(moveInput, 0f) * playerConfig.BaseMovementSpeed;
            _movementVelocity = Vector2.Lerp(_movementVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
            _rb.linearVelocity = new Vector2(_movementVelocity.x, _rb.linearVelocity.y);
        }
        else if (moveInput == 0)
        {
            _movementVelocity = Vector2.Lerp(_movementVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
            _rb.linearVelocity = new Vector2(_movementVelocity.x, _rb.linearVelocity.y);
        }
    }

    private void TurnCheck(float moveInput)
    {
        if (_isFacingRight && moveInput < 0f)
        {
            Turn(false);
        }
        else if (!_isFacingRight && moveInput> 0f)
        {
            Turn(true);
        }
    }

    private void Turn(bool shouldTurnRight)
    {
        if (shouldTurnRight)
        {
            _isFacingRight = true;
            transform.Rotate(0f, 180f, 0f);
        }
        else
        {
            _isFacingRight = false;
            transform.Rotate(0f, -180f, 0f);
        }
    }

    #endregion
    
    #region Jump

    private void JumpChecks()
    {
        if (_playerInputManager.IsJumping)
        {
            _jumpBufferTimer = playerConfig.JumpBufferingTime;
            _jumpReleasedDuringBuffer = false;
        }

        if (!_playerInputManager.IsJumping)
        {
            if (_jumpBufferTimer > 0f)
            {
                _jumpReleasedDuringBuffer = true;
            }

            if (_playerInputManager.IsJumping && VerticalVelocity > 0f)
            {
                if (_isPastApexThreshold)
                {
                    _isPastApexThreshold = false;
                    _isFastFalling = true;
                    _fastFallTime = playerConfig.TimeForUpwardsCancel;
                    VerticalVelocity = 0f;
                }
                else
                {
                    _isFastFalling = true;
                    _fastFallReleaseSpeed = VerticalVelocity;
                }
            }
        }

        if (_jumpBufferTimer > 0f && !_isJumping && (_isGrounded || _coyoteTimer > 0f))
        {
            if (!_isJumping)
            {
                _isJumping = true;
            }

            _jumpBufferTimer = 0f;
            VerticalVelocity = playerConfig.InitialJumpVelocity;

            if (_jumpReleasedDuringBuffer)
            {
                _isFastFalling = true;
                _fastFallReleaseSpeed = VerticalVelocity;
            }
        }

        if ((_isJumping || _isFalling) && _isGrounded && VerticalVelocity <= 0f)
        {
            _isJumping = false;
            _isFalling = false;
            _isFastFalling = false;
            _fastFallTime = 0f;
            _isPastApexThreshold = false;
            VerticalVelocity = Physics2D.gravity.y;
        }
    }

    private void Jump()
    {
        //apply gravity while jumping
        if (_isJumping)
        {
            //check if player's head bumped on a ceiling; if so, apply fast falling
            if (_isHeadBumped)
            {
                _isFastFalling = true;
            }

            //gravity application when ascending/jumping
            if (VerticalVelocity >= 0f)
            {
                //check if past the apex threshold
                _apexPoint = Mathf.InverseLerp(playerConfig.InitialJumpVelocity, 0f, VerticalVelocity);

                if (_apexPoint > playerConfig.ApexThreshold)
                {
                    if (!_isPastApexThreshold)
                    {
                        _isPastApexThreshold = true;
                        _timePastApexThreshold = 0f;
                    }

                    if (_isPastApexThreshold)
                    {
                        _timePastApexThreshold += Time.fixedDeltaTime;

                        if (_timePastApexThreshold < playerConfig.ApexHangTime)
                        {
                            VerticalVelocity = 0f;
                        }
                        else
                        {
                            VerticalVelocity = -0.01f;
                        }
                    }
                }
                //gravity application when ascending/jumping but NOT reaching apex threshold
                else
                {
                    VerticalVelocity += playerConfig.Gravity * Time.fixedDeltaTime;

                    if (_isPastApexThreshold)
                    {
                        _isPastApexThreshold = false;
                    }
                }
            }
            //gravity application when descending/fallling
            else if (!_isFastFalling)
            {
                VerticalVelocity += playerConfig.Gravity * playerConfig.GravityOnReleaseMultiplier *Time.fixedDeltaTime;
            }
            else if (VerticalVelocity < 0f)
            {
                if (!_isFalling)
                {
                    _isFalling = true;
                }
            }
        }
        
        //jump cut
        if (_isFastFalling)
        {
            if (_fastFallTime >= playerConfig.TimeForUpwardsCancel)
            {
                VerticalVelocity += playerConfig.Gravity * playerConfig.GravityOnReleaseMultiplier *Time.fixedDeltaTime;
            } 
            else if (_fastFallTime < playerConfig.TimeForUpwardsCancel)
            {
                VerticalVelocity = Mathf.Lerp(_fastFallReleaseSpeed, 0f, (_fastFallTime / playerConfig.TimeForUpwardsCancel));
            }
            
            _fastFallTime += Time.fixedDeltaTime;
        }
        
        //normal gravity while falling
        if (!_isGrounded && !_isJumping)
        {
            if (!_isFalling)
            {
                _isFalling = true;
            }
            
            VerticalVelocity += playerConfig.Gravity  *Time.fixedDeltaTime;
        }
        
        //clamp fall speed
        VerticalVelocity = Mathf.Clamp(VerticalVelocity, -playerConfig.MaxFallSpeed, 50f);
        _rb.linearVelocity = new(_rb.linearVelocityX, VerticalVelocity);
    }
    
    #endregion
    
    #region Collision Checks

    private void IsGrounded()
    {
        Vector2 boxcastOrigin = new(feetCollider.bounds.center.x, feetCollider.bounds.min.y);
        Vector2 boxcastSize = new(feetCollider.bounds.size.x, playerConfig.GroundCheckDistance);
        
        _groundHit = Physics2D.BoxCast(boxcastOrigin, boxcastSize, 0f, Vector2.down, playerConfig.GroundCheckDistance, playerConfig.GroundLayer);
        _isGrounded = _groundHit.collider;
    }

    private void IsHeadBumped()
    {
        Vector2 boxcastOrigin = new(feetCollider.bounds.center.x, feetCollider.bounds.max.y);
        Vector2 boxcastSize = new(feetCollider.bounds.size.x * playerConfig.HeadWidth, playerConfig.HeadCheckDistance);
        
        _headHit = Physics2D.BoxCast(boxcastOrigin, boxcastSize, 0f, Vector2.up, playerConfig.HeadCheckDistance, playerConfig.GroundLayer);
        _isHeadBumped = _headHit.collider;
    }

    private void CollisionChecks()
    {
        IsGrounded();
        IsHeadBumped();
    }
    
    #endregion

    #region Timers

    private void CountTimers()
    {
        _jumpBufferTimer -= Time.deltaTime;
        
        _coyoteTimer = _isGrounded ? _coyoteTimer -= Time.deltaTime : playerConfig.CoyoteTime;
    }

    #endregion
}
