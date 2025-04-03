using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
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
        var nextLevelHash = SceneUtility.GetScenePathByBuildIndex(GetNextLevelIndex(GameplayData.LastLevelIndex + 1));
        //var nextLevelIndex = SceneUtility.GetBuildIndexByScenePath(nextLevelHashTemp);
        Debug.Log($"Last Level Index: {GameplayData.LastLevelIndex}");
        Debug.Log($"Next Level Index: {GameplayData.LastLevelIndex + 1}");
        Debug.Log($"Next Level Path: {nextLevelHash}");
        Debug.Log($"Next Level Hash: {Path.GetFileNameWithoutExtension(nextLevelHash)}");
        //SceneManager.LoadScene(SceneUtility.GetScenePathByBuildIndex(Mathf.Max(nextLevelIndex, 2)));
        await SceneLoader.LoadScene(SceneLoader.LoadingScreenHash, Path.GetFileNameWithoutExtension(nextLevelHash));
    }

    private int GetNextLevelIndex(int levelIndex)
    {
        return levelIndex > SceneManager.sceneCountInBuildSettings - 1 ? 3 : levelIndex;
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
