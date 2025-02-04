using UnityEngine;

public class AcceleratingState : IState
{
    protected PlayerMovement _playerMovement;

    public AcceleratingState(PlayerMovement playerMovement)
    {
        _playerMovement = playerMovement;
    }
    
    public  void OnStateEnter()
    {
        Debug.Log("Entering AcceleratingState");
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
