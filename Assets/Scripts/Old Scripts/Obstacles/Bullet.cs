using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector]public TEMP_ScoreCalculator _scoreCalculator;
    [SerializeField] private int damage; 

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<GearTricks>())
        {
            if (!collision.gameObject.GetComponent<TEMP_PlayerIFrames>().IsHit)
            {
                _scoreCalculator.DecreaseScore(damage);
                _scoreCalculator.GetComponent<Temp_RankCalculator>().DecreaseStylishPoints();
                collision.gameObject.GetComponent<TEMP_PlayerIFrames>().PlayerHit();
                collision.gameObject.GetComponent<PlayerHealth>().DecreaseHealth();
            }
        }
        
        Destroy(gameObject); 
    }
}
