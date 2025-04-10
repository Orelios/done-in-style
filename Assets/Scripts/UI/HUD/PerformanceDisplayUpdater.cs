using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PerformanceDisplayUpdater : MonoBehaviour
{
    [Header("Score Display")]
    [SerializeField] private TextMeshProUGUI scoreText;
    
    [Header("Time Display")]
    [SerializeField] private TextMeshProUGUI timerText;
    
    [Header("Multiplier Display")]
    [SerializeField] private Slider scoreMultiplierDisplaySlider;
    [SerializeField] private Image scoreMultiplierBase;
    [SerializeField] private Image scoreMultiplierOutline;
    [SerializeField] private Image scoreMultiplierFill;
    [SerializeField] private float lerpDuration;
    
    [Header("Rank Display")]
    [SerializeField] private Image rankNameDisplay;

    public void UpdateScoreDisplay(int score)
    {
        scoreText.text = $"{score : 00000}";
    }

    public void UpdateTimerDisplay(float time)
    {
        timerText.text = $"{Mathf.FloorToInt(time / 60f)}:{Mathf.FloorToInt(time % 60f):D2}.{Mathf.FloorToInt(time * 100f % 100f):D2}";
    }
    
    public void UpdateSliderValue(int value)
    {
        //scoreMultiplierDisplaySlider.value = value;
        StartCoroutine(UpdateSliderValueRoutine((int)scoreMultiplierDisplaySlider.value, value, lerpDuration));
    }

    private IEnumerator UpdateSliderValueRoutine(int startValue, int endValue, float duration)
    {
        var elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            scoreMultiplierDisplaySlider.value = Mathf.Lerp(startValue, endValue, elapsedTime / duration);
            
            elapsedTime += Time.fixedDeltaTime;
                
            yield return null;
        }
        
        scoreMultiplierDisplaySlider.value = endValue;
    }
    
    public void SetNewSliderMinLimit(StylishRankSO rank)
    {
        scoreMultiplierDisplaySlider.minValue = rank.RankType switch
        {
            ERankType.NoRank => 0,
            ERankType.Loser => 1,
            ERankType.Cooking => 5,
            ERankType.Ballin => 11,
            ERankType.Awesome => 17,
            ERankType.Stylish => 24,
            _ => scoreMultiplierDisplaySlider.minValue
        };
    }
    
    public void SetNewSliderMaxLimit(StylishRankSO rank)
    {
        scoreMultiplierDisplaySlider.maxValue = rank.RequiredBreakthroughPoints;
    }
    
    public void UpdateRankDisplay(StylishRankSO rank)
    {
        rankNameDisplay.sprite = rank.RankNameSprite;
        rankNameDisplay.gameObject.SetActive(rankNameDisplay.sprite);
        rankNameDisplay.SetNativeSize();
        
        scoreMultiplierBase.sprite = rank.ScoreMultiplierBaseSprite;
        scoreMultiplierBase.gameObject.SetActive(scoreMultiplierBase.sprite);
        scoreMultiplierBase.SetNativeSize();
        
        scoreMultiplierOutline.sprite = rank.ScoreMultiplierOutlineSprite;
        scoreMultiplierOutline.gameObject.SetActive(scoreMultiplierOutline.sprite);
        scoreMultiplierOutline.SetNativeSize();
        
        scoreMultiplierFill.color = rank.ScoreMultiplierFillColor;
    }
}
