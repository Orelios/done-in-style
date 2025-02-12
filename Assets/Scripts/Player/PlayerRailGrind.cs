using UnityEngine;

public class PlayerRailGrind : MonoBehaviour
{
    [Header("Grinding Configs")]
    [Tooltip("Input here the burst of speed the player will get when grinding")]
    [SerializeField] private float grindingSpeedMultiplier;
    [Tooltip("Input here the minimum speed the player needs to start grinding")]
    [SerializeField] private float minimumSpeedToGrind;
    public float MinimumSpeedToGrind => minimumSpeedToGrind;
    [Tooltip("Adjust here the decay rate of the player's speed when stopping grinding; a lower value will result in a faster decay rate")]
    [SerializeField, Range(1, 100)] private int speedDecayRate;
    [SerializeField] private float heightOffset;
    [HideInInspector]public bool IsOnRail;
    private float _grindingSpeed;
    
    private Rigidbody2D _rb;
    private Railing _railing;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (IsOnRail && _railing is not null)
        {
            GrindRail();
        }
    }

    private void GrindRail()
    {
        _rb.linearVelocity = new Vector2(_grindingSpeed, _rb.linearVelocity.y);
    }

    public void EnableRailing(Railing railing)
    {
        IsOnRail = true;
        _railing = railing;
        _grindingSpeed = _rb.linearVelocityX * grindingSpeedMultiplier;
    }

    public void DisableRailing()
    {
        _grindingSpeed = 0f;
        IsOnRail = false;
        _railing = null;
        _rb.linearVelocity = new(_rb.linearVelocityX * (speedDecayRate / 100f), _rb.linearVelocityY);
    }
}
