using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreCalculator : MonoBehaviour
{
    private int _currentScore;
    public int CurrentScore { get => _currentScore; private set => _currentScore = value; }
    
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Start()
    {
        ResetScore();
    }

    //resets player score to zero
    public void ResetScore()
    {
        _currentScore = 0;
        scoreText.text = $"{_currentScore : 00000}";
    }

    //instantly adds score based on score value and rank multiplier, and updates score counter UI
    public void IncreaseScoreInstant(int scoreValueToAdd, float scoreMultiplier)
    {
        _currentScore += Mathf.RoundToInt(scoreValueToAdd * scoreMultiplier);
        scoreText.text = $"{_currentScore : 00000}";
    }

    //instantly reduces score based on scare value and updates score counter UI
    public void DecreaseScoreInstant(int scoreValueToRemove) 
    {
        if(_currentScore != 0) {
            _currentScore -= scoreValueToRemove;
            scoreText.text = $"{_currentScore: 00000}";
        }
    }

    //periodically adds score based on score value, rank multiplier, up to how long in seconds, and how many additions per second (defaults to 1 per second), and updates score counter UI
    public IEnumerator IncreaseScoreContinuousRoutine(int scoreValueToAdd, float scoreMultiplier, int maximumTime, float frequencyForAdding = 1f)
    {
        for (int i = 0; i < maximumTime; i++)
        {
            _currentScore += Mathf.RoundToInt(scoreValueToAdd * scoreMultiplier);
            scoreText.text = $"{_currentScore: 00000}";
            yield return new WaitForSeconds(1 / frequencyForAdding);
        }
    }
    
    //TODO: make a dedicated function that updates score counter UI just in case it needs extra flair like animation/effects
}
