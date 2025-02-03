using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldGrabber : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI inputFieldName;
    [SerializeField] private string inputText;
    
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private string playerMovementVariableName;

    private float _inputValue;

    private void Awake()
    {
        inputFieldName.text = playerMovementVariableName;
    }

    private bool ValidateInput(string input)
    {
        if (float.TryParse(input, out float result) && result >= 0)
        {
            _inputValue = result;
            return true;
        }

        return false;
    }

    public void UpdateVariableWithInput(string input)
    {
        inputText = input;
        
        if (!ValidateInput(inputText))
        {
            Debug.LogWarning($"Invalid input. {inputText} is not a valid number.");
            return;
        }

        
    }
}
