using System;
using TMPro;
using UnityEngine;

public class PlayerGearSwapper : MonoBehaviour
{
    [Header("Gear Swapping Configs")]
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

    public void SwapGear(DaredevilGearSO gearToSwapTo)
    {
        //if not and cooldown and is not switching to the same Gear
        if (Time.time < lastSwapTime + swapCooldown && gearToSwapTo != _currentGearEquipped)
            return;
        
        lastSwapTime = Time.time;
        
        //swtiches current gear into a different gear, adsjuting speed and jump values accordingly
        _currentGearEquipped = gearToSwapTo;
        HorizontalMovementMultiplier = _currentGearEquipped.MovementSpeedMultiplier;
        JumpForceMultiplier = _currentGearEquipped.JumpForceMultiplier;
        
        //adjusts debug text
        debugText.text = $"{_currentGearEquipped.DaredevilGearName}";
    }
}
