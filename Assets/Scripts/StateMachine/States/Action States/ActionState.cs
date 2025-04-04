using UnityEngine;

public class ActionState : BaseState
{
    protected Animator PlayerAnimator;
    
    //TODO: implement animator int hashes
    protected int PlayerIdlingHash;
    protected int PlayerSkatingHash;
    protected int PlayerJumpingHash;
    protected int PlayerDashingHash;
    protected int PlayerFallingHash;
    protected int PlayerSlidingHash;
    protected int PlayerRailGrindingHash;
    protected int PlayerWallRidingHash;
    protected int PlayerHurtHash;
    
    protected string PlayerJumpYVelocity;
    
    protected ActionState(Player player) : base(player)
    {
        PlayerAnimator = player.Animator;

        PlayerIdlingHash = Animator.StringToHash(player.IdlingAnimationName);
        PlayerSkatingHash = Animator.StringToHash(player.SkatingAnimationName);
        PlayerJumpingHash = Animator.StringToHash(player.JumpingAnimationName);
        PlayerDashingHash = Animator.StringToHash(player.DashingAnimationName);
        PlayerFallingHash = Animator.StringToHash(player.FallingAnimationName);
        PlayerSlidingHash = Animator.StringToHash(player.SlidingAnimationName);
        PlayerRailGrindingHash = Animator.StringToHash(player.RailGrindingAnimationName);
        PlayerWallRidingHash = Animator.StringToHash(player.WallRidingAnimationName);
        PlayerHurtHash = Animator.StringToHash(player.HurtAnimationName);

        PlayerJumpYVelocity = player.BlendTreeYVelocityName;
    }
}
