using UnityEngine;

public class TEMP_PlayerRailGrind : MonoBehaviour
{
    [Header("Grinding Configs")]
    [SerializeField] private float grindSpeed;
    [SerializeField] private float heightOffset;
    private bool _onRail;
    private float _railDirection;
    
    private Rigidbody2D _rb;
    private TEMP_Railing _railing;

    private void Start()
    {
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
        if (other.gameObject.CompareTag("Rail") && GetComponent<PlayerGearSwapper>().CurrentGearEquipped.DaredevilGearType == EDaredevilGearType.RollerBlades)
        {
            _onRail = true;
            _railing = other.gameObject.GetComponent<TEMP_Railing>();
            _railDirection = Mathf.Sign(transform.localScale.x);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Rail"))
        {
            _onRail = false;
            _railing = null;
        }
    }

    private void GrindRail()
    {
        if (_onRail && _railing is not null)
        {
            _rb.linearVelocity = new Vector2(_railDirection * grindSpeed, 0);
            
        }
    }
}
