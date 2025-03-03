using UnityEngine;

public class TurretVariant : MonoBehaviour
{
    [Header("Turret Components")]
    [SerializeField] private float towerRange;
    [SerializeField] private Transform target;

    [Header("Bullet Components")]
    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float bulletLifeSpan;
    private bool _detected;
    private Vector2 _direction;

    [Header("Components")]
    [SerializeField] private ScoreCalculator scoreCalculator;

    private void Update()
    {
        DetectPlayer();
    }

    private void DetectPlayer()
    {
        Vector2 targetPos = target.position;
        _direction = targetPos - (Vector2)transform.position;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, _direction, towerRange, LayerMask.GetMask("Player"));

        if (hit)
        {
            if (hit.collider.gameObject.GetComponent<PlayerInvulnerability>())
            {
                transform.GetChild(0).GetComponent<Transform>().up = -_direction;
            }
        }
    }

    public void Shoot()
    {
        var bulletIns = Instantiate(bullet, shootPoint.position, Quaternion.identity);
        bulletIns.GetComponent<Rigidbody2D>().AddForce(_direction * bulletSpeed);
        Destroy(bulletIns, bulletLifeSpan);
    }
}
