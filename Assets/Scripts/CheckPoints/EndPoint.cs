using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPoint : MonoBehaviour
{
    [SerializeField] private EndScreen endScreen;
    //TODO: save level scene as reference for loading next stage in static script
    private void OnTriggerEnter2D(Collider2D collision)
    {
        FindFirstObjectByType<ScoreCalculator>().IncreaseScoreOnLevelClear(FindFirstObjectByType<TimeHandler>().ElapsedTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
