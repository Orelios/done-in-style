using FMOD.Studio;
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

    public EventInstance _resultsScreenPointsIncrease;

    [NonSerialized] public string pointsTransition;

    private void Start()
    {
        scoreChannel.Invoke(GameplayData.FinalScore);
        timeChannel.Invoke(GameplayData.FinalTime);
        _resultsScreenPointsIncrease = AudioManager.instance.CreateInstance(FMODEvents.instance.ResultsScreenPointsIncrease);

        pointsTransition = "points_transition";

        _resultsScreenPointsIncrease.start();
        _resultsScreenPointsIncrease.setParameterByName(pointsTransition, 1);
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
                textToLerp.text = $"{currentValue:N0}";
            }
            else
            {
                textToLerp.text = $"{Mathf.FloorToInt(currentValue / 60f):D2}:" +
                                  $"{Mathf.FloorToInt(currentValue % 60f):D2}" +
                                  $"<font=\"Grandstander Stroke 2\"><size=75>.{Mathf.FloorToInt(currentValue * 100f % 100f):D2}</font></size>";
            }
            
            yield return null;
        }

        if (isScoreCounting)
        {
            textToLerp.text = $"{targetValue:N0}";
        }
        else
        {
            _resultsScreenPointsIncrease.setParameterByName(pointsTransition, 0);
            AudioManager.instance.InitializeMusic(FMODEvents.instance.ResultsScreenMusic);
            textToLerp.text = $"{Mathf.FloorToInt(targetValue / 60f):D2}:" +
                              $"{Mathf.FloorToInt(targetValue % 60f):D2}" +
                              $"<font=\"Grandstander Stroke 2\"><size=75>.{Mathf.FloorToInt(targetValue * 100f % 100f):D2}</font></size>";
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
