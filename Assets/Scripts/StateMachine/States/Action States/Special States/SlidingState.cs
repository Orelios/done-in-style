using UnityEngine;

public class SlidingState : ActionState
{
    public SlidingState(Player player) : base(player)
    {
    }

    public override void OnStateEnter()
    {
        PlayerAnimator.Play(PlayerSlidingHash);
    }
}
