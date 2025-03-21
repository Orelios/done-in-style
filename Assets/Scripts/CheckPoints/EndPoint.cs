using UnityEngine;

public class EndPoint : MonoBehaviour
{
    [SerializeField] private EndScreen endScreen;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Time.timeScale = 0;
        FindFirstObjectByType<ScoreCalculator>().IncreaseScoreOnLevelClear(FindFirstObjectByType<TimeHandler>().ElapsedTime);
        endScreen.Toggle(true);
        endScreen.EndScreenText("You finished the level!");
    }
}
