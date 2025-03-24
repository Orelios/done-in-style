using UnityEngine;

public class LevelResultState : GameState
{
    public LevelResultState(GameStateHandler handler, Player player) : base(handler, player)
    {
    }
    
    public override void OnStateEnter()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        GameStateHandler.IsResultScreen = true;
        Player.InputManager.EnableUserInterfaceControls();
    }

    public override void OnStateExit()
    {
        GameStateHandler.IsResultScreen = false;
    }
}
