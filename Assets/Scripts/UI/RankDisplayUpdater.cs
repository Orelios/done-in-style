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
    [SerializeField] private float fillSpeed;
    
    [Header("Rank Name Display")]
    [SerializeField] private Image rankNameDisplay;

    private Coroutine _sliderRoutine;
    
    public void UpdateSliderValue(int value)
    {
        //scoreMultiplierDisplaySlider.value = value;

        if (_sliderRoutine != null)
        {
            StopCoroutine(_sliderRoutine);
        }

        _sliderRoutine = StartCoroutine(UpdateSliderRoutine(value));
    }

    private IEnumerator UpdateSliderRoutine(int targetValue)
    {
        var startValue = scoreMultiplierDisplaySlider.value;
        var elapsedTime = 0f;
        
        while (elapsedTime < 1f)
        {
            elapsedTime += Time.fixedDeltaTime * fillSpeed;
            scoreMultiplierDisplaySlider.value = Mathf.Lerp(startValue, targetValue, elapsedTime);
            yield return null;
        }
        
        scoreMultiplierDisplaySlider.value = targetValue;
    }
    
    public void SetNewSliderLimits(int min, int max)
    {
        scoreMultiplierDisplaySlider.minValue = min;
        scoreMultiplierDisplaySlider.maxValue = max + 1;
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
