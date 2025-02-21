using UnityEngine;

public class FallingState : InAirState
{
    public FallingState(Player player) : base(player)
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
