using UnityEngine;

public class Pogostick : Gear
{
    private DaredevilGearSO _gearConfig;
    DaredevilGearSO GearConfig => _gearConfig;

    public Pogostick(New_PlayerMovement playerMovement, DaredevilGearSO gearSo, int maxJumps, float jumpCooldown, 
        float inBetweenJumpCooldown) : base(playerMovement, gearSo) 
    { 
        _maxJumps = maxJumps;
        _jumpCooldown = jumpCooldown;
        _inBetweenJumpCooldown = inBetweenJumpCooldown;
    
    }

    private int _maxJumps;
    private float _jumpCooldown;
    private float _inBetweenJumpCooldown;
    private float _lastJumpTime;
    private float _lastInBetweenJumpTime;
    private float _jumps;

    public override void ApplyMovementModifiers() 
    {

        if(PlayerMovement._rb.linearVelocityY <= 0 && PlayerMovement.IsGrounded())
        {
            PlayerMovement._rb.linearVelocityX = 0;
        }
    }
    public override void DoTrick() 
    {
        if (Time.time < _lastJumpTime + _jumpCooldown) { return; }
        else if (_jumps == 0) { _jumps = _maxJumps; }

        if (_jumps != 0)
        {
            //add score
            //scoreCalculator.AddScore(scorePerTrick, rankCalculator.CurrentStylishRank.ScoreMultiplier);
            //rankCalculator.IncreaseStylishPoints();

            if (Time.time >= _lastInBetweenJumpTime + _inBetweenJumpCooldown) { _jumps = _maxJumps; }

            PlayerMovement._rb.linearVelocity = new Vector2(PlayerMovement._rb.linearVelocity.x, 
                PlayerMovement.PlayerConfigsSO.JumpPower * GearConfig.JumpForceMultiplier);

            _jumps--;

            _lastInBetweenJumpTime = Time.time;

            if (_jumps == 0) { _lastJumpTime = Time.time; }
        }

    }
    public override void DoSpecial() 
    { 

    
    }

   
}
