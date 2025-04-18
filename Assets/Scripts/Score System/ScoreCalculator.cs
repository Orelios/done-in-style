using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreCalculator : MonoBehaviour
{
    [SerializeField] private IntEventChannel scoreEventChannel;
    [SerializeField] private IntEventChannel scorePopUpEventChannel;
    private int _currentScore;
    public int CurrentScore { get => _currentScore; private set => _currentScore = value; }

    private void OnEnable()
    {
        ResetScore();
        GameplayData.Reset();
    }

    //resets player score to zero
    public void ResetScore()
    {
        _currentScore = 0;
        scoreEventChannel.Invoke(_currentScore);
    }

    //instantly adds score based on score value and rank multiplier, and updates score counter UI
    public void IncreaseScoreInstant(int scoreValueToAdd, float scoreMultiplier)
    {
        var score = Mathf.RoundToInt(scoreValueToAdd * scoreMultiplier);
        _currentScore += score;
        
        scorePopUpEventChannel.Invoke(score);
        scoreEventChannel.Invoke(_currentScore);
    }

    //instantly reduces score based on scare value and updates score counter UI
    public void DecreaseScoreInstant(int scoreValueToRemove) 
    {
        if(_currentScore != 0) {
            _currentScore -= scoreValueToRemove;
            
            scorePopUpEventChannel.Invoke(-scoreValueToRemove);
            scoreEventChannel.Invoke(_currentScore);
        }
    }

    //periodically adds score based on score value, rank multiplier, up to how long in seconds, and how many additions per second (defaults to 1 per second), and updates score counter UI
    public IEnumerator IncreaseScoreContinuousRoutine(int scoreValueToAdd, float scoreMultiplier, int maximumTime, float frequencyForAdding = 1f)
    {
        for (int i = 0; i < maximumTime; i++)
        {
            var score = Mathf.RoundToInt(scoreValueToAdd * scoreMultiplier);
            _currentScore += score;
            
            scorePopUpEventChannel.Invoke(score);
            scoreEventChannel.Invoke(_currentScore);
            
            yield return new WaitForSeconds(1 / frequencyForAdding);
        }
    }

    public void IncreaseScoreOnLevelClear(float timeElapsed)
    {
        _currentScore += timeElapsed switch
        {
            < 60f => 10000,
            < 90f => 8000,
            < 120f => 6000,
            < 150f => 4000,
            < 180f => 2000,
            _ => 0
        };
        
        GameplayData.RecordScore(_currentScore);
        GameplayData.RecordTime(timeElapsed);
        scoreEventChannel.Invoke(_currentScore);
    }
}
