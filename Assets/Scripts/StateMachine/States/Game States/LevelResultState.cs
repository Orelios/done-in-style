using UnityEngine;

public class LevelResultState : GameState
{
    public LevelResultState(Player player) : base(player)
    {
    }
    
    public override void OnStateEnter()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
