using UnityEngine;

public interface IGear
{
    DaredevilGearSO GearConfig { get; }

    void ApplyMovementModifiers() { }
    void DoTrick() { }
    void DoSpecial() { }
}
