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
    private Bus SFXBus;
    private Bus BGMusicBus;
    public static AudioManager instance { get; private set; }

    private void Awake()
    {
        if(instance != null) { Debug.LogError("found more than one audio manager"); }
        instance = this;

        MasterBus = RuntimeManager.GetBus("bus:/");
        SFXBus = RuntimeManager.GetBus("bus:/SFX");
        BGMusicBus = RuntimeManager.GetBus("bus:/Music");
    }

    private void Update()
    {
        MasterBus.setVolume(MasterVolume);
        SFXBus.setVolume(SFXVolume);
        BGMusicBus.setVolume(BGMusicVolume);
    }

    public void PlayOneShot(EventReference sound, Vector3 position )
    {
        RuntimeManager.PlayOneShot(sound, position);
    }

    public EventInstance CreateInstance(EventReference eventReference)
    {
        //Debug.Log("walkingAU");
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        return eventInstance;
    }


}
