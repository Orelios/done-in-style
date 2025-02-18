using UnityEngine;

public class ActionState : BaseState
{
    protected Animator Animator;
    protected string SuperStateName;
    protected string SubStateName;
    
    //TODO: implement animator int hashes
    
    protected ActionState(Player player) : base(player)
    {
        Animator = player.Animator;
    }
}
