using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreCalculator : MonoBehaviour
{
    /*private int _currentScore;
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
        scoreText.text = $"{_currentScore : 000000}";
    }

    //instantly adds score based on score value and rank multiplier, and updates score counter UI
    public void IncreaseScoreInstant(int scoreValueToAdd, float scoreMultiplier)
    {
        _currentScore += Mathf.RoundToInt(scoreValueToAdd * scoreMultiplier);
        scoreText.text = $"{_currentScore : 000000}";
    }

    //instantly reduces score based on scare value and updates score counter UI
    public void DecreaseScoreInstant(int scoreValueToRemove) 
    {
        if(_currentScore != 0) {
            _currentScore -= scoreValueToRemove;
            scoreText.text = $"{_currentScore: 000000}";
        }
    }

    //periodically adds score based on score value, rank multiplier, up to how long in seconds, and how many additions per second (defaults to 1 per second), and updates score counter UI
    public IEnumerator IncreaseScoreContinuousRoutine(int scoreValueToAdd, float scoreMultiplier, int maximumTime, float frequencyForAdding = 1f)
    {
        for (int i = 0; i < maximumTime; i++)
        {
            _currentScore += Mathf.RoundToInt(scoreValueToAdd * scoreMultiplier);
            scoreText.text = $"{_currentScore: 000000}";
            yield return new WaitForSeconds(1 / frequencyForAdding);
        }
    }
    
    public void AddExtraScoreBasedOnTime(float time)
    {
        _currentScore += time switch
        {
            < 3f * 60f => 10000,
            < 3.5f * 60f => 8000,
            < 4f * 60f => 6000,
            < 4.5f * 60f => 4000,
            < 5f * 60f => 2000,
            _ => 0
        };
    }*/

    [SerializeField] private IntEventChannel scoreEventChannel;
    
    private int _currentScore;
    public int CurrentScore { get => _currentScore; private set => _currentScore = Mathf.Clamp(value, 0, int.MaxValue); }

    private void OnEnable()
    {
        ResetScore();
    }

    //reset score to 0 then trigger the score event
    public void ResetScore()
    {
        _currentScore = 0;
        TriggerScoreEvent(0);
    }
    
    //add score instantly based on a score value and the player's current score multiplier, then trigger the score event
    public void IncreaseScoreInstant(int scoreValueToAdd, float scoreMultiplier)
    {
        var scoreToAdd = Mathf.RoundToInt(scoreValueToAdd * scoreMultiplier);
        TriggerScoreEvent(scoreToAdd);
    }
    
    //reduce score based on a score value then trigger the score event; prevents negative score value
    public void DecreaseScoreInstant(int scoreValueToRemove)
    {
        if (_currentScore != 0) {
            TriggerScoreEvent(-scoreValueToRemove);
        }
    }
    
    //add score periodically based on score value and multiplier, then trigger the score event at regular intervals for a certain amount of time
    public IEnumerator IncreaseScoreContinuousRoutine(int scoreValueToAdd, float scoreMultiplier, int maximumTime, float frequencyForAdding = 1f)
    {
        for (var i = 0; i < maximumTime; i++)
        {
            var scoreToAdd = Mathf.RoundToInt(scoreValueToAdd * scoreMultiplier);
            TriggerScoreEvent(scoreToAdd);
            yield return new WaitForSeconds(1 / frequencyForAdding);
        }
    }
    
    // Add extra score based on time elapsed in the level then trigger the score event
    public void AddExtraScoreBasedOnTime(float time)
    {
        var extraScore = time switch
        {
            < 3f * 60f => 10000,
            < 3.5f * 60f => 8000,
            < 4f * 60f => 6000,
            < 4.5f * 60f => 4000,
            < 5f * 60f => 2000,
            _ => 0
        };
        TriggerScoreEvent(extraScore);
    }
    
    private void TriggerScoreEvent(int scoreChange)
    {
        _currentScore += scoreChange;
        scoreEventChannel.Invoke(_currentScore); // Notify UI listeners about the updated score
    }
}
