using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultsScreenButtons : MonoBehaviour
{
    [SerializeField] private float bigFactor;
    [SerializeField] private float duration;
    private Vector2 _original;
    private Button _button;
    
    private void OnEnable()
    {
        _original = transform.localScale;
        _button = GetComponent<Button>();
        
        _button.interactable = false;
    }

    public async void NextLevel()
    {
        var nextLevelHash = SceneUtility.GetScenePathByBuildIndex(GetNextLevelIndex(GameplayData.LastLevelIndex + 1));
        /*Debug.Log($"Last Level Index: {GameplayData.LastLevelIndex}");
        Debug.Log($"Next Level Index: {GameplayData.LastLevelIndex + 1}");
        Debug.Log($"Next Level Path: {nextLevelHash}");
        Debug.Log($"Next Level Hash: {Path.GetFileNameWithoutExtension(nextLevelHash)}");*/

        await SceneLoader.LoadScene(SceneLoader.LoadingScreenHash, Path.GetFileNameWithoutExtension(nextLevelHash));
    }

    private int GetNextLevelIndex(int levelIndex)
    {
        return levelIndex > SceneManager.sceneCountInBuildSettings - 1 ? 3 : levelIndex;
    }

    public async void RetryLevel()
    {
        await SceneLoader.LoadScene(SceneLoader.LoadingScreenHash, GameplayData.LastLevelHash);
    }

    public async void BackToMainMenu()
    {
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

    public void AllowButtonInteraction()
    {
        _button.interactable = true;
    }
}
