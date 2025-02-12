using UnityEngine;

public class DestructibleObjects : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerTricks playerTricks) && playerTricks.IsDoingTrick())
        {
            //TODO: ADD POINTS WHEN DESTROYED
            TimeHandler.SlowDownTime();
            Destroy(gameObject);
        }
    }
}
