using UnityEngine;

public class PlayerRailGrind : MonoBehaviour
{
    [Header("Grinding Configs")]
    [SerializeField] private float grindingSpeedMultiplier;
    [SerializeField] private float minimumSpeedToGrind;
    public float MinimumSpeedToGrind => minimumSpeedToGrind;
    [SerializeField] private float heightOffset;
    public bool IsOnRail;
    private float _railDirection;
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
        _railDirection = transform.rotation.y == 0 ? 1 : -1;
    }

    public void DisableRailing()
    {
        _grindingSpeed = 0f;
        _railDirection = 0f;
        IsOnRail = false;
        _railing = null;
        _rb.linearVelocity = new(_rb.linearVelocityX / 2f, _rb.linearVelocityY);
    }
}
