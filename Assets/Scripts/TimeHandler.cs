using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TimeHandler : MonoBehaviour
{
    [SerializeField] private FloatEventChannel timeEventChannel;
    [SerializeField, Range(0.1f, 1f)] private float slowDownFactor;
    [SerializeField] private float slowDownDuration;
    [SerializeField, Range(0.1f, 1f)] private float percentageForSmoothingToStart;
    
    private static float _originalTimeScale = 1f;
    private static float _slowDownFactor;
    private static float _slowDownDuration;
    private static float _percentageForSmoothingToStart;

    private float _elapsedTime;
    public float ElapsedTime => _elapsedTime;
    private void Awake()
    {
        Time.timeScale = _originalTimeScale;
        _slowDownFactor = slowDownFactor;
        _slowDownDuration = slowDownDuration;
    }

    private void FixedUpdate()
    {
        _elapsedTime += Time.fixedDeltaTime;
        
        timeEventChannel.Invoke(_elapsedTime);
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

            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            elapsedTime += Time.unscaledDeltaTime;
            
            yield return null;
        }
        
        Time.timeScale = _originalTimeScale;
        Time.fixedDeltaTime = 0.02f;
    }
}
