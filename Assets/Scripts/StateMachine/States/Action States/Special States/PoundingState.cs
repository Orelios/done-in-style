using UnityEngine;

public class PoundingState : SpecialState
{
    public PoundingState(Player player) : base(player)
    {
    }

    public override void OnStateEnter()
    {
        PlayerAnimator.Play(PlayerFallingHash);
    }
}
