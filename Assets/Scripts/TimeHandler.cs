using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TimeHandler : MonoBehaviour
{
    [SerializeField, Range(0.1f, 1f)] private float slowDownFactor;
    [SerializeField] private float slowDownDuration;
    [SerializeField, Range(0.1f, 1f)] private float percentageForSmoothingToStart;
    [SerializeField] private TextMeshProUGUI timerText;
    
    private static float _originalTimeScale = 1f;
    private static float _slowDownFactor;
    private static float _slowDownDuration;
    private static float _percentageForSmoothingToStart;

    private float _elapsedTime;
    private void Awake()
    {
        Time.timeScale = _originalTimeScale;
        _slowDownFactor = slowDownFactor;
        _slowDownDuration = slowDownDuration;
    }

    private void Update()
    {
        _elapsedTime += Time.deltaTime;
        
        timerText.text = $"{Mathf.FloorToInt(_elapsedTime / 60f)} : {Mathf.FloorToInt(_elapsedTime % 60f):D2}.{Mathf.FloorToInt((_elapsedTime * 100f) % 100f):D2}";
    }

    public static void SlowDownTime()
    {
        Time.timeScale = _slowDownFactor;
        
        CoroutineHandler.StartTimeSlowCoroutine(RestoreTimeScaleRoutine(_slowDownDuration));
    }

    private static IEnumerator RestoreTimeScaleRoutine(float duration)
    {
        var elapsedTime = 0f;
        var initialTimeScale = Time.timeScale;

        while (elapsedTime < duration)
        {
            var progress = elapsedTime / duration;

            Time.timeScale = progress >= _percentageForSmoothingToStart ? 
                Mathf.Lerp(_slowDownFactor, _originalTimeScale, (_slowDownDuration * _percentageForSmoothingToStart) / duration) : 
                Mathf.Lerp(initialTimeScale, _originalTimeScale, elapsedTime / (duration * _percentageForSmoothingToStart));
            
            
            elapsedTime += Time.unscaledDeltaTime;
            
            yield return null;
        }
        
        Time.timeScale = _originalTimeScale;
    }
}
