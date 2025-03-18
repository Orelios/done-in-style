using UnityEngine;

public class LevelResultState : GameState
{
    public LevelResultState(Player player) : base(player)
    {
    }
    
    public override void OnStateEnter()
    {
        SetGameTimeScale(0f);
    }

    public override void OnStateExit()
    {
        SetGameTimeScale(1f);
    }
}
