using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class Tutorial : MonoBehaviour
{
    public InputActionAsset inputActions;
    public List<GameObject> keybinds = new List<GameObject>();
    //public List<TMP_Text> keybindTexts = new List<TMP_Text>();
    public List<string> tutorialTexts = new List<string>();
    //public List<GameObject> tutorial = new List<GameObject>();
    /*
    [SerializeField] private GameObject moveLeft;
    [SerializeField] private GameObject moveRight;
    [SerializeField] private GameObject jump;
    [SerializeField] private GameObject dash;
    [SerializeField] private GameObject groundPound;
    */
    [SerializeField] private TMP_Text empty;
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
        /* Clear any existing data to avoid duplication if Start is called multiple times
        //tutorialTexts.Clear();
        foreach (TMP_Text keybind in keybindTexts)
        {
            string textComponent = keybind.text;
            tutorialTexts.Add(textComponent);
        }
        */

        GetBinding("Move");
        GetBinding("Jump");
        GetBinding("Dash");
        GetBinding("GroundPound");
        GetBinding("Sliding");
        GetBinding("TrickMove");


        for (int i = 0; i < tutorialTexts.Count; i++)
        {
            if (tutorialTexts[i] == "Left Arrow" || tutorialTexts[i] == "Left")
            {
                tutorialTexts[i] = "←";
            }
            else if (tutorialTexts[i] == "Right Arrow" || tutorialTexts[i] == "Right")
            {
                tutorialTexts[i] = "→";
            }
            else if (tutorialTexts[i] == "Up Arrow" || tutorialTexts[i] == "Up")
            {
                tutorialTexts[i] = "↑";
            }
            else if (tutorialTexts[i] == "Down Arrow" || tutorialTexts[i] == "Down")
            {
                tutorialTexts[i] = "↓";
            }
            else if (tutorialTexts[i] == "Spacebar" || tutorialTexts[i] == "Space")
            {
                tutorialTexts[i] = "␣";
            }
        }

        if (leftText != null)
        {
            leftText.text = tutorialTexts[1];
        }
        if (rightText != null)
        {
            rightText.text = tutorialTexts[2];
        }
        if (jumpText != null)
        {
            jumpText.text = tutorialTexts[3];
        }
        if (doubleJump1Text != null)
        {
            doubleJump1Text.text = tutorialTexts[3];
        }
        if (doubleJump2Text != null)
        {
            doubleJump2Text.text = tutorialTexts[3];
        }
        if (dashText != null)
        {
            dashText.text = tutorialTexts[4];
        }
        if (poundText != null)
        {
            poundText.text = tutorialTexts[5];
        }
        if (slideText != null)
        {
            slideText.text = tutorialTexts[6];
        }
        if (trickText != null)
        {
            trickText.text = tutorialTexts[7];
        }
        /*
        if (pauseText != null)
        {
            pauseText.text = tutorialTexts[8];
        }
        */

    }

    public void UpdateTutorialKeybinds()
    {
        tutorialTexts.Clear();
        /*
        foreach (TMP_Text keybind in keybindTexts)
        {
            string textComponent = keybind.text;
            tutorialTexts.Add(textComponent);
        }
        */

        GetBinding("Move");
        GetBinding("Jump");
        GetBinding("Dash");
        GetBinding("GroundPound");
        GetBinding("Sliding");
        GetBinding("TrickMove");


        for (int i = 0; i < tutorialTexts.Count; i++)
        {
            if (tutorialTexts[i] == "Left Arrow" || tutorialTexts[i] == "Left")
            {
                tutorialTexts[i] = "←";
            }
            else if (tutorialTexts[i] == "Right Arrow" || tutorialTexts[i] == "Right")
            {
                tutorialTexts[i] = "→";
            }
            else if (tutorialTexts[i] == "Up Arrow" || tutorialTexts[i] == "Up")
            {
                tutorialTexts[i] = "↑";
            }
            else if (tutorialTexts[i] == "Down Arrow" || tutorialTexts[i] == "Down")
            {
                tutorialTexts[i] = "↓";
            }
            else if (tutorialTexts[i] == "Spacebar" || tutorialTexts[i] == "Space")
            {
                tutorialTexts[i] = "␣";
            }
        }

        if (leftText != null)
        {
            leftText.text = tutorialTexts[1];
        }
        if (rightText != null)
        {
            rightText.text = tutorialTexts[2];
        }
        if (jumpText != null)
        {
            jumpText.text = tutorialTexts[3];
        }
        if (doubleJump1Text != null)
        {
            doubleJump1Text.text = tutorialTexts[3];
        }
        if (doubleJump2Text != null)
        {
            doubleJump2Text.text = tutorialTexts[3];
        }
        if (dashText != null)
        {
            dashText.text = tutorialTexts[4];
        }
        if (poundText != null)
        {
            poundText.text = tutorialTexts[5];
        }
        if (slideText != null)
        {
            slideText.text = tutorialTexts[6];
        }
        if (trickText != null)
        {
            trickText.text = tutorialTexts[7];
        }
        /*
        if (pauseText != null)
        {
            pauseText.text = tutorialTexts[8];
        }
        */
    }

    private void GetBinding(string name)
    {
        InputAction action = inputActions.FindAction(name);
        if (action != null)
        {
            foreach (var binding in action.bindings)
            {
                //Debug.Log($"Binding Path: {binding.path}");

                // Get human-readable key (like "W", "Space", etc.)
                string readableKey = InputControlPath.ToHumanReadableString(
                    binding.effectivePath,
                    InputControlPath.HumanReadableStringOptions.OmitDevice
                );

                if (binding.isPartOfComposite)
                {
                    Debug.Log($"(Composite) Key: {readableKey}");
                    tutorialTexts.Add(readableKey);
                }
                else
                {
                    Debug.Log($"Key: {readableKey}");
                    tutorialTexts.Add(readableKey);
                }
            }
        }
        else
        {
            Debug.LogError("Action not found!");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            UpdateTutorialKeybinds();
        }
    }
}
