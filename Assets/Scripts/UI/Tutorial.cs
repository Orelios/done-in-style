using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tutorial : MonoBehaviour
{
    public List<GameObject> keybinds = new List<GameObject>();
    public List<TMP_Text> keybindTexts = new List<TMP_Text>();
    public List<string> tutorialTexts = new List<string>();
    //public List<GameObject> tutorial = new List<GameObject>();
    /*
    [SerializeField] private GameObject moveLeft;
    [SerializeField] private GameObject moveRight;
    [SerializeField] private GameObject jump;
    [SerializeField] private GameObject dash;
    [SerializeField] private GameObject groundPound;
    */
    [SerializeField] private TMP_Text leftText;
    [SerializeField] private TMP_Text rightText;
    [SerializeField] private TMP_Text jumpText;
    [SerializeField] private TMP_Text doubleJump1Text;
    [SerializeField] private TMP_Text doubleJump2Text;
    [SerializeField] private TMP_Text dashText;
    [SerializeField] private TMP_Text poundText;
    [SerializeField] private TMP_Text slideText;
    [SerializeField] private TMP_Text trickText;
    [SerializeField] private TMP_Text pauseText;
    void Start()
    {
        // Clear any existing data to avoid duplication if Start is called multiple times
        //tutorialTexts.Clear();
        foreach (TMP_Text keybind in keybindTexts)
        {
            string textComponent = keybind.text;
            tutorialTexts.Add(textComponent);
        }
        for (int i = 0; i < tutorialTexts.Count; i++)
        {
            if (tutorialTexts[i] == "Left")
            {
                tutorialTexts[i] = "←";
            }
            else if (tutorialTexts[i] == "Right")
            {
                tutorialTexts[i] = "→";
            }
            else if (tutorialTexts[i] == "Up")
            {
                tutorialTexts[i] = "↑";
            }
            else if (tutorialTexts[i] == "Down")
            {
                tutorialTexts[i] = "↓";
            }
        }
        
        if (leftText != null)
        {
            leftText.text = tutorialTexts[0];
        }
        if (rightText != null)
        {
            rightText.text = tutorialTexts[1];
        }
        if (jumpText != null)
        {
            jumpText.text = tutorialTexts[2];
        }
        if (doubleJump1Text != null)
        {
            doubleJump1Text.text = tutorialTexts[2];
        }
        if (doubleJump2Text != null)
        {
            doubleJump2Text.text = tutorialTexts[2];
        }
        if (dashText != null)
        {
            dashText.text = tutorialTexts[3];
        }
        if (poundText != null)
        {
            poundText.text = tutorialTexts[4];
        }
        if (slideText != null)
        {
            slideText.text = tutorialTexts[5];
        }
        if (trickText != null)
        {
            trickText.text = tutorialTexts[6];
        }
        if (pauseText != null)
        {
            pauseText.text = tutorialTexts[7];
        }
    }

    public void UpdateTutorialKeybinds()
    {
        tutorialTexts.Clear();
        foreach (TMP_Text keybind in keybindTexts)
        {
            string textComponent = keybind.text;
            tutorialTexts.Add(textComponent);
        }

        for (int i = 0; i < tutorialTexts.Count; i++)
        {
            if (tutorialTexts[i] == "Left")
            {
                tutorialTexts[i] = "←";
            }
            else if (tutorialTexts[i] == "Right")
            {
                tutorialTexts[i] = "→";
            }
            else if (tutorialTexts[i] == "Up")
            {
                tutorialTexts[i] = "↑";
            }
            else if (tutorialTexts[i] == "Down")
            {
                tutorialTexts[i] = "↓";
            }
        }
        
        if (leftText != null)
        {
            leftText.text = tutorialTexts[0];
        }
        if (rightText != null)
        {
            rightText.text = tutorialTexts[1];
        }
        if (jumpText != null)
        {
            jumpText.text = tutorialTexts[2];
        }
        if (doubleJump1Text != null)
        {
            doubleJump1Text.text = tutorialTexts[2];
        }
        if (doubleJump2Text != null)
        {
            doubleJump2Text.text = tutorialTexts[2];
        }
        if (dashText != null)
        {
            dashText.text = tutorialTexts[3];
        }
        if (poundText != null)
        {
            poundText.text = tutorialTexts[4];
        }
        if (slideText != null)
        {
            slideText.text = tutorialTexts[5];
        }
        if (trickText != null)
        {
            trickText.text = tutorialTexts[6];
        }
        if (pauseText != null)
        {
            pauseText.text = tutorialTexts[7];
        }
    }


}
