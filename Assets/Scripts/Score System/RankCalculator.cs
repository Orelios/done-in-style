using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;

public class RankCalculator : MonoBehaviour
{
    [SerializeField] private IntEventChannel comboEventChannel;
    [SerializeField] private RankEventChannel rankEventChannel;
    
    [Header("Stylish Ranks Configs")]
    [SerializeField] private List<StylishRankSO> stylishRanksList = new();
    //railing speed multipliers based on rank
    public float[] railSpeedMults = new float[] { 1, 2, 3, 4, 5, 6 };
    [SerializeField] private StylishRankSO defaultStylishRank;
    public StylishRankSO CurrentStylishRank {get; private set;}
    [SerializeField] private int _currentStylishRankIndex;
    public int CurrentStylishRankIndex => _currentStylishRankIndex;

    [Header("Stylish Points Configs")] 
    [SerializeField] private int maxStylishPoints;
    [SerializeField] private TextMeshProUGUI stylishRankText;
    [SerializeField] private TextMeshProUGUI  stylishPointsText;
    public int CurrentStylishPoints { get; private set; }

    [Header(("Falloff Timer Configs"))]
    [SerializeField] private float pointsFalloffTime;
    private float _pointFalloffTimer;
    
    /*[Header("Display")]
    [SerializeField] private RankDisplayUpdater displayUpdater;*/

    private void Start()
    {
        defaultStylishRank = stylishRanksList[0];
        CurrentStylishRank = defaultStylishRank;
        _currentStylishRankIndex = stylishRanksList.IndexOf(defaultStylishRank);
        stylishRankText.text = $"{CurrentStylishRank.RankName}";
        CurrentStylishPoints = 0;
        stylishPointsText.text = $"{CurrentStylishPoints}";
        _pointFalloffTimer = pointsFalloffTime;
        
        //displayUpdater.UpdateRankDisplay(CurrentStylishRank);
        //displayUpdater.SetNewSliderLimits(0,  stylishRanksList[_currentStylishRankIndex].RequiredBreakthroughPoints);
        rankEventChannel.Invoke(CurrentStylishRank);
    }

    private void Update()
    {
        _pointFalloffTimer -= Time.deltaTime;

        if (_pointFalloffTimer <= 0)
        {
            DecreaseStylishPoints(1);
        }
    }

    //increments rank, clamps rank value between the lowest and highest rank index, and updates rank UI
    private  void IncreaseStylishRank()
    {
        _currentStylishRankIndex++;
        _currentStylishRankIndex = Mathf.Clamp(_currentStylishRankIndex, 0, stylishRanksList.Count - 1);
        CurrentStylishRank = stylishRanksList[_currentStylishRankIndex];
       // stylishRankText.text = $"{CurrentStylishRank.RankName}";
       
       /*displayUpdater.UpdateRankDisplay(CurrentStylishRank);
       displayUpdater.SetNewSliderLimits(stylishRanksList[_currentStylishRankIndex - 1].RequiredBreakthroughPoints,  stylishRanksList[_currentStylishRankIndex].RequiredBreakthroughPoints);*/
       rankEventChannel.Invoke(CurrentStylishRank);

        switch (_currentStylishRankIndex - 1)
        {
            case 1:
                AudioManager.instance.PlayOneShot(FMODEvents.instance.VOXCookin, this.transform.position);
                break;
            case 2:
                AudioManager.instance.PlayOneShot(FMODEvents.instance.VOXBallin, this.transform.position);
                break;
            case 3:
                AudioManager.instance.PlayOneShot(FMODEvents.instance.VOXAwesome, this.transform.position);
                break;
            case 4:
                AudioManager.instance.PlayOneShot(FMODEvents.instance.VOXStylish, this.transform.position);
                break;
        }
    }

    //decrements rank, clamps rank value between the lowest and highest rank index, and updates rank UI
    private  void DecreaseStylishRank()
    {
        _currentStylishRankIndex--;
        _currentStylishRankIndex = Mathf.Clamp(_currentStylishRankIndex, 0, stylishRanksList.Count - 1);
        CurrentStylishRank = stylishRanksList[_currentStylishRankIndex]; 
        //stylishRankText.text = $"{CurrentStylishRank.RankName}";
        
        /*displayUpdater.UpdateRankDisplay(CurrentStylishRank);
        displayUpdater.SetNewSliderLimits(stylishRanksList[Mathf.Min(_currentStylishRankIndex - 1, 0)].RequiredBreakthroughPoints,  stylishRanksList[_currentStylishRankIndex].RequiredBreakthroughPoints);*/
        rankEventChannel.Invoke(CurrentStylishRank);
    }

    //increments points, clamps points value between the lowest and highest points breakthrough, resets falloff timer, and updates points UI
    public void IncreaseStylishPoints()
    {
        CurrentStylishPoints++;
        CurrentStylishPoints= Mathf.Clamp(CurrentStylishPoints, 0, maxStylishPoints);
        _pointFalloffTimer = pointsFalloffTime;        
        //string is shown as the value of current stylish points and a new line with the text MAX with font size of 75 if current stylish points is max
        //stylishPointsText.text = $"{CurrentStylishPoints}" + $"<size=75>{(CurrentStylishPoints == maxStylishPoints ? "\nMAX" : "")}</size>";
        
        //displayUpdater.UpdateSliderValue(CurrentStylishPoints);
        comboEventChannel.Invoke(CurrentStylishPoints);
        
        if (CurrentStylishPoints == CurrentStylishRank.RequiredBreakthroughPoints)
        {
            IncreaseStylishRank();
        }
    }
    
    public IEnumerator IncreaseStylishPointsContinuousRoutine(int maximumTime, float frequencyForAdding = 1f)
    {
        for (int i = 0; i < maximumTime; i++)
        {
            CurrentStylishPoints++;
            CurrentStylishPoints= Mathf.Clamp(CurrentStylishPoints, 0, maxStylishPoints);
            _pointFalloffTimer = pointsFalloffTime;            
            //stylishPointsText.text = $"{CurrentStylishPoints}" + $"<size=75>{(CurrentStylishPoints == maxStylishPoints ? "\nMAX" : "")}</size>";

            //displayUpdater.UpdateSliderValue(CurrentStylishPoints);
            comboEventChannel.Invoke(CurrentStylishPoints);
            
            if (CurrentStylishPoints == CurrentStylishRank.RequiredBreakthroughPoints)
            {
                IncreaseStylishRank();
            }
            
            yield return new WaitForSeconds(1 / frequencyForAdding);
        }
    }

    //decrements points, clamps points value between the lowest and highest points breakthrough, resets falloff timer, and updates points UI
    public void DecreaseStylishPoints(int value)
    {
        CurrentStylishPoints-= value;
        CurrentStylishPoints= Mathf.Clamp(CurrentStylishPoints, 0, maxStylishPoints);
        _pointFalloffTimer = pointsFalloffTime;        
        //stylishPointsText.text = $"{CurrentStylishPoints}";

        //displayUpdater.UpdateSliderValue(CurrentStylishPoints);
        comboEventChannel.Invoke(CurrentStylishPoints);
        
        if (_currentStylishRankIndex > 0 && CurrentStylishPoints < stylishRanksList[_currentStylishRankIndex - 1].RequiredBreakthroughPoints)
        {
            DecreaseStylishRank();
        }
    }
}
