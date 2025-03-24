using UnityEngine;

public class TurretVariant : MonoBehaviour, ITriggerable
{
    [Header("Turret Configs")]
    
    [Header("Bullet Components")]
    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float bulletLifeSpan;
    [SerializeField] private bool canShootAgain;

    [Header("Components")]
    [SerializeField] private ScoreCalculator scoreCalculator;

    private bool _shouldShoot = true;
    
    public void Shoot()
    {
        var bulletIns = Instantiate(bullet, shootPoint.position, Quaternion.identity);
        bulletIns.GetComponent<Rigidbody2D>().AddForce(transform.right * bulletSpeed);
        Destroy(bulletIns, bulletLifeSpan);
        _shouldShoot = canShootAgain;
    }

    public void DoTrigger()
    {
        if (_shouldShoot)
        {
            Shoot();
        }
    }

    public void StopTrigger()
    {
        /*if (_shouldShoot)
        {
            Shoot();
        }*/
    }
}
