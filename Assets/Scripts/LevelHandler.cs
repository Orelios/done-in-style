using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelHandler : MonoBehaviour
{
    private ScoreCalculator _scoreCalculator;
    private RankCalculator _rankCalculator;
    private TimeHandler _timeHandler;
    private ResultScreen _resultScreen;

    private bool _isLevelRunning;
    private bool _isLevelFinished;

    private void Start()
    {
        _scoreCalculator = FindFirstObjectByType<ScoreCalculator>();
        _rankCalculator = FindFirstObjectByType<RankCalculator>();
        _timeHandler = FindFirstObjectByType<TimeHandler>();
        _resultScreen = FindFirstObjectByType<ResultScreen>();
    }

    public void StartLevel()
    {
        _isLevelRunning = true;
        _isLevelFinished = false;
        _scoreCalculator.ResetScore();
        _timeHandler.StartTimer();
    }
    
    public void EndLevel()
    {
        _isLevelRunning = false;
        _isLevelFinished = true;
        _timeHandler.EndTimer();
        _scoreCalculator.AddExtraScoreBasedOnTime(_timeHandler.ElapsedTimeInLevel);
        _resultScreen.ShowResultScreen(_scoreCalculator.CurrentScore, _timeHandler.ElapsedTimeInLevel, _rankCalculator.HighestStylishPoints);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
