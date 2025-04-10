using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    public void DetectIfGameOver(int playerHealth)
    {
        if (playerHealth <= 0)
        {
            Debug.Log("Game Over");
        }
    }
}
