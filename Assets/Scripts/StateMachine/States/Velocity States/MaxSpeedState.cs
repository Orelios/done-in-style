using UnityEngine;

public class MaxSpeedState : VelocityState
{
    public MaxSpeedState(PlayerMovement playerMovement) : base(playerMovement){ }
    
    public override void OnStateEnter()
    {
        Debug.Log("Entering MaxSpeedState");
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
