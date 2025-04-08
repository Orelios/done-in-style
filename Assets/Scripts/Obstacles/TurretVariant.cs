using System;
using UnityEngine;
using FMOD.Studio;

public class TurretVariant : MonoBehaviour, ITriggerable
{
    [Header("Turret Configs")] 
    [SerializeField] private EBehaviourType shootWhen;
    [SerializeField] private float fireRate;
    
    [Header("Bullet Components")]
    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float bulletLifeSpan;
    [SerializeField] private bool canShootAgain;

    private bool _shouldShoot = true;
    private float _timeElapsed;

    private void Update()
    {
        if (shootWhen == EBehaviourType.PlayerStays)
        {
            _timeElapsed += Time.deltaTime;
        }
    }

    public void Shoot()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.TMShoot, this.transform.position);
        var bulletIns = Instantiate(bullet, shootPoint.position, Quaternion.identity);
        bulletIns.GetComponent<Rigidbody2D>().linearVelocity = transform.right * bulletSpeed;
        Destroy(bulletIns, bulletLifeSpan);
        _shouldShoot = canShootAgain;

        if (shootWhen == EBehaviourType.PlayerStays)
        {
            _timeElapsed = 0f;
        }
    }

    public void DoTriggerEnter()
    {
        if (_shouldShoot && shootWhen == EBehaviourType.PlayerEnters)
        {
            Shoot();
        }
    }
    
    public void DoTriggerStay()
    {
        if (_shouldShoot && shootWhen == EBehaviourType.PlayerStays && _timeElapsed >= fireRate)
        {
            Shoot();
        }
    }

    public void StopTriggerExit()
    {
        if (_shouldShoot && shootWhen == EBehaviourType.PlayerExits)
        {
            Shoot();
        }
    }
}

public enum EBehaviourType
{
    PlayerEnters,
    PlayerStays,
    PlayerExits
}
