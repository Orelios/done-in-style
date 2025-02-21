using UnityEngine;

public class HurtState : SpecialState
{
    public HurtState(Player player) : base(player)
    {
    }

    public override void OnStateEnter()
    {
        PlayerAnimator.Play(PlayerHurtHash);
    }
}
