using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
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
        SceneManager.GetSceneByBuildIndex(SceneManager.sceneCountInBuildSettings + 1);
    }

    public void LoadPreviousScene()
    {
        SceneManager.GetSceneByBuildIndex(SceneManager.sceneCountInBuildSettings - 1);
    }
}
