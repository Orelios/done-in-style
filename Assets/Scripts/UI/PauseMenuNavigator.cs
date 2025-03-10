using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenuNavigator : MonoBehaviour
{
    [SerializeField] private List<GameObject> sections;
    [SerializeField] private string mainInterfaceHash;
    [SerializeField] private string settingsInterfaceHash;
    [SerializeField] private string confirmationInterfaceHash;
    [SerializeField] private GameObject mainDefaultSelected;
    [SerializeField] private GameObject settingsDefaultSelected;
    [SerializeField] private GameObject confirmationDefaultSelected;
    private EventSystem _eventSystem;

    private void OnEnable()
    {
        _eventSystem = EventSystem.current;
    }

    public void PauseGame()
    {
        foreach (var section in sections)
        {
            section.SetActive(false);
        }
        
        sections.FirstOrDefault(main => main.name == mainInterfaceHash)?.SetActive(true);
        
        _eventSystem.SetSelectedGameObject(mainDefaultSelected);
    }

    public void ResumeGame()
    {
        foreach (var section in sections)
        {
            section.SetActive(false);
        }
    }
}
