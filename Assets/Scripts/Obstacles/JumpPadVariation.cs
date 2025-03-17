using UnityEngine;

public class JumpPadVariation : MonoBehaviour
{
    [Header("Jump Pad Variation components")]
    [SerializeField] private Transform target;
    [SerializeField] private float bounceHeight;
    [SerializeField] private float maxHeight;

    private Vector2 _direction;
    private void Update()
    {
        FindTrajectory(); 
    }

    private void FindTrajectory()
    {
        Vector2 targetPos = target.position;
        _direction = targetPos - (Vector2)transform.position;

        //RaycastHit2D hit = Physics2D.Raycast(transform.position, _direction, bouncePadRange, LayerMask.GetMask(targetLayerName));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.GetComponent<Rigidbody2D>().AddForce(_direction * bounceHeight, ForceMode2D.Impulse);
        Debug.Log(_direction * bounceHeight);

        collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity =
                Vector2.ClampMagnitude(collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity, maxHeight);
    }
}
