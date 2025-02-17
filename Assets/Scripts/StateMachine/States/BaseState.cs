using UnityEngine;

public class BaseState : IState
{
    protected Player Player;

    public BaseState(Player player)
    {
        Player = player;
    }
    
    public virtual void OnStateEnter() { }

    public virtual void Update() { }

    public virtual void FixedUpdate() { }

    public virtual void OnStateExit() { }
}
