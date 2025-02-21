using UnityEngine;

public class RisingState : InAirState
{
    public RisingState(Player player) : base(player)
    {
    }

    public override void OnStateEnter()
    {
        PlayerAnimator.Play(PlayerJumpingHash);
    }

    public override void Update()
    {
        PlayerAnimator.SetFloat(PlayerJumpYVelocity, Player.Rigidbody.linearVelocityY);
    }
}
