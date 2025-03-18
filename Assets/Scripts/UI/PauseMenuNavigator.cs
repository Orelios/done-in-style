using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class PauseMenuNavigator : MonoBehaviour
{
    [FormerlySerializedAs("sections")]
    [Header("Pause Menu Sections")]
    [SerializeField] private List<GameObject> interfaceSections;
    
    [Header("Main Interface")]
    [SerializeField] private string mainInterfaceHash;
     [SerializeField] private GameObject mainDefaultSelected;   
    
    [Header("Settings Interface")]
    [SerializeField] private string settingsInterfaceHash;
    [SerializeField] private GameObject settingsDefaultSelected;        
    [SerializeField] private List<GameObject> settingsSections;
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

    private void OnEnable()
    {
        _eventSystem = EventSystem.current;
    }

    public void PauseGame()
    {
        foreach (var section in interfaceSections)
        {
            section.SetActive(false);
        }
        
        interfaceSections.FirstOrDefault(main => main.name == mainInterfaceHash)?.SetActive(true);
        interfaceSections.FirstOrDefault(main => main.name == mainInterfaceHash)?.transform.GetChild(0).gameObject.SetActive(true);
        
        _eventSystem.SetSelectedGameObject(mainDefaultSelected);
    }

    public void OpenSettingsDirectory()
    {
        foreach (var section in interfaceSections)
        {
            section.SetActive(false);
        }
        
        interfaceSections.FirstOrDefault(main => main.name == settingsInterfaceHash)?.SetActive(true);
        interfaceSections.FirstOrDefault(main => main.name == settingsInterfaceHash)?.transform.GetChild(0).gameObject.SetActive(true);
        
        _eventSystem.SetSelectedGameObject(settingsDefaultSelected);
    }

    public void OpenSettingsAudio()
    {
        foreach (var section in settingsSections)
        {
            section.SetActive(false);
        }
        
        settingsSections.FirstOrDefault(main => main.name == audioSettingsHash)?.SetActive(true);
        
        _eventSystem.SetSelectedGameObject(audioSettingsDefaultSelected);
    }
    
    public void OpenSettingsVideo()
    {
        
    }
    
    public void OpenSettingsControls()
    {
        
    }

    public void ResumeGame()
    {
        foreach (var section in interfaceSections)
        {
            section.SetActive(false);
        }

        foreach (var section in settingsSections)
        {
            section.SetActive(false);
        }
    }
}
