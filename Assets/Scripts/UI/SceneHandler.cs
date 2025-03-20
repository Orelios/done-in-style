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

    public void QuitApllication()
    {
        Application.Quit();
    }

    public void LoadNextScene()
    {
        //SceneManager.GetSceneByBuildIndex(SceneManager.sceneCountInBuildSettings + 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadPreviousScene()
    {
        //SceneManager.GetSceneByBuildIndex(SceneManager.sceneCountInBuildSettings - 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    /*private void OnEnable()
    {
        SceneManager.sceneLoaded += OnEnableScene;
    }

    public void OnEnableScene(Scene scene, LoadSceneMode mode)
    {
        _gameStateHandler = FindFirstObjectByType<GameStateHandler>();
        _player = FindFirstObjectByType<Player>();
        _pauseMenuNavigator = FindFirstObjectByType<PauseMenuNavigator>();
        _gameStateHandler.AssignValues(_player, _pauseMenuNavigator);
        Debug.Log("OnEnableScene called");
    }*/
}
