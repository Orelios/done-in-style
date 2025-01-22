using UnityEngine;

[CreateAssetMenu(fileName = "NewDaredevilGear", menuName = "Scriptable Objects/Daredevil Gear")]
public class DaredevilGearSO : ScriptableObject
{
    [SerializeField] private EDaredevilGearType daredevilGearType;
    [SerializeField] private string daredevilGearName;
    [SerializeField, Range(0.1f, 2.0f)] private float movementSpeedMultiplier;
    [SerializeField, Range(0.1f, 2.0f)] private float jumpForceMultiplier;

    public EDaredevilGearType DaredevilGearType {get => daredevilGearType; private set => daredevilGearType = value; }
    public string DaredevilGearName {get => daredevilGearName; private set => daredevilGearName = value; }
    public float MovementSpeedMultiplier {get => movementSpeedMultiplier; private set => movementSpeedMultiplier = value; }
    public float JumpForceMultiplier {get => jumpForceMultiplier; private set => jumpForceMultiplier = value; }
    
    //TODO: icon, sprite, description
}

public enum EDaredevilGearType
{
    None,
    Skateboard,
    RollerBlades,
    PogoStick
}