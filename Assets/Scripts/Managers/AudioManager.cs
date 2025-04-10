using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    [Header("Volume")]
    [Range(0, 1)]public float MasterVolume = 1;
    [Range(0, 1)] public float SFXVolume = 1;
    [Range(0, 1)] public float BGMusicVolume = 1;

    private Bus MasterBus;
    public Bus SFXBus;
    public Bus BGMusicBus;
    public Bus InGameSFXBus; 

    public EventInstance musicEventInstance;
    public EventInstance ambienceEventInstance;
    public EventInstance PlayerRailing; 
    public static AudioManager instance { get; private set; }

    private void Awake()
    {
        if(instance != null) { Debug.LogError("found more than one audio manager"); }
        instance = this;

        MasterBus = RuntimeManager.GetBus("bus:/");
        SFXBus = RuntimeManager.GetBus("bus:/SFX");
        BGMusicBus = RuntimeManager.GetBus("bus:/Music");
        InGameSFXBus = RuntimeManager.GetBus("bus:/SFX/InGameSFX");

        //On Awake, load PlayerPrefs
        MasterVolume = PlayerPrefs.GetFloat("Master");
        BGMusicVolume = PlayerPrefs.GetFloat("BG");
        instance.SFXVolume = PlayerPrefs.GetFloat("SFX");
    }

    private void Update()
    {
        MasterBus.setVolume(MasterVolume);
        SFXBus.setVolume(SFXVolume);
        BGMusicBus.setVolume(BGMusicVolume);
    }

    public void InitializeAmbience(EventReference ambienceEventReference)
    {
        ambienceEventInstance = CreateInstance(ambienceEventReference);
        ambienceEventInstance.start();
    }

    public void InitializeMusic(EventReference musicEventReference)
    {
        musicEventInstance = CreateInstance(musicEventReference);
        musicEventInstance.start();
    }

    public void PlayOneShot(EventReference sound, Vector3 position )
    {
        RuntimeManager.PlayOneShot(sound, position);
    }

    public void PlayOneShotNoLocation(EventReference sound)
    {
        RuntimeManager.PlayOneShot(sound);
    }

    public EventInstance CreateInstance(EventReference eventReference)
    {
        //Debug.Log("walkingAU");
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        return eventInstance;
    }


}
