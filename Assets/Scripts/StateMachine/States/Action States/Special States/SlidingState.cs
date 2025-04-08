using UnityEngine;

public class SlidingState : SpecialState
{
    public SlidingState(Player player) : base(player)
    {
    }

    public override void OnStateEnter()
    {
        Player.Sprite.transform.localPosition = new Vector2(0f, -0.21f);
        PlayerAnimator.Play(PlayerSlidingHash);
    }
    
    public override void OnStateExit()
    {
        //reset sprite local transform
        Player.Sprite.transform.localPosition = Vector2.zero;
        Player.Tricks.IsSliding = false;
    }
}
