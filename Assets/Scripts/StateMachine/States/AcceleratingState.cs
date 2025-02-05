using UnityEngine;

public class AcceleratingState : VelocityState
{
    public AcceleratingState(PlayerMovement playerMovement) : base(playerMovement) { }
    
    public override void OnStateEnter()
    {
        Debug.Log("Entering AcceleratingState");
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
