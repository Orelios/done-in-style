using UnityEngine;

public class PlayerRailGrind : MonoBehaviour
{
    [Header("Grinding Configs")]
    [SerializeField] private float grindingSpeedMultiplier;
    [SerializeField] private float heightOffset;
    private bool _onRail;
    private float _railDirection;
    private float _grindingSpeed;
    
    private PlayerMovement _playerMovement;
    private Rigidbody2D _rb;
    private Railing _railing;

    private void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (_onRail)
        {
            GrindRail();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Rail"))
        {
            /*if (Mathf.Abs(_rb.linearVelocityX) < 3f)
            {
                Physics2D.IgnoreCollision(other.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
                return;
            }*/
            _onRail = true;
            _railing = other.gameObject.GetComponent<Railing>();
            _grindingSpeed = _rb.linearVelocityX * grindingSpeedMultiplier;
            _railDirection = transform.rotation.y == 0 ? 1 : -1;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Rail"))
        {
            //Physics2D.IgnoreCollision(other.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>(), false);
            _grindingSpeed = 0f;
            _railDirection = 0f;
            _onRail = false;
            _railing = null;
            _rb.linearVelocity = new(0f, _rb.linearVelocityY);
        }
    }

    private void GrindRail()
    {
        if (_onRail && _railing is not null)
        {
            _rb.linearVelocity = new Vector2(_grindingSpeed, _rb.linearVelocity.y);
        }
    }
}
