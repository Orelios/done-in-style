using UnityEngine;

public class DestructibleObjects : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerTricks playerTricks) && (playerTricks.IsDashing
            || playerTricks.IsPounding))
        {
            TimeHandler.SlowDownTime();
            Destroy(gameObject);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerTricks playerTricks) && (playerTricks.IsDashing 
            || playerTricks.IsPounding))
        {
            TimeHandler.SlowDownTime();
            transform.GetChild(0).GetComponent<Collider2D>().enabled = false;
            Destroy(gameObject);
        }
    }
}
