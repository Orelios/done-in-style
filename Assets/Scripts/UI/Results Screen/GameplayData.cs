using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameplayData
{
    public static int FinalScore;
    public static float FinalTime;
    public static string LastLevelHash;
    public static int LastLevelIndex;

    public static void RecordScore(int finalScore)
    {
        FinalScore = finalScore;
        Debug.Log($"Final SCORE: {finalScore} in scene: {SceneManager.GetActiveScene().name}");
    }

    public static void RecordTime(float time)
    {
        FinalTime = time;
        Debug.Log($"Final TIME: {time} in scene: {SceneManager.GetActiveScene().name}");
    }

    public static void RecordLevel(string levelName, int levelIndex)
    {
        if (!levelName.Contains("Screen") || levelIndex > 2)
        {
            LastLevelHash = levelName;
            LastLevelIndex = levelIndex;
            Debug.LogWarning($"{LastLevelHash}, index {LastLevelIndex}");
        }
    }
    
    public static void Reset()
    {
        FinalScore = 0;
        FinalTime = 0;
        
        Debug.Log($"RESET SCORE AND TIME: {FinalScore}, {FinalTime}");
    }
}
