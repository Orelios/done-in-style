using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TEMP_PlayerScore : MonoBehaviour
{
    [SerializeField] private ScoreCalculator scoreCalculator;
    [SerializeField] private RankCalculator rankCalculator;

    private int _scoreValue = 100;

    /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            DoTrick(100);
        }
    }*/

    public void DoTrick(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            scoreCalculator.IncreaseScoreInstant(_scoreValue, rankCalculator.CurrentStylishRank.ScoreMultiplier);
            rankCalculator.IncreaseStylishPoints();
        }
    }
}
