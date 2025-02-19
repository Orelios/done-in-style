using UnityEngine;

public class DestructibleObjects : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerTricks playerTricks) && (playerTricks.IsDashing
            || playerTricks.IsPounding))
        {
            collision.gameObject.GetComponent<PlayerTricks>().AddScoreAndRank();
            collision.gameObject.GetComponent<PlayerTricks>().EnableTrick(gameObject);
            TimeHandler.SlowDownTime();
            Destroy(gameObject);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerTricks playerTricks) && (playerTricks.IsDashing 
            || playerTricks.IsPounding))
        {
            if (collision.gameObject.GetComponent<PlayerTricks>().canTrick == false)
            {
                collision.gameObject.GetComponent<PlayerTricks>().EnableTrick(gameObject);
            }
            TimeHandler.SlowDownTime();
            transform.GetChild(0).GetComponent<Collider2D>().enabled = false;
            Destroy(gameObject);
        }
    }
}
