using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public static class SceneLoader
{
    public static string LoadingScreenHash = "LoadingScreen";
    public static string TitleScreenHash = "TitleScreen";
    public static string ResultsScreenHash = "ResultsScreen";
    
    public static async Task LoadScene(string loadingScreen, string sceneToLoad, float minimumLoadingTime = 2f)
    {
        var loadingScreenAsync = SceneManager.LoadSceneAsync(loadingScreen, LoadSceneMode.Additive);
        loadingScreenAsync.allowSceneActivation = true;

        while (!loadingScreenAsync.isDone)
        {
            await Task.Yield();
        }

        var videoPlayer = Object.FindFirstObjectByType<VideoPlayer>();
        videoPlayer.Stop();
        videoPlayer.time = 0f;
        videoPlayer.Play();
        
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

            if (!videoPlayer.isPlaying && sceneToLoadAsync.progress >= 0.9f && timeElapsedInLoading >= minimumLoadingTime)
            {
                sceneToLoadAsync.allowSceneActivation = true;
                GameplayData.RecordLevel(sceneToLoad, SceneManager.GetSceneByName(sceneToLoad).buildIndex);
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
