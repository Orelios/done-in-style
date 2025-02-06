using UnityEngine;

public class Skateboard : Gear
{
    private DaredevilGearSO _gearConfig;
    DaredevilGearSO GearConfig => _gearConfig;

    public Skateboard(New_PlayerMovement playerMovement, DaredevilGearSO gearSo) : base(playerMovement, gearSo) { }
    public override void ApplyMovementModifiers()
    {

    }
    public override void DoTrick()
    {


    }
    public override void DoSpecial()
    {


    }


}
