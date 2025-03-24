using UnityEngine;

public class GameplayState : GameState
{
    public GameplayState(GameStateHandler handler, Player player) : base(handler, player)
    {
    }

    public override void OnStateEnter()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        GameStateHandler.IsGameplay = true;
        Player.InputManager.EnableGameplayControls();
    }

    public override void OnStateExit()
    {
        GameStateHandler.IsGameplay = false;
    }
}
