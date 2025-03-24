using UnityEngine;

public class BaseState : IState
{
    protected Player Player;
    protected GameStateHandler GameStateHandler;

    public BaseState(Player player)
    {
        Player = player;
    }

    public BaseState(GameStateHandler gameStateHandler, Player player)
    {
        GameStateHandler = gameStateHandler;
        Player = player;
    }
    
    public virtual void OnStateEnter() { }

    public virtual void Update() { }

    public virtual void FixedUpdate() { }

    public virtual void OnStateExit() { }
}
