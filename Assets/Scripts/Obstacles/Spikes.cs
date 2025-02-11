using UnityEngine;

public class Spikes : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private float BounceHeight;
    [SerializeField] private ScoreCalculator scoreCalculator;
    [SerializeField] private int damage; 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<TEMP_PlayerIFrames>() && !collision.gameObject.GetComponent<TEMP_PlayerIFrames>().IsHit)
        {
            scoreCalculator.DecreaseScore(damage);
            scoreCalculator.GetComponent<RankCalculator>().DecreaseStylishPoints();
            collision.gameObject.GetComponent<TEMP_PlayerIFrames>().PlayerHit();
        }

    }
}
