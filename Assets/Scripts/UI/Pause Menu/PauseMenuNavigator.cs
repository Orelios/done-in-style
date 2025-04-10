using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PauseMenuNavigator : MonoBehaviour
{
    [Header("Background Elements")] 
    [SerializeField] private List<GameObject> backgroundElements;
    [Header("Pause Menu Sections")]
    [SerializeField] private List<GameObject> interfaceSections;
    
    [Header("Overlay Switcher")]
    [SerializeField] private OverlaySwitcher overlaySwitcher;
    
    [Header("Main Interface")]
    [SerializeField] private string mainInterfaceHash;
    
    [Header("Settings Interface")]
    [SerializeField] private string settingsInterfaceHash;
    [SerializeField] private List<GameObject> settingsSections;
    [SerializeField] private string settingsDirectoryHash;
    [SerializeField] private string settingsConfigurationHash;
    [SerializeField] private string audioSettingsHash;
    [SerializeField] private string videoSettingsHash;
    [SerializeField] private string controlsSettingsHash;
    
    [Header("Confirmation Interface")]
    [SerializeField] private string confirmationInterfaceHash;
    
    private EMenuLookingAt _currentlyLookingAt;

    public void OpenPauseMenu()
    {
        foreach (var element in backgroundElements)
        {
            element.SetActive(true);
        }
            
        foreach (var section in interfaceSections)
        {
            section.SetActive(false);
        }
        
        interfaceSections.FirstOrDefault(main => main.name == mainInterfaceHash)?.SetActive(true);
        interfaceSections.FirstOrDefault(main => main.name == mainInterfaceHash)?.transform.GetChild(0).gameObject.SetActive(true);
        
        overlaySwitcher.EnableMainOverlay();
        
        _currentlyLookingAt = EMenuLookingAt.MainInterface;
    }

    public void OpenSettingsDirectory()
    {
        foreach (var section in interfaceSections)
        {
            section.SetActive(false);
        }

        foreach (var section in settingsSections)
        {
            section.SetActive(false);
        }
        
        interfaceSections.FirstOrDefault(settings => settings.name == settingsInterfaceHash)?.SetActive(true);
        settingsSections.FirstOrDefault(directory => directory.name == settingsDirectoryHash)?.SetActive(true);
        
        _currentlyLookingAt = EMenuLookingAt.SettingsDirectory;
    }

    public void OpenSettingsAudio()
    {
        foreach (var section in settingsSections)
        {
            section.SetActive(false);
        }
        
        settingsSections.FirstOrDefault(directory => directory.name == settingsDirectoryHash)?.SetActive(true);
        settingsSections.FirstOrDefault(configuration => configuration.name == settingsConfigurationHash)?.SetActive(true);
        settingsSections.FirstOrDefault(audioSettings => audioSettings.name == audioSettingsHash)?.SetActive(true);
        
        _currentlyLookingAt = EMenuLookingAt.AudioSettings;
    }
    
    public void OpenSettingsVideo()
    {
        foreach (var section in settingsSections)
        {
            section.SetActive(false);
        }
        
        settingsSections.FirstOrDefault(directory => directory.name == settingsDirectoryHash)?.SetActive(true);
        settingsSections.FirstOrDefault(configuration => configuration.name == settingsConfigurationHash)?.SetActive(true);
        settingsSections.FirstOrDefault(videoSettings => videoSettings.name == videoSettingsHash)?.SetActive(true);
        
        _currentlyLookingAt = EMenuLookingAt.VideoSettings;
    }
    
    public void OpenSettingsControls()
    {
        foreach (var section in settingsSections)
        {
            section.SetActive(false);
        }
        
        settingsSections.FirstOrDefault(directory => directory.name == settingsDirectoryHash)?.SetActive(true);
        settingsSections.FirstOrDefault(configuration => configuration.name == settingsConfigurationHash)?.SetActive(true);
        settingsSections.FirstOrDefault(controlsSettings => controlsSettings.name == controlsSettingsHash)?.SetActive(true);
        
        _currentlyLookingAt = EMenuLookingAt.ControlsSettings;
    }

    public void OpenConfirmation()
    {
        foreach (var section in interfaceSections)
        {
            section.SetActive(false);   
        }
        
        interfaceSections.FirstOrDefault(confirmation => confirmation.name == confirmationInterfaceHash)?.SetActive(true);
        
        _currentlyLookingAt = EMenuLookingAt.Confirmation;
    }

    public void Back()
    {
        switch (_currentlyLookingAt)
        {
            case EMenuLookingAt.MainInterface:
                ClosePauseMenu();
                GameStateHandler.Instance.ResumeGame();
                break;
            case EMenuLookingAt.SettingsDirectory:
            case EMenuLookingAt.Confirmation:
            case EMenuLookingAt.AudioSettings:
            case EMenuLookingAt.VideoSettings:
            case EMenuLookingAt.ControlsSettings:
                OpenPauseMenu();
                break;
        }
    }

    private void ClosePauseMenu()
    {
        foreach (var element in backgroundElements)
        {
            element.SetActive(false);
        }
        
        foreach (var section in interfaceSections)
        {
            section.SetActive(false);
        }

        foreach (var section in settingsSections)
        {
            section.SetActive(false);
        }
        
        _currentlyLookingAt = EMenuLookingAt.None;
    }
}

public enum EMenuLookingAt
{
    None,
    MainInterface,
    SettingsDirectory,
    AudioSettings,
    VideoSettings,
    ControlsSettings,
    Confirmation
}
