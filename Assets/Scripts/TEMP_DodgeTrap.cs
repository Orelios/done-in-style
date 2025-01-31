using UnityEngine;

public class TEMP_DodgeTrap : MonoBehaviour
{
    [SerializeField]private TEMP_ScoreCalculator _ScoreCalculator;
    private bool _playerPassed;

    private void Awake()
    {
        _playerPassed = false; 
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.GetComponent<TEMP_PlayerIFrames>().IsHit && !_playerPassed)
        {
            _ScoreCalculator.AddScore(100, 
                _ScoreCalculator.GetComponent<Temp_RankCalculator>().CurrentStylishRank.ScoreMultiplier);

            _ScoreCalculator.GetComponent<Temp_RankCalculator>().IncreaseStylishPoints();

            _playerPassed=true;
        }
    }
}
