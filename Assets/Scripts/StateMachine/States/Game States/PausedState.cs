using UnityEngine;

public class PausedState : GameState
{
    public PausedState(Player player) : base(player)
    {
    }

    public override void OnStateEnter()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SetGameTimeScale(0f);
    }

    public override void OnStateExit()
    {
        SetGameTimeScale(1f);
    }
}
