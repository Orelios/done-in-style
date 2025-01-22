using System;
using TMPro;
using UnityEngine;

public class TEMP_StylishCombo : MonoBehaviour
{
    [Header("Style Configs")]
    public StylishRank[] StylishRanks;
    [SerializeField] private float pointFalloffTimer = 2.0f;
    [SerializeField] private int maxStylishPoints;
    private StylishRank _currentRank;
    private StylishRank _highestRank;
    private StylishRank _lowestRank;
    private int _currentStylishPoints = 0;
    private float _timeBeforeRankFalloff = 0f;
    
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI currentRankText;
    [SerializeField] private TextMeshProUGUI currentPointsText;

    private void Start()
    {
        _lowestRank = StylishRanks[0];
        _currentRank = _lowestRank;
        currentRankText.text = _currentRank.RankName;
        currentPointsText.text = $"{_currentStylishPoints}";
    }

    private void Update()
    {
        HandleStylishRankTimer();
        
        if (Input.GetKeyDown(KeyCode.F))
        {
            _currentStylishPoints++;
            _currentStylishPoints = Mathf.Clamp(_currentStylishPoints++, 0, maxStylishPoints);
            _timeBeforeRankFalloff = 0f;
            currentPointsText.text = $"{_currentStylishPoints}";
            
            if (_currentStylishPoints == _currentRank.RequiredBreakthroughPoints)
            {
                IncreaseStylishRank();
            }
        }
    }

    private void HandleStylishRankTimer()
    {
        _timeBeforeRankFalloff += Time.deltaTime;

        if (_timeBeforeRankFalloff >= pointFalloffTimer)
        {
            _currentStylishPoints--;
            _currentStylishPoints = Mathf.Clamp(_currentStylishPoints, 0, maxStylishPoints);
            _timeBeforeRankFalloff = 0f;
            currentPointsText.text = $"{_currentStylishPoints}";

            if (_currentStylishPoints < _currentRank.RequiredBreakthroughPoints)
            {
                DecreaseStylishRank();
            }
        }
    }

    private void DecreaseStylishRank()
    {
        int rankIndex = Array.IndexOf(StylishRanks, _currentRank) - 1;
        _currentRank = StylishRanks[Mathf.Clamp(rankIndex, 0, StylishRanks.Length - 1)];
        currentRankText.text = _currentRank.RankName;
    }

    private void IncreaseStylishRank()
    {
        int rankIndex = Array.IndexOf(StylishRanks, _currentRank) + 1;
        _currentRank = StylishRanks[Mathf.Clamp(rankIndex, 0, StylishRanks.Length - 1)];
        currentRankText.text = _currentRank.RankName;
    }
}

[Serializable] public struct StylishRank
{
    public string RankName;
    public StylishRankType RankType;
    public int RequiredBreakthroughPoints;
}

public enum StylishRankType
{
    NoRank = 0,
    LRank = 1,
    CRank = 2,
    BRank = 3,
    ARank = 4,
    SRank = 5,
}
