using UnityEngine;

public class EndPoint : MonoBehaviour
{
    [SerializeField] private EndScreen endScreen;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            FindFirstObjectByType<GameStateHandler>().FinishLevel();
        }
    }
}
