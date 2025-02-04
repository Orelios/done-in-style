using UnityEngine;

public class MaxSpeedState : IState
{
    protected PlayerMovement _playerMovement;

    public MaxSpeedState(PlayerMovement playerMovement)
    {
        _playerMovement = playerMovement;
    }
    
    public  void OnStateEnter()
    {
        Debug.Log("Entering MaxSpeedState");
    }

    public  void Update()
    {
        
    }

    public  void FixedUpdate()
    {
        
    }

    public  void OnStateExit()
    {
        
    }
}
