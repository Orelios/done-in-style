using UnityEngine;

public class IdlingState : GroundedState
{
    public IdlingState(Player player) : base(player)
    {
    }

    public override void OnStateEnter()
    {
        PlayerAnimator.Play(PlayerIdlingHash);
    }
}
