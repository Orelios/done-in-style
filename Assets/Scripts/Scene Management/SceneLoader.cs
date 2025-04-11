using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public static class SceneLoader
{
    public static string LoadingScreenName = "LoadingScreen";
    public static string TitleScreenName = "TitleScreen";
    public static string ResultsScreenName = "ResultsScreen";
    
    public static async Task LoadScene(string loadingScreen, string sceneToLoad, bool showLoadingOperation = false, float minimumLoadingTime = 2f)
    {
        await ScreenFadeHandler.Instance.FadeOutAsync();
        
        if (showLoadingOperation)
        {
            var loadingScreenAsync = SceneManager.LoadSceneAsync(loadingScreen, LoadSceneMode.Additive);
            loadingScreenAsync.allowSceneActivation = true;
            
            while (!loadingScreenAsync.isDone)
            {
                await Task.Yield();
            }
            
            var videoPlayer = Object.FindFirstObjectByType<VideoPlayer>();
        
            videoPlayer.targetTexture.Release();
            videoPlayer.targetTexture.Create();
            videoPlayer.time = 0f;
            videoPlayer.Play();
            
            await ScreenFadeHandler.Instance.FadeInAsync();
            
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

                var isVideoFinished = videoPlayer == null || !videoPlayer.isPlaying;
            
                if (isVideoFinished && sceneToLoadAsync.progress >= 0.9f && timeElapsedInLoading >= minimumLoadingTime)
                {
                    sceneToLoadAsync.allowSceneActivation = true;
                }
                
                await Task.Yield();
            }
            
            await ScreenFadeHandler.Instance.FadeOutAsync();
            
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneToLoad));
            GameplayData.RecordLevel(sceneToLoad, SceneManager.GetSceneByName(sceneToLoad).buildIndex);

            var unloadLoadingScreenAsync = SceneManager.UnloadSceneAsync(loadingScreen);

            while (!unloadLoadingScreenAsync.isDone)
            {
                await Task.Yield();
            }
        }
        else
        {
            var loadingScreenAsync = SceneManager.LoadSceneAsync(loadingScreen, LoadSceneMode.Additive);
            loadingScreenAsync.allowSceneActivation = true;
            
            while (!loadingScreenAsync.isDone)
            {
                await Task.Yield();
            }
            
            var previousScene = SceneManager.GetActiveScene();
            var sceneToUnloadAsync = SceneManager.UnloadSceneAsync(previousScene);
            
            while (!sceneToUnloadAsync.isDone)
            {
                await Task.Yield();
            }
            
            var sceneToLoadAsync = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
            sceneToLoadAsync.allowSceneActivation = false;
            
            while (sceneToLoadAsync.progress < 0.9f)
            {
                await Task.Yield();
            }
            
            sceneToLoadAsync.allowSceneActivation = true;

            while (!sceneToLoadAsync.isDone)
            {
                await Task.Yield();
            }
            
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneToLoad));
            
            var unloadLoadingScreenAsync = SceneManager.UnloadSceneAsync(loadingScreen);
            
            while (!unloadLoadingScreenAsync.isDone)
            {
                await Task.Yield();
            }
            
            GameplayData.RecordLevel(sceneToLoad, SceneManager.GetSceneByName(sceneToLoad).buildIndex);
        }

        await ScreenFadeHandler.Instance.FadeInAsync();
    }
}
