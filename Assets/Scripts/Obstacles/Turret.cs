using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("Turret Components")]
    [SerializeField] private float towerRange;
    [SerializeField] private Transform target;
    [SerializeField] private float shootCooldown;
    private float _lastShootTime;

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
            if (hit.collider.gameObject.GetComponent<TEMP_PlayerIFrames>())
            {
                transform.GetChild(0).GetComponent<Transform>().up = -_direction;

                if (Time.time >= _lastShootTime + shootCooldown)
                {
                    Shoot();
                }
            }
        }
    }

    private void Shoot()
    {
        bullet.gameObject.GetComponent<Bullet>()._scoreCalculator = scoreCalculator;
        GameObject bulletIns = Instantiate(bullet, shootPoint.position, Quaternion.identity);
        bulletIns.GetComponent<Rigidbody2D>().AddForce(_direction * bulletSpeed);
        //Physics2D.IgnoreCollision(bulletIns.GetComponent<Collider2D>(), bullet.gameObject.GetComponent<Collider2D>());
        Destroy(bulletIns, bulletLifeSpan);
        _lastShootTime = Time.time;
    }

}
