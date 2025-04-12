using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultsScreenButtons : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Texture2D normalCursor;
    [SerializeField] private Texture2D highlightedCursor;
    [SerializeField] private GameObject demoEndScreen;
    [SerializeField] private float bigFactor;
    [SerializeField] private float duration;
    [SerializeField] private bool isInteractableAtStart;
    private Vector2 _original;
    private Button _button;
    
    private void OnEnable()
    {
        _original = transform.localScale;
        _button = GetComponent<Button>();
        
        _button.interactable = isInteractableAtStart;
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        Cursor.SetCursor(highlightedCursor, new(50, 0), CursorMode.Auto);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(normalCursor, new(33, 2), CursorMode.Auto);
    }

    public async void NextLevel()
    {
        if (GameplayData.LastLevelIndex < SceneManager.sceneCountInBuildSettings - 1)
        {
            var nextLevelHash = SceneUtility.GetScenePathByBuildIndex(GetNextLevelIndex(GameplayData.LastLevelIndex + 1));
            /*Debug.Log($"Last Level Index: {GameplayData.LastLevelIndex}");
            Debug.Log($"Next Level Index: {GameplayData.LastLevelIndex + 1}");
            Debug.Log($"Next Level Path: {nextLevelHash}");
            Debug.Log($"Next Level Hash: {Path.GetFileNameWithoutExtension(nextLevelHash)}");*/
            AudioManager.instance.musicEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            await SceneLoader.LoadScene(SceneLoader.LoadingScreenName, Path.GetFileNameWithoutExtension(nextLevelHash));
        }
        else
        {
            AudioManager.instance.musicEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            AudioManager.instance.InitializeMusic(FMODEvents.instance.CreditsMusic);
            demoEndScreen?.SetActive(true);
        }
    }

    private int GetNextLevelIndex(int levelIndex)
    {
        return levelIndex > SceneManager.sceneCountInBuildSettings - 1 ? 3 : levelIndex;
    }

    public async void RetryLevel()
    {
        AudioManager.instance.musicEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        await SceneLoader.LoadScene(SceneLoader.LoadingScreenName, GameplayData.LastLevelHash);
    }

    public async void BackToMainMenu()
    {
        AudioManager.instance.musicEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        await SceneLoader.LoadScene(SceneLoader.LoadingScreenName, SceneLoader.TitleScreenName);
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
