using System.Collections;
using UnityEngine;

public class PlayerRailGrind : MonoBehaviour
{
    [Header("Grinding Configs")]
    [SerializeField] private float grindingSpeedMultiplier;
    [SerializeField] private float minimumSpeedToGrind;
    public float MinimumSpeedToGrind => minimumSpeedToGrind;
    [SerializeField] private Vector2 momentumDecay = new Vector2(0.345f, 0.69f);
    
    public bool IsOnRail;
    private float _railDirection;
    private float _grindingSpeed;
    private Quaternion _rotationBeforeGrinding;
    private SpriteRenderer _playerSprite;
    
    private Rigidbody2D _rb;
    private Railing _railing;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerSprite = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
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
    
    private IEnumerator DecayMomentum()
    {
        Vector2 lastVelocity = _rb.linearVelocity;

        _rb.linearVelocity = new(lastVelocity.x > 0 ? lastVelocity.x - momentumDecay.x : lastVelocity.x + momentumDecay.x, lastVelocity.y - momentumDecay.y);
        
        yield return null;
    }

    public void EnableRailing(Railing railing)
    {
        _rotationBeforeGrinding = transform.rotation;
        IsOnRail = true;
        _railing = railing;
        _grindingSpeed = _rb.linearVelocityX * grindingSpeedMultiplier;
        _railDirection = transform.rotation.y == 0 ? 1 : -1;
        _playerSprite.gameObject.transform.rotation = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y, Mathf.Approximately(_railDirection, 1) ? railing.transform.localEulerAngles.z : -railing.transform.localEulerAngles.z);
    }

    public void DisableRailing()
    {
        _grindingSpeed = 0f;
        _railDirection = 0f;
        IsOnRail = false;
        _railing = null;
        _playerSprite.gameObject.transform.rotation = _rotationBeforeGrinding;
        StartCoroutine(DecayMomentum());
    }
}