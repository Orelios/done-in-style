using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultsDisplayer : MonoBehaviour
{
    [SerializeField] private List<ResultsScreenButtons> resultsScreenButtons;
    
    [SerializeField] private IntEventChannel scoreChannel;
    [SerializeField] private FloatEventChannel timeChannel;
    
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI clearRankText;
    [SerializeField] private float countDuration;

    private void Start()
    {
        scoreChannel.Invoke(GameplayData.FinalScore);
        timeChannel.Invoke(GameplayData.FinalTime);
    }

    public void ShowScoreResult(int score)
    {
        StartCoroutine(CountUpRoutine(score, scoreText));
    }

    public void ShowTimeResult(float time)
    {
        StartCoroutine(CountUpRoutine(time, timeText, false));
    }

    private IEnumerator CountUpRoutine(float targetValue, TextMeshProUGUI textToLerp, bool isScoreCounting = true)
    {
        var elapsedTime = 0f;
        var startingValue = 0;

        while (elapsedTime < countDuration)
        {
            //TODO: convert to input system
            if (Input.anyKeyDown)
            {
                break;
            }
            
            elapsedTime += Time.unscaledDeltaTime;
            var currentValue = Mathf.Lerp(startingValue, targetValue, elapsedTime / countDuration);
            
            if (isScoreCounting)
            {
                textToLerp.text = $"{currentValue:n0}";
            }
            else
            {
                textToLerp.text = $"{Mathf.FloorToInt(currentValue / 60f)}:" +
                                  $"{Mathf.FloorToInt(currentValue % 60f):D2}." +
                                  $"<size=75>{Mathf.FloorToInt(currentValue * 100f % 100f):D2}</size>";
            }
            
            yield return null;
        }

        if (isScoreCounting)
        {
            textToLerp.text = $"{targetValue:n0}";
        }
        else
        {
            textToLerp.text = $"{Mathf.FloorToInt(targetValue / 60f)}:" +
                              $"{Mathf.FloorToInt(targetValue % 60f):D2}." +
                              $"<size=75>{Mathf.FloorToInt(targetValue * 100f % 100f):D2}</size>";
        }

        foreach (var button in resultsScreenButtons)
        {
            button.AllowButtonInteraction();
        }
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
