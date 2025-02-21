using UnityEngine;

public class GrindingState : SpecialState
{
    public GrindingState(Player player) : base(player)
    {
    }

    public override void OnStateEnter()
    {
        //offset the sprite local transform a little bit downwards to proper position the skateboard while grinding on rails
        Player.Sprite.transform.localPosition = new Vector2(0f, -0.4f);
        PlayerAnimator.Play(PlayerRailGrindingHash);
    }

    public override void OnStateExit()
    {
        //reset sprite local transform
        Player.Sprite.transform.localPosition = Vector2.zero;
    }
}
