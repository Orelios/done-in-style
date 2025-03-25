using UnityEngine;

public class PlayerRailing : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 5f; // Maximum movement speed
    [SerializeField] private float acceleration = 2f; // Acceleration rate
    [SerializeField] private float deceleration = 2f; // Deceleration rate when stopping
    [SerializeField] private float speedRequirement = 5;

    private float _currentSpeed = 0f;
    private bool _IsmovingOnRail = true; // Whether the player is moving
    private PlayerMovement _playerMovement;
    private RankCalculator _rankCalculator;
    private float _speedOnEnter;
    private float rankSpeedMult = 1f;

    public bool IsMovingOnRail { get => _IsmovingOnRail; set => _IsmovingOnRail = value; }
    public float SpeedRequirement { get => speedRequirement; set => speedRequirement = value; }

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _rankCalculator = GameObject.Find("Systems/Score System").GetComponent<RankCalculator>();
        _IsmovingOnRail = false; 
    }
    public void MoveForward()
    {
        //Debug.Log(_playerMovement.IsGrounded());

        if (_IsmovingOnRail)
        {
            // Accelerate towards max speed
            _speedOnEnter = Mathf.MoveTowards(_speedOnEnter, maxSpeed, acceleration);

        }

        rankSpeedMult = _rankCalculator.speedMults[_rankCalculator.CurrentStylishRankIndex];

        // Apply velocity in the direction the player is facing
        _playerMovement.Rb.linearVelocity = transform.right * _speedOnEnter * rankSpeedMult;
        /*
        else
        {
            // Decelerate to a stop
            _currentSpeed = Mathf.MoveTowards( _currentSpeed, 0, deceleration * Time.fixedDeltaTime);
        }
        */

    }

    public void ApplyOnenterSpeed(float speedOnEnter)
    {
        _speedOnEnter = speedOnEnter; 
    }
}
