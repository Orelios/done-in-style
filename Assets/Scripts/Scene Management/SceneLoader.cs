using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public static async Task LoadScene(string loadingScreen, string sceneToLoad, float minimumLoadingTime = 2f)
    {
        var loadingScreenAsync = SceneManager.LoadSceneAsync(loadingScreen, LoadSceneMode.Additive);
        loadingScreenAsync.allowSceneActivation = true;

        while (!loadingScreenAsync.isDone)
        {
            await Task.Yield();
        }
        
        var sceneToUnloadAsync = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

        while (!sceneToUnloadAsync.isDone)
        {
            await Task.Yield();
        }
        
        var sceneToLoadAsync = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
        sceneToLoadAsync.allowSceneActivation = false;

        var timeElapsedInLoading = 0f;

        while (!sceneToLoadAsync.isDone)
        {
            timeElapsedInLoading += Time.unscaledDeltaTime;

            if (sceneToLoadAsync.progress >= 0.9f && timeElapsedInLoading >= minimumLoadingTime)
            {
                sceneToLoadAsync.allowSceneActivation = true;
            }

            await Task.Yield();
        }
        
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneToLoad));
        
        var unloadLoadingScreenAsync = SceneManager.UnloadSceneAsync(loadingScreen);

        while (!unloadLoadingScreenAsync.isDone)
        {
            await Task.Yield();
        }
    }
}
