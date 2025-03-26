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
        var nextLevelHash = SceneUtility.GetScenePathByBuildIndex(GameplayData.LastLevelIndex + 1);
        var nextLevelIndex = SceneUtility.GetBuildIndexByScenePath(nextLevelHash);
        
        SceneManager.LoadScene(SceneUtility.GetScenePathByBuildIndex(Mathf.Max(nextLevelIndex, 1)));
    }

    public void RetryLevel()
    {
        SceneManager.LoadScene(GameplayData.LastLevelHash);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("TitleScreen");
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
            var lerpFactor = elapsedTime / duration;
            
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, lerpFactor);
            
            elapsedTime += Time.unscaledDeltaTime;
            
            yield return null;
        }
        
        transform.localScale = targetScale;
    }
}
