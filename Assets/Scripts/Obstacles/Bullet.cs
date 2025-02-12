using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector]public ScoreCalculator _scoreCalculator;
    [SerializeField] private int damage; 

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<TEMP_PlayerIFrames>() && !collision.gameObject.GetComponent<TEMP_PlayerIFrames>().IsHit)
        {
            _scoreCalculator.DecreaseScore(damage);
            _scoreCalculator.GetComponent<RankCalculator>().DecreaseStylishPoints();
            collision.gameObject.GetComponent<TEMP_PlayerIFrames>().PlayerHit();
        }

        
        if(!collision.gameObject.TryGetComponent<Bullet>(out Bullet bullet)) { Destroy(gameObject); }
        

    }
}
