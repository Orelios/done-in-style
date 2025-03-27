using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    private enum VolumeType
    {
        MASTER,
        MUSIC,
        AMBIENCE,
        SFX
    }

    [Header("Type")]
    [SerializeField] private VolumeType volumeType;

    private Slider volumeSlider;

    private void Awake()
    {
        volumeSlider = GetComponent<Slider>();
    }

    private void Start()
    {
        switch (volumeType)
        {
            case VolumeType.MASTER:
                if (PlayerPrefs.HasKey("Master"))
                {
                    AudioManager.instance.MasterVolume = PlayerPrefs.GetFloat("Master");
                }
                else
                {
                    AudioManager.instance.MasterVolume = 0.5f;
                }
                //AudioManager.instance.MasterVolume = 0.5f;
                volumeSlider.value = AudioManager.instance.MasterVolume;
                break;
            case VolumeType.MUSIC:
                if (PlayerPrefs.HasKey("BG"))
                {
                    AudioManager.instance.BGMusicVolume = PlayerPrefs.GetFloat("BG");
                }
                else
                {
                    AudioManager.instance.BGMusicVolume = 0.5f;
                }
                //AudioManager.instance.BGMusicVolume = 0.5f;
                volumeSlider.value = AudioManager.instance.BGMusicVolume;
                break;
            case VolumeType.SFX:
                if (PlayerPrefs.HasKey("SFX"))
                {
                    AudioManager.instance.SFXVolume = PlayerPrefs.GetFloat("SFX");
                }
                else
                {
                    AudioManager.instance.SFXVolume = 0.5f;
                }
                //AudioManager.instance.SFXVolume = 0.5f;
                volumeSlider.value = AudioManager.instance.SFXVolume;
                break;
            default:
                Debug.LogWarning("Volume Type not supported: " + volumeType);
                break;
        }
    }

    /*private void Update()
    {
        switch (volumeType)
        {
            case VolumeType.MASTER:
                volumeSlider.value = AudioManager.instance.MasterVolume;
                break;
            case VolumeType.MUSIC:
                volumeSlider.value = AudioManager.instance.BGMusicVolume;
                break;
            case VolumeType.SFX:
                volumeSlider.value = AudioManager.instance.SFXVolume;
                break;
            default:
                Debug.LogWarning("Volume Type not supported: " + volumeType);
                break;
        }
    }*/

    public void OnSliderValueChanged()
    {
        switch (volumeType)
        {
            case VolumeType.MASTER:
                AudioManager.instance.MasterVolume = volumeSlider.value;
                PlayerPrefs.SetFloat("Master", AudioManager.instance.MasterVolume);
                break;
            case VolumeType.MUSIC:
                AudioManager.instance.BGMusicVolume = volumeSlider.value;
                PlayerPrefs.SetFloat("BG", AudioManager.instance.BGMusicVolume);
                break;
            case VolumeType.SFX:
                AudioManager.instance.SFXVolume = volumeSlider.value;
                PlayerPrefs.SetFloat("SFX", AudioManager.instance.SFXVolume);
                break;
            default:
                Debug.LogWarning("Volume Type not supported: " + volumeType);
                break;
        }
    }
}
