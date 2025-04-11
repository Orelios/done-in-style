using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RankDisplayUpdater : MonoBehaviour
{
    [Header("Score Multiplier Display")]
    [SerializeField] private Slider scoreMultiplierDisplaySlider;
    [SerializeField] private Image scoreMultiplierBase;
    [SerializeField] private Image scoreMultiplierOutline;
    [SerializeField] private Image scoreMultiplierFill;
    
    [Header("Rank Name Display")]
    [SerializeField] private Image rankNameDisplay;
    
    public void UpdateSliderValue(int value)
    {
        scoreMultiplierDisplaySlider.value = value;
    }
    
    public void SetNewSliderLimits(int min, int max)
    {
        scoreMultiplierDisplaySlider.minValue = min;
        scoreMultiplierDisplaySlider.maxValue = max + 1;
    }

    public void SetNewSliderMinLimit(StylishRankSO rank)
    {
        scoreMultiplierDisplaySlider.minValue = rank.RequiredBreakthroughPoints;
    }
    
    public void SetNewSliderMaxLimit(StylishRankSO rank)
    {
        scoreMultiplierDisplaySlider.minValue = rank.RequiredBreakthroughPoints + 1;
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
