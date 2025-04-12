using UnityEngine;
using FMOD.Studio;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private GameObject gameOverScreen;
    
    public void DetectIfGameOver(int playerHealth)
    {
        if (playerHealth <= 0)
        {
            AudioManager.instance.InGameSFXBus.setMute(true);
            AudioManager.instance.PlayOneShotNoLocation(FMODEvents.instance.WastedActivate);
            AudioManager.instance.musicEventInstance.stop(STOP_MODE.ALLOWFADEOUT);
            AudioManager.instance.ambienceEventInstance.stop(STOP_MODE.ALLOWFADEOUT);
            AudioManager.instance.musicEventInstance.release();
            AudioManager.instance.ambienceEventInstance.release();
            
            gameOverScreen.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
