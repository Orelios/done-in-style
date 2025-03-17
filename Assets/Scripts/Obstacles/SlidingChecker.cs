using UnityEngine;

public class SlidingChecker : MonoBehaviour
{
    [SerializeField] private Transform restartPointRight, restartPointLeft;
    private Transform _closestRespawnPoint; 
    private void OnTriggerStay2D(Collider2D collision)
    {
        restartPointLeft.position = collision.gameObject.transform.position - restartPointLeft.transform.position;
        restartPointRight.position = collision.gameObject.transform.position - restartPointRight.transform.position;
        //_closestRespawnPoint =  
        if (collision.gameObject.TryGetComponent<PlayerTricks>(out var playerTricks))
        {
            if (!playerTricks.IsSliding)
            {
                collision.gameObject.transform.position = restartPointLeft.position;
                collision.transform.gameObject.GetComponent<PlayerInvulnerability>().DamagePlayer(); 
            }
        }
    }
}
