using UnityEngine;

public class AtRestState : VelocityState
{
    public AtRestState(PlayerMovement playerMovement) : base(playerMovement){ }
    
    public override void OnStateEnter()
    {
        Debug.Log("Entering AtRestState");
    }

    public override void Update()
    {
        
    }

    public override void FixedUpdate()
    {
        
    }

    public override void OnStateExit()
    {
        
    }
}
