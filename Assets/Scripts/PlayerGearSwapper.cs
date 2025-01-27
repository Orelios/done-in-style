using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGearSwapper : MonoBehaviour
{
    [Header("Gear Swapping Configs")] 
    [SerializeField] private List<DaredevilGearSO> daredevilGears = new();
    [SerializeField] private DaredevilGearSO defaultGearToEquip;
    private DaredevilGearSO _currentGearEquipped;
    public DaredevilGearSO CurrentGearEquipped => _currentGearEquipped;
    
    [SerializeField] private float swapCooldown;
    private float lastSwapTime;
    
    public float HorizontalMovementMultiplier { get; private set; }
    public float JumpForceMultiplier { get; private set; }

    [Header("Debug Configs")]
    [SerializeField] private GameObject debugAnchor;
    [SerializeField] private TextMeshProUGUI debugText;
    [SerializeField] private bool showDebugText;
    
    void Start()
    {
        _currentGearEquipped = defaultGearToEquip;
        HorizontalMovementMultiplier = _currentGearEquipped.MovementSpeedMultiplier;
        JumpForceMultiplier = _currentGearEquipped.JumpForceMultiplier;
        debugText.text = $"{_currentGearEquipped.DaredevilGearName}";
    }

    private void Update()
    {
        //constantly checks if debug bool value's active; toggles debug text accordingly
        debugAnchor.SetActive(showDebugText);
    }

    //TODO: optimize to not be dependent on being hard coded
    public void SwapGear(InputAction.CallbackContext context)
    {
        //determines which gear to equip based on which gear swap key is pressed
        switch (context.control.name)
        {
            case "a":
                _currentGearEquipped = daredevilGears[daredevilGears.FindIndex(gear => gear.DaredevilGearType == EDaredevilGearType.Skateboard)];
                break;
            case "s":
                _currentGearEquipped = daredevilGears[daredevilGears.FindIndex(gear => gear.DaredevilGearType == EDaredevilGearType.RollerBlades)];
                break;
            case "d":
                _currentGearEquipped = daredevilGears[daredevilGears.FindIndex(gear => gear.DaredevilGearType == EDaredevilGearType.PogoStick)];
                break;
            default:
                Debug.LogWarning($"Mismatch! Control name {context.control.name} was not recognized");
                break;
        }
        
        //adjusts movement and jump multipliers based on last swapped gear
        HorizontalMovementMultiplier = _currentGearEquipped.MovementSpeedMultiplier;
        JumpForceMultiplier = _currentGearEquipped.JumpForceMultiplier;
        
        //adjusts debug text
        debugText.text = $"{_currentGearEquipped.DaredevilGearName}";
    }
}
