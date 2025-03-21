using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPoint : MonoBehaviour
{
    [SerializeField] private EndScreen endScreen;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        FindFirstObjectByType<ScoreCalculator>().IncreaseScoreOnLevelClear(FindFirstObjectByType<TimeHandler>().ElapsedTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
