using UnityEngine;

public class TurretVariant : MonoBehaviour
{
    [Header("Bullet Components")]
    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float bulletLifeSpan;

    [Header("Components")]
    [SerializeField] private ScoreCalculator scoreCalculator;

    public void Shoot()
    {
        var bulletIns = Instantiate(bullet, shootPoint.position, Quaternion.identity);
        bulletIns.GetComponent<Rigidbody2D>().AddForce(new Vector2(bulletSpeed, 0));
        Destroy(bulletIns, bulletLifeSpan);
    }
}
