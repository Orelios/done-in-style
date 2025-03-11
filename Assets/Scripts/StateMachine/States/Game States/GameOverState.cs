using UnityEngine;

public class GameOverState : GameState
{
    public GameOverState(Player player) : base(player)
    {
    }

    public override void OnStateEnter()
    {
        SetGameTimeScale(0f);
    }

    public override void OnStateExit()
    {
        SetGameTimeScale(1f);
    }
}
