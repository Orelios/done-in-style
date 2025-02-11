using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class New_PlayerMovement : MonoBehaviour
{
    private PlayerInputManager _playerInputManager;

    [Header("Components")]
    private Rigidbody2D _rb;
    [Tooltip("Attach here the Player's GroundCheck")]
    public Transform GroundCheck;
    [Tooltip("Select here the Ground LayerMask for ground detection")]
    public LayerMask GroundLayer;
    [Tooltip("Attach here the Camera Handler")]
    [SerializeField] private CameraHandler cameraHandler;
    [Tooltip("Attach here the PlayerConfigSO")]
    [SerializeField] private PlayerConfigsSO playerConfigsSO;

    [Header("Direction of Player Facing")]
    public bool IsFacingRight = true;
    private Vector2 _velocity;

    private float _lastGroundedTime = 0f;

    [Header("Debug Configs")]
    [SerializeField] private bool showGizmos = false;

    private PlayerGearSwapper _playerGearSwapper;
    private PlayerTricks _playerTricks;

    private void Awake()
    {
        _playerInputManager = GetComponent<PlayerInputManager>();
        _playerGearSwapper = GetComponent<PlayerGearSwapper>();
        _playerTricks = GetComponent<PlayerTricks>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (_rb.linearVelocityY < cameraHandler.YVelocityThreshold && !cameraHandler.IsPanningCoroutineActive)
        {
            cameraHandler.LerpCameraPanning(true);
        }

        if (_rb.linearVelocityY >= 0 && !cameraHandler.IsPanningCoroutineActive)
        {
            cameraHandler.LerpCameraPanning(false);
        }
    }

    private void FixedUpdate()
    {
        //Disables movement while dashing
        if (_playerTricks.IsDashing && _playerGearSwapper.CurrentGearEquipped.DaredevilGearType
            == EDaredevilGearType.Skateboard) { return; }

        Move(); 

        switch (IsFacingRight)
        {
            //Flips player sprite to the direction they are heading to 
            case false when _playerInputManager.HorizontalMovement > 0f:
            case true when _playerInputManager.HorizontalMovement < 0f:
                Flip();
                break;
        }

        //Handles the timer for coyote time
        _lastGroundedTime = IsGrounded() ? 0f : _lastGroundedTime += Time.deltaTime;

        Jump();
        Gravity();
    }

    #region HorizontalMovement
    private void Move()
    {
        // Calculate target speed based on input
        float targetSpeed = _playerInputManager.HorizontalMovement * playerConfigsSO.BaseSpeed *
            _playerGearSwapper.HorizontalMovementMultiplier;

        float speedDif = targetSpeed - _rb.linearVelocity.x;

        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? playerConfigsSO.Acceleration : playerConfigsSO.Deceleration;

        float appliedMovementSpeed = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, playerConfigsSO.VelPower) * Mathf.Sign(speedDif);

        _rb.AddForce(appliedMovementSpeed * Vector2.right);

        //Friction
        if (_lastGroundedTime > 0 && Mathf.Abs(_playerInputManager.HorizontalMovement) < 0.01f)
        {
            float amount = Mathf.Min(Mathf.Abs(_rb.linearVelocity.x), Mathf.Abs(playerConfigsSO.FrictionAmount));

            amount *= Mathf.Sign(_rb.linearVelocity.x);

            _rb.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
        }
    }
    #endregion

    #region Jumping
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(GroundCheck.position, 0.2f, GroundLayer);
    }

    public void Jump()
    {
        //OPTIMIZE: validates if player can jump;
        bool canJump = IsGrounded() || (!IsGrounded() && _lastGroundedTime < playerConfigsSO.CoyoteTime);

        if (_playerInputManager.IsJumping && canJump)
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, playerConfigsSO.JumpPower * 
                _playerGearSwapper.JumpForceMultiplier);
        }
        else if (!_playerInputManager.IsJumping)
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _rb.linearVelocity.y);
        }
    }
    #endregion

    #region Gravity
    private void Gravity()
    {
        if (_rb.linearVelocity.y < 0)
        {
            _rb.gravityScale = playerConfigsSO.BaseGravity * playerConfigsSO.FallSpeedMultiplier;
            _rb.linearVelocity = new Vector2(_rb.linearVelocityX, Mathf.Max(_rb.linearVelocity.y, -(playerConfigsSO.MaxFallSpeed -
                _playerTricks.FallingSpeedModifier)));
        }
        else
        {
            _rb.gravityScale = playerConfigsSO.BaseGravity;
        }
    }
    #endregion
    private void Flip() //flips character where player is facing towards
    {
        IsFacingRight = !IsFacingRight;
        Vector3 flipRotation = new(transform.rotation.x, IsFacingRight == true ? 0 : 180f, transform.rotation.z);
        transform.rotation = Quaternion.Euler(flipRotation);
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos)
        {
            return;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(GroundCheck.position, 0.2f);
    }
}
