using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    [field: Header("Player SFX")]
    [SerializeField] private EventReference playerJump; 
    [SerializeField] private EventReference playerDash;
    [SerializeField] private EventReference playerLanding;
    [SerializeField] private EventReference playerStep;
    [SerializeField] private EventReference playerSkating;
    [SerializeField] private EventReference playerSkatingAir;
    [SerializeField] private EventReference playerRailGrinding;
    [SerializeField] private EventReference playerTrick;
    [SerializeField] private EventReference playerGroundPound;
    public EventReference PlayerJump { get => playerJump; set => playerJump = value; }
    public EventReference PlayerDash { get => playerDash; set => playerDash = value; }
    public EventReference PlayerLanding { get => playerLanding; set => playerLanding = value; }
    public EventReference PlayerStep { get => playerStep; set => playerStep = value; }
    public EventReference PlayerSkating { get => playerSkating; set => playerSkating = value; }
    public EventReference PlayerSkatingAir { get => playerSkatingAir; set => playerSkatingAir = value; }
    public EventReference PlayerRailGrinding { get => playerRailGrinding; set => playerRailGrinding = value; }
    public EventReference PlayerTrick { get => playerTrick; set => playerTrick = value; }
    public EventReference PlayerGroundPound { get => playerGroundPound; set => playerGroundPound = value; }

    [field: Header("Environment SFX")]
    [SerializeField] private EventReference slowmo;
    [SerializeField] private EventReference breakObstacle;
    [SerializeField] private EventReference springBoard;
    public EventReference Slowmo { get => slowmo; set => slowmo = value; }
    public EventReference BreakObstacle { get => breakObstacle; set => breakObstacle = value; }
    public EventReference SpringBoard { get => springBoard; set => springBoard = value; }

    public static FMODEvents instance { get; private set; }

    private void Awake()
    {
        if (instance != null) { Debug.LogError("found more than one FMODEvents"); }
        instance = this; 
    }

    
}
