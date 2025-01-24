using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Temp_RankCalculator : MonoBehaviour
{
    [Header("Stylish Ranks Configs")]
    [SerializeField] private List<StylishRankSO> stylishRanksList = new();
    [SerializeField] private StylishRankSO defaultStylishRank;
    public StylishRankSO CurrentStylishRank {get; private set;}
    private int _currentStylishRankIndex;

    [Header("Stylish Points Configs")]
    public int CurrentStylishPoints { get; private set; }
    [SerializeField] private TextMeshProUGUI stylishRankText;
    [SerializeField] private TextMeshProUGUI  stylishPointsText;

    [Header(("Falloff Timer Configs"))]
    [SerializeField] private float pointsFalloffTime;
    private float _pointFalloffTimer;

    private void Start()
    {
        defaultStylishRank = stylishRanksList[0];
        CurrentStylishRank = defaultStylishRank;
        _currentStylishRankIndex = stylishRanksList.IndexOf(defaultStylishRank);
        stylishRankText.text = $"{CurrentStylishRank.RankName}";
        CurrentStylishPoints = 0;
        stylishPointsText.text = $"{CurrentStylishPoints}";
        _pointFalloffTimer = pointsFalloffTime;
    }

    private void Update()
    {
        _pointFalloffTimer -= Time.deltaTime;

        if (_pointFalloffTimer <= 0)
        {
            DecreaseStylishPoints();
        }
    }

    //increments rank, clamps rank value between the lowest and highest rank index, and updates rank UI
    private  void IncreaseStylishRank()
    {
        _currentStylishRankIndex++;
        _currentStylishRankIndex = Mathf.Clamp(_currentStylishRankIndex, 0, stylishRanksList.Count - 1);
        CurrentStylishRank = stylishRanksList[_currentStylishRankIndex];
        stylishRankText.text = $"{CurrentStylishRank.RankName}";
    }

    //decrements rank, clamps rank value between the lowest and highest rank index, and updates rank UI
    private  void DecreaseStylishRank()
    {
        _currentStylishRankIndex--;
        _currentStylishRankIndex = Mathf.Clamp(_currentStylishRankIndex, 0, stylishRanksList.Count - 1);
        CurrentStylishRank = stylishRanksList[_currentStylishRankIndex]; 
        stylishRankText.text = $"{CurrentStylishRank.RankName}";
    }

    //increments points, clamps points value between the lowest and highest points breakthrough, resets falloff timer, and updates points UI
    public void IncreaseStylishPoints()
    {
        //CurrentStylishPoints = Mathf.Clamp(CurrentStylishPoints++, 0, stylishRanksList[^1].RequiredBreakthroughPoints + 1);
        CurrentStylishPoints++;
        CurrentStylishPoints= Mathf.Clamp(CurrentStylishPoints, 0, stylishRanksList[^2].RequiredBreakthroughPoints + 1);
        stylishPointsText.text = $"{CurrentStylishPoints}";
        _pointFalloffTimer = pointsFalloffTime;
        
        if (CurrentStylishPoints > CurrentStylishRank.RequiredBreakthroughPoints)
        {
            IncreaseStylishRank();
        }
    }

    //decrements points, clamps points value between the lowest and highest points breakthrough, resets falloff timer, and updates points UI
    public void DecreaseStylishPoints()
    {
        //CurrentStylishPoints = Mathf.Clamp(CurrentStylishPoints--, 0, stylishRanksList[^1].RequiredBreakthroughPoints + 1);
        CurrentStylishPoints--;
        CurrentStylishPoints= Mathf.Clamp(CurrentStylishPoints, 0, stylishRanksList[^2].RequiredBreakthroughPoints + 1);
        stylishPointsText.text = $"{CurrentStylishPoints}";
        _pointFalloffTimer = pointsFalloffTime;
        
        if (_currentStylishRankIndex > 0 && CurrentStylishPoints < stylishRanksList[_currentStylishRankIndex - 1].RequiredBreakthroughPoints)
        {
            DecreaseStylishRank();
        }
    }
}
