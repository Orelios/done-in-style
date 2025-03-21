using UnityEngine;

public class GameState : BaseState
{
    public GameState(GameStateHandler handler, Player player) : base(handler, player)
    {
    }

    public void SetGameTimeScale(float scale)
    {
        Time.timeScale = scale;
    }
}
