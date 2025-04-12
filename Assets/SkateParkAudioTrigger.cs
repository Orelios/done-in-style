using UnityEngine;
using FMOD.Studio;

public class SkateParkAudioTrigger : MonoBehaviour
{
    [SerializeField] private bool activateInGameMusic = true;
    [SerializeField] private bool activateTitleScreenMusic = false; 
    void Start()
    {
        if (activateInGameMusic)
        {
            AudioManager.instance.InitializeMusic(FMODEvents.instance.SkateParkMusic1);
            AudioManager.instance.InitializeAmbience(FMODEvents.instance.SkateParkAmbience);
        }

        if (activateTitleScreenMusic)
        {
            AudioManager.instance.InitializeMusic(FMODEvents.instance.MainMenuMusic);
        }
        
    }

    public void ActivateCreditsMusic()
    {
        AudioManager.instance.musicEventInstance.stop(STOP_MODE.ALLOWFADEOUT);
        AudioManager.instance.InitializeMusic(FMODEvents.instance.CreditsMusic);
    }
        
    public void ActivateMainMenuMusic()
    {
        AudioManager.instance.musicEventInstance.stop(STOP_MODE.ALLOWFADEOUT);
        AudioManager.instance.InitializeMusic(FMODEvents.instance.MainMenuMusic);
    }
}
