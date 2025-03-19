using UnityEngine;

public class GameplayState : GameState
{
    public GameplayState(Player player) : base(player)
    {
    }

    public override void OnStateEnter()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public override void OnStateExit()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
