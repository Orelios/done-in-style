using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreCalculator : MonoBehaviour
{
    [SerializeField] private IntEventChannel scoreEventChannel;
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
        _currentScore += Mathf.RoundToInt(scoreValueToAdd * scoreMultiplier);
        scoreEventChannel.Invoke(_currentScore);
    }

    //instantly reduces score based on scare value and updates score counter UI
    public void DecreaseScoreInstant(int scoreValueToRemove) 
    {
        if(_currentScore != 0) {
            _currentScore -= scoreValueToRemove;
            scoreEventChannel.Invoke(_currentScore);
        }
    }

    //periodically adds score based on score value, rank multiplier, up to how long in seconds, and how many additions per second (defaults to 1 per second), and updates score counter UI
    public IEnumerator IncreaseScoreContinuousRoutine(int scoreValueToAdd, float scoreMultiplier, int maximumTime, float frequencyForAdding = 1f)
    {
        for (int i = 0; i < maximumTime; i++)
        {
            _currentScore += Mathf.RoundToInt(scoreValueToAdd * scoreMultiplier);
            scoreEventChannel.Invoke(_currentScore);
            
            yield return new WaitForSeconds(1 / frequencyForAdding);
        }
    }

    public void IncreaseScoreOnLevelClear(float timeElapsed)
    {
        _currentScore += timeElapsed switch
        {
            < 180f => 10000,
            < 210f => 8000,
            < 240f => 6000,
            < 300f => 4000,
            < 360f => 2000,
            _ => 0
        };
        
        GameplayData.RecordScore(_currentScore);
        GameplayData.RecordTime(timeElapsed);
        scoreEventChannel.Invoke(_currentScore);
    }
}
