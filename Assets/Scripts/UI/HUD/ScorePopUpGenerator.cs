using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ScorePopUpGenerator : MonoBehaviour
{
    [Header("Object Pool Configs")]
    [SerializeField] private ScorePopUp scorePopUpPrefab;
    [SerializeField] private int defaultPoolSize;
    [SerializeField] private int maxPoolSize;
    
    [Header("Pop Up Configs")]
    [SerializeField] private Color gainScoreColor;
    [SerializeField] private Color loseScoreColor;
    [SerializeField] private AnimationCurve popUpFadeCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);
    [SerializeField] private float popUpDuration;
    [SerializeField] private float popUpMoveSpeed;
    
    private ObjectPool<ScorePopUp> _scorePopUpPool;

    private void Awake()
    {
        _scorePopUpPool = new(GeneratePopUp, OnGetScorePopUp, OnReleaseScorePopUp, OnDestroyScorePopUp, false, defaultPoolSize, maxPoolSize);

        PreGeneratePopUps();
    }

    #region Object Pool Initialization
    private ScorePopUp GeneratePopUp()
    {
        var instance = Instantiate(scorePopUpPrefab, transform);
        instance.SetPool(_scorePopUpPool);
        return instance;
    }

    private void OnGetScorePopUp(ScorePopUp popUp)
    {
        popUp.gameObject.SetActive(true);
    }
    
    private void OnReleaseScorePopUp(ScorePopUp popUp)
    {
        popUp.gameObject.SetActive(false);
    }
    
    private void OnDestroyScorePopUp(ScorePopUp popUp)
    {
        Destroy(popUp.gameObject);
    }

    public ScorePopUp GetScorePopUp()
    {
        return _scorePopUpPool.Get();
    }

    public void ReturnScorePopUp(ScorePopUp popUp)
    {
        _scorePopUpPool.Release(popUp);
    }
    #endregion

    private void PreGeneratePopUps()
    {
        var popUpList = new List<ScorePopUp>();
        
        for (int i = 0; i < defaultPoolSize; i++)
        {
            var popUp = _scorePopUpPool.Get();
            popUpList.Add(popUp);
        }

        foreach (var popUp in popUpList)
        {
            _scorePopUpPool.Release(popUp);
        }
    }
    
    public void ShowScorePopUp(int score)
    {
        var popUp = GetScorePopUp();
        var popUpColor = score >= 0 ? gainScoreColor : loseScoreColor;
        
        popUp.Show(popUpColor, score, popUpFadeCurve, popUpDuration, popUpMoveSpeed);
    }
}
