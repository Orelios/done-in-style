using UnityEngine;

public class SlidingChecker : MonoBehaviour
{
    [SerializeField] private Transform restartPointRight, restartPointLeft;
    [SerializeField] private Transform closestRespawnPoint;
    private void OnTriggerStay2D(Collider2D collision)
    {
        float distanceLeft = Vector2.Distance(collision.gameObject.transform.position, restartPointLeft.transform.position); 
        float distanceRight = Vector2.Distance(collision.gameObject.transform.position, restartPointRight.transform.position);

        if(distanceLeft < distanceRight) { closestRespawnPoint.transform.position = restartPointLeft.transform.position; }
        else { closestRespawnPoint.transform.position = restartPointRight.transform.position; }
        if (collision.gameObject.TryGetComponent<PlayerTricks>(out var playerTricks))
        {
            if (!playerTricks.IsSliding)
            {
                collision.gameObject.transform.position = closestRespawnPoint.transform.position;
                collision.transform.gameObject.GetComponent<PlayerInvulnerability>().DamagePlayer();
            }
        }
    }
}
