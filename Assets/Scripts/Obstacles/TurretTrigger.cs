using System.Collections;
using UnityEngine;

public class TurretTrigger : MonoBehaviour
{
    [SerializeField] private TurretVariant[] turretVariant;
    [SerializeField] private float shootCooldown; 
    private bool _canShoot;

    private void Awake()
    {
        _canShoot = true; 
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_canShoot)
        {
            for (int x = 0; x < turretVariant.Length; x++)
            {
                turretVariant[x].Shoot();
            }
            StartCoroutine(ShootCooldown());
        }
       
    }

    private IEnumerator ShootCooldown()
    {
        _canShoot = false;

        yield return new WaitForSeconds(shootCooldown);

        _canShoot = true; 
    }
}
