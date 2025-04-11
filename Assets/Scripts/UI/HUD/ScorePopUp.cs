using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public class ScorePopUp : MonoBehaviour
{
    private IObjectPool<ScorePopUp> _popUpPool;
    private Coroutine _returnAfterDelayRoutine;
    private TextMeshProUGUI _scoreText;
    private Vector2 _originalPosition;

    private void Awake()
    {
        _scoreText = GetComponent<TextMeshProUGUI>();
        _originalPosition = GetComponent<RectTransform>().anchoredPosition;
    }

    public void SetPool(IObjectPool<ScorePopUp> pool)
    {
        _popUpPool = pool;
    }

    public void Show(Color colorToUse, int scoreValue, AnimationCurve fadeAnimationCurve, float returnDelay, float moveSpeed)
    {
        gameObject.SetActive(true);
        
        _scoreText.color = colorToUse;
        _scoreText.text = $"{(scoreValue >= 0 ? "+" : "-") + Mathf.Abs(scoreValue)}";
        
        SetTextTransparency(1f);

        if (_returnAfterDelayRoutine != null)
        {
            StopCoroutine(_returnAfterDelayRoutine);
        }

        _returnAfterDelayRoutine = StartCoroutine(ReturnToPoolAfterDelayRoutine(fadeAnimationCurve, returnDelay, moveSpeed));
    }

    public void Hide()
    {
        if (_returnAfterDelayRoutine != null)
        {
            StopCoroutine(_returnAfterDelayRoutine);
            _returnAfterDelayRoutine = null;
        }
        
        _popUpPool?.Release(this);
    }

    private IEnumerator ReturnToPoolAfterDelayRoutine(AnimationCurve animationCurve, float delay, float moveSpeed)
    {
        var elapsedTime = 0f;
        var rectTransform = transform as RectTransform;
        var startPos = _originalPosition;
        rectTransform.anchoredPosition = startPos;

        while (elapsedTime < delay)
        {
            var time  = elapsedTime / delay;
            
            rectTransform.anchoredPosition= startPos + Vector2.up * (time * moveSpeed);
            
            /*var transparency = Mathf.Lerp(1f, 0f, time);
            SetTextTransparency(transparency);*/
            
            var transparencyCurve = animationCurve.Evaluate(time);
            SetTextTransparency(transparencyCurve);
            
            elapsedTime += Time.fixedDeltaTime;
            yield return null;
        }
        
        SetTextTransparency(0f);
        Hide();
    }

    private void SetTextTransparency(float transparency)
    {
        var textColor = _scoreText.color;
        textColor.a = transparency;
        _scoreText.color = textColor;
    }
}
