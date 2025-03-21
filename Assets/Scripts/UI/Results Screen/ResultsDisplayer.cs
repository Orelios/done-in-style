using System;
using TMPro;
using UnityEngine;

public class ResultsDisplayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timeText;

    public void ShowScoreResult(int score)
    {
        Debug.Log(score);
        scoreText.text = $"SCORE: <size=100>{score:n0}</size>";
    }

    public void ShowTimeResult(float time)
    {
        timeText.text = $"TIME: <size=100>{Mathf.FloorToInt(time / 60f * Time.fixedDeltaTime)}:{Mathf.FloorToInt(time % 60f):D2}.{Mathf.FloorToInt(time * 100f % 100f):D2}</size>";
    }
}
