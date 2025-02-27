using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    private void Awake()
    {
        if(instance != null) { Debug.LogError("found more than one audio manager"); }
        instance = this;
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
