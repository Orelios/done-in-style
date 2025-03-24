using UnityEngine;

public class TitleScreenState : GameState
{
    public TitleScreenState(GameStateHandler handler, Player player) : base(handler, player)
    {
    }

    public override void OnStateEnter()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        //Object.FindFirstObjectByType<PlayerInputManager>().EnableUserInterfaceControls();
        GameStateHandler.IsTitleScreen = true;
        Player.InputManager.EnableUserInterfaceControls();
    }

    public override void OnStateExit()
    {
        GameStateHandler.IsTitleScreen = false;
    }
}
