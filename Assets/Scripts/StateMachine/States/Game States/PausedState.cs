using UnityEngine;

public class PausedState : GameState
{
    public PausedState(GameStateHandler handler, Player player) : base(handler, player)
    {
    }

    public override void OnStateEnter()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        GameStateHandler.IsGamePaused = true;        
        Player.InputManager.EnableUserInterfaceControls();
        SetGameTimeScale(0f);
    }

    public override void OnStateExit()
    {
        GameStateHandler.IsGamePaused = false;
        SetGameTimeScale(1f);
    }
}
