using UnityEngine;

public class DestructibleObjects : MonoBehaviour
{
    private Onomatopoeia _type;

    private void Start()
    {
        _type = transform.parent.GetComponentInChildren<Onomatopoeia>();
        #region Makes sure this Destructible Object is the one with a Canvas
        if (_type == null)
        {
            Destroy(gameObject);
        }
        #endregion
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerTricks playerTricks) && (playerTricks.IsDashing
            || playerTricks.IsPounding))
        {
            collision.gameObject.GetComponent<PlayerTricks>().AddScoreAndRank();
            playerTricks.EnableTrickDestroyed();
            TimeHandler.SlowDownTime();
            _type.StartTypingVFX();
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
                playerTricks.EnableTrickDestroyed();
            }
            TimeHandler.SlowDownTime();
            transform.GetChild(0).GetComponent<Collider2D>().enabled = false;
            _type.StartTypingVFX();
            Destroy(gameObject);
        }
    }
}
