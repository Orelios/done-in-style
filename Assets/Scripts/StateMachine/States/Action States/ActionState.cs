using UnityEngine;

public class ActionState : BaseState
{
    protected Animator Animator;
    
    //TODO: implement animator int hashes
    
    protected ActionState(Player player) : base(player)
    {
        Animator = player.Animator;
    }
}
