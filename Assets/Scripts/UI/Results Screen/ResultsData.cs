using UnityEngine;

public static class ResultsData
{
    public static int FinalScore;
    public static float FinalTime;

    public static void RecordScore(int finalScore)
    {
        FinalScore = finalScore;
    }

    public static void RecordTime(float time)
    {
        FinalTime = time;
    }
    
    public static void Reset()
    {
        FinalScore = 0;
        FinalTime = 0;
    }
}
