using UnityEngine;

public class TEMP_DodgeTrap : MonoBehaviour
{
    [SerializeField]private ScoreCalculator _ScoreCalculator;
    [SerializeField] private int scoreIncrease = 100; 
    private bool _playerPassed;

    private void Awake()
    {
        _playerPassed = false; 
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerInvulnerability>(out PlayerInvulnerability playerInvulnerability))
        {
            if (!playerInvulnerability.IsHit && !_playerPassed)
            {
                _ScoreCalculator.IncreaseScoreInstant(scoreIncrease,
                    _ScoreCalculator.GetComponent<RankCalculator>().CurrentStylishRank.ScoreMultiplier);

                _ScoreCalculator.GetComponent<RankCalculator>().IncreaseStylishPoints();

                _playerPassed = true;

                gameObject.SetActive(false);
            }
        }
        
    }
}
