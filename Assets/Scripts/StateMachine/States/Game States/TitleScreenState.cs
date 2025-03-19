using UnityEngine;

public class TitleScreenState : GameState
{
    public TitleScreenState(Player player) : base(player)
    {
    }

    public override void OnStateEnter()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
