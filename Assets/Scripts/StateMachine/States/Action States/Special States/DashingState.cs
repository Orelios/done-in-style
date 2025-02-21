using UnityEngine;

public class DashingState : SpecialState
{
    public DashingState(Player player) : base(player)
    {
    }

    public override void OnStateEnter()
    {
        PlayerAnimator.Play(PlayerDashingHash);
    }
}
