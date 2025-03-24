using System;
using TMPro;
using UnityEngine;

public class ResultsDisplayer : MonoBehaviour
{
    [SerializeField] private IntEventChannel scoreChannel;
    [SerializeField] private FloatEventChannel timeChannel;
    
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI clearRankText;

    private void Start()
    {
        /*ShowScoreResult(ResultsData.FinalScore);
        ShowTimeResult(ResultsData.FinalTime);*/
        scoreChannel.Invoke(ResultsData.FinalScore);
        timeChannel.Invoke(ResultsData.FinalTime);
    }

    public void ShowScoreResult(int score)
    {
        scoreText.text = $"SCORE: <size=100>{score:n0}</size>";
    }

    public void ShowTimeResult(float time)
    {
        timeText.text = $"TIME: <size=100>{Mathf.FloorToInt(time / 60f * Time.fixedDeltaTime)}:{Mathf.FloorToInt(time % 60f):D2}.{Mathf.FloorToInt(time * 100f % 100f):D2}</size>";
    }
    
    public void DetermineClearRank(int score)
    {
        clearRankText.text = score switch
        {
            < 15000 => "L",
            < 20000 => "C",
            < 25000 => "B",
            < 30000 => "A",
            _ => "S"
        };
    }
}
