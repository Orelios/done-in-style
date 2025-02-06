using UnityEngine;

public class Gear : MonoBehaviour, IGear
{
    protected DaredevilGearSO GearConfig;
    DaredevilGearSO IGear.GearConfig => GearConfig;
    protected New_PlayerMovement PlayerMovement; 
    protected Gear(New_PlayerMovement playerMovement, DaredevilGearSO gearSo)
    {
        PlayerMovement = playerMovement;
        GearConfig = gearSo;
    }

    public virtual void ApplyMovementModifiers() { }
    public virtual void DoTrick() { }
    public virtual void DoSpecial() { }
}
