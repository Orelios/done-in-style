using UnityEngine;

public class GameState : BaseState
{
    public GameState(Player player) : base(player)
    {
    }

    public void SetGameTimeScale(float scale)
    {
        Time.timeScale = scale;
    }
}
