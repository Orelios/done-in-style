using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultsScreenButtons : MonoBehaviour
{
    [SerializeField] private float bigFactor;
    [SerializeField] private float duration;
    private Vector2 _original;

    private void OnEnable()
    {
        _original = transform.localScale;
    }

    public void NextLevel()
    {
        
    }

    public void RetryLevel()
    {
        SceneManager.LoadScene(GameplayData.LastLevelHash);
    }

    public void BackToMainMenu()
    {
        
    }

    public void Big()
    {
        //transform.localScale = new(bigFactor, bigFactor);
        StartCoroutine(ChangeScaleRoutine(new(bigFactor, bigFactor)));
    }

    public void Small()
    {
        //transform.localScale = _original;
        StartCoroutine(ChangeScaleRoutine(_original));
    }

    private IEnumerator ChangeScaleRoutine(Vector2 targetScale)
    {
        var elapsedTime = 0f;
        
        while (elapsedTime < duration)
        {
            // Calculate the lerp factor
            float lerpFactor = elapsedTime / duration;

            // Lerp the scale
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, lerpFactor);

            // Increase elapsed time based on frame time
            elapsedTime += Time.unscaledDeltaTime;

            // Wait until the next frame
            yield return null;
        }

        // Ensure the final scale is exactly the target scale
        transform.localScale = targetScale;
    }
}
