using UnityEngine;

public class GameOverState : GameState
{
    public GameOverState(GameStateHandler handler, Player player) : base(handler, player)
    {
    }

    public override void OnStateEnter()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        GameStateHandler.IsGameOver = true;
        Player.InputManager.EnableUserInterfaceControls();
    }

    public override void OnStateExit()
    {
        GameStateHandler.IsGameOver = false;
    }
}
