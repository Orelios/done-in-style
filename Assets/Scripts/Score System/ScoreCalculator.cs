using System;
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
        _currentScore = 0;
        scoreText.text = $"{_currentScore : 00000}";
    }

    //adds scores based on score value and rank multiplier and updates score counter UI
    public void AddScore(int scoreValueToAdd, float scoreMultiplier)
    {
        _currentScore += Mathf.RoundToInt(scoreValueToAdd * scoreMultiplier);
        scoreText.text = $"{_currentScore : 00000}";
    }

    public void DecreaseScore(int scoreValueToRemove) 
    {
        if(_currentScore != 0) {
            _currentScore -= scoreValueToRemove;
            scoreText.text = $"{_currentScore: 00000}";
        }
    }
}
