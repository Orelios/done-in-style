using UnityEngine;
using FMOD.Studio;

public class SkateParkAudioTrigger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioManager.instance.InitializeMusic(FMODEvents.instance.SkateParkMusic1);
        AudioManager.instance.InitializeAmbience(FMODEvents.instance.SkateParkAmbience);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
