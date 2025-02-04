using UnityEngine;

public class AtRestState : IState
{
    protected PlayerMovement _playerMovement;

    public AtRestState(PlayerMovement playerMovement)
    {
        _playerMovement = playerMovement;
    }
    
    public  void OnStateEnter()
    {
        Debug.Log("Entering AtRestState");
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
