using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    private GameStateHandler _gameStateHandler;
    private Player _player;
    private PauseMenuNavigator _pauseMenuNavigator;

    public void RestartScene() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    public void ReturnToMainMenu()
    {
        GameplayData.Reset();
        SceneManager.LoadScene("TitleScreen");
    }

    public void QuitApllication()
    {
        Application.Quit();
    }

    public void StartGameplay()
    {
        SceneManager.LoadScene(2);
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadPreviousScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
