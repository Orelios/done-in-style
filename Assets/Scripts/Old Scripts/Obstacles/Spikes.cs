using UnityEngine;

public class Spikes : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private float BounceHeight;
    [SerializeField] private TEMP_ScoreCalculator scoreCalculator;
    [SerializeField] private int damage; 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<TEMP_PlayerIFrames>())
        {
            if (!collision.gameObject.GetComponent<TEMP_PlayerIFrames>().IsHit)
            {
                collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity =
                    new Vector2((-BounceHeight * 2) * collision.gameObject.GetComponent<Transform>().transform.rotation.y == 0 ? 1 : -1, BounceHeight);

                scoreCalculator.DecreaseScore(damage);
                scoreCalculator.GetComponent<Temp_RankCalculator>().DecreaseStylishPoints();

                collision.gameObject.GetComponent<TEMP_PlayerIFrames>().PlayerHit();
                collision.gameObject.GetComponent<PlayerHealth>().DecreaseHealth();
            }
        }
        
    }
}
