using Unity.VisualScripting;
using UnityEngine;

public class TEMP_DodgeTrap : MonoBehaviour
{
    private GameObject _ScoreCalculator;
    [SerializeField] private int scoreIncrease = 100; 
    private bool _playerPassed = false;

    private void Start()
    {
        _ScoreCalculator = GameObject.Find("Score System");
    }
    /*
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerInvulnerability>(out PlayerInvulnerability playerInvulnerability))
        {
            if (playerInvulnerability.IsHit)
            {
                _playerPassed = true;
            }
        }
    }
    */
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerInvulnerability>(out PlayerInvulnerability playerInvulnerability))
        {
            if (!playerInvulnerability.IsHit && !_playerPassed)
            {
                _ScoreCalculator.GetComponent<ScoreCalculator>().IncreaseScoreInstant(scoreIncrease,
                _ScoreCalculator.GetComponent<RankCalculator>().CurrentStylishRank.ScoreMultiplier);

                _ScoreCalculator.GetComponent<RankCalculator>().IncreaseStylishPoints();

                _playerPassed = true;
            }
        }
        
    }

    public void PlayerDamaged()
    {
        _playerPassed = true;
    }
    
}
