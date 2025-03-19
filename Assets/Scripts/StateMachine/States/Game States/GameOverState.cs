using UnityEngine;

public class GameOverState : GameState
{
    public GameOverState(Player player) : base(player)
    {
    }

    public override void OnStateEnter()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
