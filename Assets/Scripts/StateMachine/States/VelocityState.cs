using UnityEngine;

public class VelocityState : IState
{
    protected PlayerMovement PlayerMovement;
    //TODO: implement animator
    protected Animator PlayerAnimator;

    protected VelocityState(PlayerMovement playerMovement)
    {
        PlayerMovement = playerMovement;
    }
    
    public virtual void OnStateEnter() { }

    public virtual void Update() { }

    public virtual void FixedUpdate() { }

    public virtual void OnStateExit() { }
}
