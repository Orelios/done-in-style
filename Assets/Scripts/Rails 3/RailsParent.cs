using UnityEngine;

public class RailsParent : MonoBehaviour
{
    public bool hasTricked = false;
    public bool hasGivenScore = false;
    public Graffiti graffiti;
    private bool hasAppliedGraffiti;
    private PlayerTricks _playerTricks;

    private Coroutine _scoreRoutine;
    private Coroutine _rankRoutine;

    [Header("Components")]
    [SerializeField] private ScoreCalculator scoreCalculator;
    [SerializeField] private RankCalculator rankCalculator;

    [Header("Points Configs")]
    [SerializeField] private int pointsPerSecond;
    [SerializeField] private int maxTimeForPoints;
    public void GiveScore()
    {
        if (!hasGivenScore)
        {
            _playerTricks.AddScoreAndRank();
            hasGivenScore = true;
        }
    }

    public void ApplyGraffiti()
    {
        if (!hasAppliedGraffiti)
        {
            if (graffiti != null)
            {
                graffiti.StartGraffiti();
            }
            hasAppliedGraffiti = true;
        }
    }

    public void CheckTrickMove()
    {
        if (!hasTricked)
        {
            _playerTricks.DisableCanTrick();
            _playerTricks.EnableTrick(gameObject);
            hasTricked = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (!hasGivenScore)
        {
            _scoreRoutine = StartCoroutine(scoreCalculator.IncreaseScoreContinuousRoutine(pointsPerSecond, rankCalculator.CurrentStylishRank.ScoreMultiplier, maxTimeForPoints));
            //_rankRoutine = StartCoroutine(rankCalculator.IncreaseStylishPointsContinuousRoutine(maxTimeForPoints));
            
        }
        
        if (collision.gameObject.TryGetComponent<PlayerTricks>(out var playerTricks))
        {
            _playerTricks = playerTricks;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        StopAllCoroutines();
        ApplyGraffiti();
        CheckTrickMove();
    }
}
