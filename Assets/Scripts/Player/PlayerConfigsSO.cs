using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerConfigs", menuName = "Scriptable Objects/Player Configs")]
public class PlayerConfigsSO : ScriptableObject
{
    /*[Header("Horizontal Movement")] 
    [SerializeField] private float baseMovementSpeed;
    [SerializeField] private float groundAcceleration;
    [SerializeField] private float groundDeceleration;
    [SerializeField] private float airAcceleration;
    [SerializeField] private float airDeceleration;
    public float BaseMovementSpeed { get => baseMovementSpeed; set => baseMovementSpeed = value; }
    public float GroundAcceleration { get => groundAcceleration; set => groundAcceleration = value; }
    public float GroundDeceleration { get => groundDeceleration; set => groundDeceleration = value; }
    public float AirAcceleration { get => airAcceleration; set => airAcceleration = value; }
    public float AirDeceleration { get => airDeceleration; set => airDeceleration = value; }
    
    [Header("Jump")]
    [SerializeField] private float jumpHeight;
    [SerializeField] private float jumpCompensationFactor;
    [SerializeField] private float timeUntilJumpApex;
    [SerializeField] private float gravityOnReleaseMultiplier;
    [SerializeField] private float maxFallSpeed;
    public float JumpHeight { get => jumpHeight; set => jumpHeight = value; }
    public float JumpCompensationFactor { get => jumpCompensationFactor; set => jumpCompensationFactor = value; }
    public float TimeUntilJumpApex {get => timeUntilJumpApex; set => timeUntilJumpApex = value; }
    public float GravityOnReleaseMultiplier { get => gravityOnReleaseMultiplier; set => gravityOnReleaseMultiplier = value; }
    public float MaxFallSpeed { get => maxFallSpeed; set => maxFallSpeed = value; }

    [Header("Jump Cut")] 
    [SerializeField] private float timeForUpwardsCancel;
    public float TimeForUpwardsCancel { get => timeForUpwardsCancel; set => timeForUpwardsCancel = value; }
    
    [Header("Jump Apex")]
    [SerializeField] private float apexThreshold;
    [SerializeField] private float apexHangTime;
    public float ApexThreshold { get => apexThreshold; set => apexThreshold = value; }
    public float ApexHangTime { get => apexHangTime; set => apexHangTime = value; }

    [Header("Jump Buffering")] 
    [SerializeField] private float jumpBufferingTime;
    public float JumpBufferingTime { get => jumpBufferingTime; set => jumpBufferingTime = value; }
    
    [Header("Coyote Time")]
    [SerializeField] private float coyoteTime;
    public float CoyoteTime { get => coyoteTime; set => coyoteTime = value; }
    
    [Header("Collision Checks")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float headCheckDistance;
    [SerializeField] private float headWidth;
    public LayerMask GroundLayer { get => groundLayer; set => groundLayer = value; }
    public float GroundCheckDistance { get => groundCheckDistance; set => groundCheckDistance = value; }
    public float HeadCheckDistance { get => headCheckDistance; set => headCheckDistance = value; }
    public float HeadWidth { get => headWidth; set => headWidth = value; }

    public float Gravity { get; private set; }
    public float InitialJumpVelocity { get; private set; }
    public float AdjustedJumpHeight { get; private set; }

    private void OnValidate()
    {
        CalculateValues();
    }

    private void OnEnable()
    {
        CalculateValues();
    }

    private void CalculateValues()
    {
        AdjustedJumpHeight *= jumpCompensationFactor;
        Gravity = -(2f * AdjustedJumpHeight) / Mathf.Pow(timeUntilJumpApex, 2f);
        InitialJumpVelocity = Mathf.Abs(Gravity);
    }*/
    /*
    [Header("Horizontal Movement")] 
    [SerializeField] private float baseMaxSpeed;
    [SerializeField] private float baseAcceleration;
    [HideInInspector] public float AccelerationAmount;
    [SerializeField] private float baseDeceleration;
    [HideInInspector] public float DecelerationAmount;
    [SerializeField, Range(0f, 1)] private float inAirAccelerationMultiplier;
    [SerializeField, Range(0f, 1)] private float inAirDecelerationMultiplier;
    [SerializeField] private bool doConserveMomentum;

    [Header("Jump")] 
    [SerializeField] private float maxJumpHeight;
    [SerializeField] private float jumpTimeToApex;
    [HideInInspector] public float JumpForce;
    [SerializeField] private float jumpCutGravityMultiplier;
    [SerializeField, Range(0f, 1)] private float jumpHangGravityMultiplier;
    [SerializeField] private float jumpHangTimeThreshold;
    [SerializeField] private float jumpHangAccelerationMultiplier;
    [SerializeField] private float jumpHangMaxSpeedMultiplier;
    [SerializeField] private float coyoteTime;
    [SerializeField] private float jumpInputBufferTime;

    [Header("Gravity")] 
    [HideInInspector] public float GravityStrength;
    [HideInInspector] public float GravityScale;
    [SerializeField] private float fallGravityMultiplier;
    [SerializeField] private float maxFallSpeed;
    
    private void OnValidate()
    {
        GravityStrength = -(2 * maxJumpHeight) / Mathf.Pow(jumpTimeToApex, 2);
        GravityScale = GravityStrength / Physics2D.gravity.y;
        
        AccelerationAmount = (50 * baseAcceleration) / baseMaxSpeed;
        DecelerationAmount = (50 * baseDeceleration) / baseMaxSpeed;

        JumpForce = Mathf.Abs(GravityStrength) * jumpTimeToApex;

        AccelerationAmount = Mathf.Clamp(AccelerationAmount, 0.01f, baseMaxSpeed);
        DecelerationAmount = Mathf.Clamp(DecelerationAmount, 0.01f, baseMaxSpeed);
    }*/

    [Header("Horizontal Movement Configs")]
    [Tooltip("Insert here the base movement speed of the Player")]
    [SerializeField] private float baseSpeed = 8f;
    [Tooltip("Insert here the acceleration factor of the Player")]
    [SerializeField] private float acceleration;
    [Tooltip("Insert here the deceleration factor of the Player")]
    [SerializeField] private float deceleration;
    [Tooltip("Insert here the velocity power of the Player; this is used mainly for when the Player is changing horizontal directions")]
    [SerializeField] private float velPower;
    [Tooltip("Insert here the friction amount; this helps the deceleration to put the Player to a complete stop faster ")]
    [SerializeField] private float frictionAmount;
    public float BaseSpeed { get => baseSpeed; set => baseSpeed = value; }
    public float Acceleration { get => acceleration; set => acceleration = value; }
    public float Deceleration { get => deceleration; set => deceleration = value; }
    public float VelPower { get => velPower; set => velPower = value; }
    public float FrictionAmount { get => frictionAmount; set => frictionAmount = value; }
    public float AppliedMovementSpeed { get; private set; }

    [Header("Jump Configs")]
    [Tooltip("Insert here the jump power of the Player; this is how high the Player can jump")]
    [SerializeField] private float jumpPower;
    [Tooltip("Insert here how long Coyote Time will run after the player goes off of a ledge; this is how long the Player can still jump after going off a ledge")]
    [SerializeField, Range(0.05f, 0.25f)] private float coyoteTime = 0.2f;
    [Tooltip("UNIMPLEMENTED: Insert here how long the Player's jump input will buffer; this is how long the Player's jump input is saved when in the air to help time the next jump when landing on the ground")]
    [SerializeField, Range(0.05f, 0.25f)] private float jumpBufferTime = 0.2f;
    public float JumpPower { get => jumpPower; set => jumpPower = value; }
    public float CoyoteTime { get => coyoteTime; set => coyoteTime = value; }
    public float JumpBufferTime { get => jumpBufferTime; set => jumpBufferTime = value; }

    [Header("Gravity")]
    [Tooltip("Insert here the base gravity value of the Player; this is how fast the Player will fall down")]
    [SerializeField] private float baseGravity;
    [Tooltip("Insert here the maximum falling speed the Player will have when falling down")]
    [SerializeField] private float maxFallSpeed;
    [Tooltip("Insert here the fall speed multiplier when falling down; this is used to help reach the maximum falling speed faster")]
    [SerializeField] private float fallSpeedMultiplier;
    public float BaseGravity { get => baseGravity; set => baseGravity = value; }
    public float MaxFallSpeed { get => maxFallSpeed; set => maxFallSpeed = value; }
    public float FallSpeedMultiplier { get => fallSpeedMultiplier; set => fallSpeedMultiplier = value; }
}
