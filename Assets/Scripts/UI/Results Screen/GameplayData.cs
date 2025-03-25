using UnityEngine;

public static class GameplayData
{
    public static int FinalScore;
    public static float FinalTime;
    public static string LastLevelHash;

    public static void RecordScore(int finalScore)
    {
        FinalScore = finalScore;
    }

    public static void RecordTime(float time)
    {
        FinalTime = time;
    }

    public static void RecordLevel(string levelName)
    {
        LastLevelHash = levelName;
        Debug.LogAssertion(LastLevelHash);
    }
    
    public static void Reset()
    {
        FinalScore = 0;
        FinalTime = 0;
    }
}
