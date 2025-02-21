using UnityEngine;

public class WallRidingState : SpecialState
{
    public WallRidingState(Player player) : base(player)
    {
    }

    public override void OnStateEnter()
    {
        PlayerAnimator.Play(PlayerWallRidingHash);
    }
}
