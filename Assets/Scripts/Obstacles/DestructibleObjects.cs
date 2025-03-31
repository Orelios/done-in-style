using UnityEngine;

public class DestructibleObjects : MonoBehaviour
{
    private Onomatopoeia _type;
    [SerializeField] private bool dashable = true, poundable = true, doubleJumpable = true;

    private void Start()
    {
        #region Makes sure this Destructible Object is the one with a Canvas
        if (!transform.parent.TryGetComponent<OnomatopoeiaManager>(out _))
        {
            Destroy(gameObject);  
        }
        #endregion
        _type = transform.GetComponentInParent<OnomatopoeiaManager>().onomatopoeia.GetComponent<Onomatopoeia>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerTricks playerTricks) && ((dashable ? (playerTricks.IsDashing || playerTricks.DashCanDestroy) : false)
            || (poundable ? playerTricks.IsPounding : false) || (doubleJumpable ? playerTricks.CanDestroy : false)))
        {
            collision.gameObject.GetComponent<PlayerTricks>().AddScoreAndRank();
            playerTricks.EnableTrickDestroyed();
            TimeHandler.SlowDownTime();
            _type.StartTypingVFX();
            AudioManager.instance.PlayOneShot(FMODEvents.instance.Slowmo, this.transform.position);
            AudioManager.instance.PlayOneShot(FMODEvents.instance.BreakObstacle, this.transform.position);
            Destroy(gameObject);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerTricks playerTricks) && ((dashable ? (playerTricks.IsDashing || playerTricks.DashCanDestroy) : false)
            || (poundable ? playerTricks.IsPounding : false) || (doubleJumpable ? playerTricks.CanDestroy : false)))
        {
            if (collision.gameObject.GetComponent<PlayerTricks>().canTrick == false)
            {
                playerTricks.EnableTrickDestroyed();
            }
            TimeHandler.SlowDownTime();
            transform.GetChild(0).GetComponent<Collider2D>().enabled = false;
            _type.StartTypingVFX();
            AudioManager.instance.PlayOneShot(FMODEvents.instance.Slowmo, this.transform.position);
            AudioManager.instance.PlayOneShot(FMODEvents.instance.BreakObstacle, this.transform.position);
            Destroy(gameObject);
        }
    }
}
