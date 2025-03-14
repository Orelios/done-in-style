using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenuNavigator : MonoBehaviour
{
    [Header("Pause Menu Sections")]
    [SerializeField] private List<GameObject> interfaceSections;
    
    [Header("Main Interface")]
    [SerializeField] private string mainInterfaceHash;
     [SerializeField] private GameObject mainDefaultSelected;   
    
    [Header("Settings Interface")]
    [SerializeField] private string settingsInterfaceHash;
    [SerializeField] private List<GameObject> settingsSections;
    [SerializeField] private string settingsDirectoryHash; 
    [SerializeField] private GameObject settingsDirectoryDefaultSelected;      
    [SerializeField] private string audioSettingsHash;
    [SerializeField] private GameObject audioSettingsDefaultSelected;  
    [SerializeField] private string videoSettingsHash;
    [SerializeField] private GameObject videoSettingsDefaultSelected;  
    [SerializeField] private string controlsSettingsHash;
    [SerializeField] private GameObject controlsSettingsDefaultSelected;  
    
    [Header("Confirmation Interface")]
    [SerializeField] private string confirmationInterfaceHash;
    [SerializeField] private GameObject confirmationDefaultSelected;
    
    private EventSystem _eventSystem; 
    private EMenuLookingAt _currentlyLookingAt;

    private void OnEnable()
    {
        _eventSystem = EventSystem.current;
    }

    public void OpenMainInterface()
    {
        foreach (var section in interfaceSections)
        {
            section.SetActive(false);
        }
        
        interfaceSections.FirstOrDefault(main => main.name == mainInterfaceHash)?.SetActive(true);
        interfaceSections.FirstOrDefault(main => main.name == mainInterfaceHash)?.transform.GetChild(0).gameObject.SetActive(true);
        
        _eventSystem.SetSelectedGameObject(mainDefaultSelected);
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
        
        _eventSystem.SetSelectedGameObject(settingsDirectoryDefaultSelected);
        _currentlyLookingAt = EMenuLookingAt.SettingsDirectory;
    }

    public void OpenSettingsAudio()
    {
        foreach (var section in settingsSections)
        {
            section.SetActive(false);
        }
        
        settingsSections.FirstOrDefault(audioSettings => audioSettings.name == audioSettingsHash)?.SetActive(true);
        
        _eventSystem.SetSelectedGameObject(audioSettingsDefaultSelected);
        _currentlyLookingAt = EMenuLookingAt.AudioSettings;
    }
    
    public void OpenSettingsVideo()
    {
        foreach (var section in settingsSections)
        {
            section.SetActive(false);
        }
        
        settingsSections.FirstOrDefault(videoSettings => videoSettings.name == videoSettingsHash)?.SetActive(true);
        
        _eventSystem.SetSelectedGameObject(videoSettingsDefaultSelected);
        _currentlyLookingAt = EMenuLookingAt.VideoSettings;
    }
    
    public void OpenSettingsControls()
    {
        foreach (var section in settingsSections)
        {
            section.SetActive(false);
        }
        
        settingsSections.FirstOrDefault(controlsSettings => controlsSettings.name == controlsSettingsHash)?.SetActive(true);
        
        _eventSystem.SetSelectedGameObject(controlsSettingsDefaultSelected);
        _currentlyLookingAt = EMenuLookingAt.ControlsSettings;
    }

    public void SelectKey()
    {
        _currentlyLookingAt = EMenuLookingAt.SelectedKey;
    }

    public void Back()
    {
        switch (_currentlyLookingAt)
        {
            case EMenuLookingAt.MainInterface:
                CloseAllInterfaces();
                FindFirstObjectByType<GameStateHandler>().ResumeGame();
                break;
            case EMenuLookingAt.SettingsDirectory:
                OpenMainInterface();
                break;
            case EMenuLookingAt.AudioSettings:
            case EMenuLookingAt.VideoSettings:
            case EMenuLookingAt.ControlsSettings:
                OpenSettingsDirectory();
                break;
        }
    }

    public void CloseAllInterfaces()
    {
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
    SelectedAudio,
    VideoSettings,
    ControlsSettings,
    SelectedKey
}
