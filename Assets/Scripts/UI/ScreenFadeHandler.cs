using System;
using System.Threading.Tasks;
using UnityEngine;

public class ScreenFadeHandler : MonoBehaviour
{
    public static ScreenFadeHandler Instance;
    
    [SerializeField] private float fadeDuration;
    [SerializeField] private AnimationCurve fadeCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    [SerializeField] private bool fadeInOnSceneLoad = true;
    
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        transform.SetParent(null);
        
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private async void Start()
    {
        if (fadeInOnSceneLoad)
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 1f;
            
            await FadeInAsync();
        }
    }

    public async Task FadeInAsync()
    {
        _canvasGroup.alpha = 1f;
        await FadeAsync(1f, 0f);
    }
    
    public async Task FadeOutAsync()
    {
        _canvasGroup.alpha = 0f;
        await FadeAsync(0f, 1f);
    }

    private async Task FadeAsync(float startingAlpha, float targetAlpha)
    {
        var timeElapsed = 0f;
        startingAlpha = Mathf.Clamp(startingAlpha, 0f, 1f);
        targetAlpha = Mathf.Clamp(targetAlpha, 0f, 1f);
        
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.interactable = false;
        
        while (timeElapsed < fadeDuration)
        {
            timeElapsed += Time.unscaledDeltaTime;
            
            var normalizedTime = Mathf.Clamp01(timeElapsed / fadeDuration);
            var curve = fadeCurve.Evaluate(normalizedTime);
            _canvasGroup.alpha = Mathf.Lerp(startingAlpha, targetAlpha, curve);
            
            await Task.Yield();
        }
        
        _canvasGroup.alpha = targetAlpha;

        if (Mathf.Approximately(targetAlpha, 0f))
        {
            _canvasGroup.blocksRaycasts = false;
        }
    }
}
