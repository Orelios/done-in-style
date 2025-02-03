using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerConfigs", menuName = "Scriptable Objects/Player Configs")]
public class PlayerConfigsSO : ScriptableObject
{
    [Header("Horizontal Movement")] 
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
    }
}
