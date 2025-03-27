using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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

    public async void NextLevel()
    {
        var nextLevelHash = SceneUtility.GetScenePathByBuildIndex(Mathf.Max(GameplayData.LastLevelIndex + 1, 3));
        //var nextLevelIndex = SceneUtility.GetBuildIndexByScenePath(nextLevelHashTemp);
        
        //SceneManager.LoadScene(SceneUtility.GetScenePathByBuildIndex(Mathf.Max(nextLevelIndex, 2)));
        await SceneLoader.LoadScene(SceneLoader.LoadingScreenHash, Path.GetFileNameWithoutExtension(nextLevelHash));
    }

    public async void RetryLevel()
    {
        //SceneManager.LoadScene(GameplayData.LastLevelHash);
        await SceneLoader.LoadScene(SceneLoader.LoadingScreenHash, GameplayData.LastLevelHash);
    }

    public async void BackToMainMenu()
    {
        //SceneManager.LoadScene("TitleScreen");
        await SceneLoader.LoadScene(SceneLoader.LoadingScreenHash, SceneLoader.TitleScreenHash);
    }

    public void Big()
    {
        StartCoroutine(ChangeScaleRoutine(new(bigFactor, bigFactor)));
    }

    public void Small()
    {
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
