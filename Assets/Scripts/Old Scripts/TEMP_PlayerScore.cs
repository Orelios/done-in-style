using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TEMP_PlayerScore : MonoBehaviour
{
    [SerializeField] private TEMP_ScoreCalculator scoreCalculator;
    [SerializeField] private Temp_RankCalculator rankCalculator;

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
            scoreCalculator.AddScore(_scoreValue, rankCalculator.CurrentStylishRank.ScoreMultiplier);
            rankCalculator.IncreaseStylishPoints();
        }
    }
}
