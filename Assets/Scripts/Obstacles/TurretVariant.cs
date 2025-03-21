using UnityEngine;
using FMOD.Studio;

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
        AudioManager.instance.PlayOneShot(FMODEvents.instance.TMShoot, this.transform.position);
        var bulletIns = Instantiate(bullet, shootPoint.position, Quaternion.identity);
        bulletIns.GetComponent<Rigidbody2D>().AddForce(new Vector2(bulletSpeed, 0));
        Destroy(bulletIns, bulletLifeSpan);
        _shouldShoot = canShootAgain;        
    }

    public void DoTrigger()
    {
        //Shoot();
    }

    public void StopTrigger()
    {
        if (_shouldShoot)
        {
            Shoot();
        }
    }
}
