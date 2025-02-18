using UnityEngine;

public class ActionState : BaseState
{
    protected Animator PlayerAnimator;
    
    //TODO: implement animator int hashes
    
    protected ActionState(Player player) : base(player)
    {
        PlayerAnimator = player.Animator;
    }
}
