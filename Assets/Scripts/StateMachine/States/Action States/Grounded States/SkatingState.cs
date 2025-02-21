using UnityEngine;

public class SkatingState : GroundedState
{
    public SkatingState(Player player) : base(player)
    {
    }

    public override void OnStateEnter()
    {
        PlayerAnimator.Play(PlayerSkatingHash);
    }

    public override void FixedUpdate()
    {
        //TODO: call horizontal movement function here
    }
}
