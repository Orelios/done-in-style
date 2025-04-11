using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    [field: Header("BGM")]
    [SerializeField] private EventReference skateParkMusic1;
    [SerializeField] private EventReference resultsScreenMusic;
    [SerializeField] private EventReference mainMenuMusic;
    [SerializeField] private EventReference creditsMusic;
    public EventReference SkateParkMusic1 { get => skateParkMusic1; set => skateParkMusic1 = value; }
    public EventReference ResultsScreenMusic { get => resultsScreenMusic; set => resultsScreenMusic = value; }
    public EventReference MainMenuMusic { get => mainMenuMusic; set => mainMenuMusic = value; }
    public EventReference CreditsMusic { get => creditsMusic; set => creditsMusic = value; }

    [field: Header("Ambience")]
    [SerializeField] private EventReference skateParkAmbience;
    public EventReference SkateParkAmbience { get => skateParkAmbience; set => skateParkAmbience = value; }

    [field: Header("Player SFX")]
    [SerializeField] private EventReference playerJump; 
    [SerializeField] private EventReference playerDash;
    [SerializeField] private EventReference playerLanding;
    [SerializeField] private EventReference playerLandingWallRide;
    [SerializeField] private EventReference playerStep;
    [SerializeField] private EventReference playerSkating;
    [SerializeField] private EventReference playerSkatingAir;
    [SerializeField] private EventReference playerSkatingWallRide;
    [SerializeField] private EventReference playerRailGrinding;
    [SerializeField] private EventReference playerTrick;
    [SerializeField] private EventReference playerGroundPound;
    [SerializeField] private EventReference playerHurt;
    [SerializeField] private EventReference playerMovement;
    public EventReference PlayerJump { get => playerJump; set => playerJump = value; }
    public EventReference PlayerDash { get => playerDash; set => playerDash = value; }
    public EventReference PlayerLanding { get => playerLanding; set => playerLanding = value; }
    public EventReference PlayerLandingWallRide { get => playerLandingWallRide; set => playerLandingWallRide = value; }
    public EventReference PlayerStep { get => playerStep; set => playerStep = value; }
    public EventReference PlayerSkating { get => playerSkating; set => playerSkating = value; }
    public EventReference PlayerSkatingAir { get => playerSkatingAir; set => playerSkatingAir = value; }
    public EventReference PlayerSkatingWallRide { get => playerSkatingWallRide; set => playerSkatingWallRide = value; }
    public EventReference PlayerRailGrinding { get => playerRailGrinding; set => playerRailGrinding = value; }
    public EventReference PlayerTrick { get => playerTrick; set => playerTrick = value; }
    public EventReference PlayerGroundPound { get => playerGroundPound; set => playerGroundPound = value; }
    public EventReference PlayerHurt { get => playerHurt; set => playerHurt = value; }
    public EventReference PlayerMovement { get => playerMovement; set => playerMovement = value; }

    [field: Header("Environment SFX")]
    [SerializeField] private EventReference slowmo;
    [SerializeField] private EventReference breakObstacle;
    [SerializeField] private EventReference springBoard;
    [SerializeField] private EventReference healthPickup;
    [SerializeField] private EventReference tmShoot;
    [SerializeField] private EventReference cameraRecording;
    [SerializeField] private EventReference graffitiSpray;
    public EventReference Slowmo { get => slowmo; set => slowmo = value; }
    public EventReference BreakObstacle { get => breakObstacle; set => breakObstacle = value; }
    public EventReference SpringBoard { get => springBoard; set => springBoard = value; }
    public EventReference HealthPickup { get => healthPickup; set => healthPickup = value; }
    public EventReference TMShoot { get => tmShoot; set => tmShoot = value; }
    public EventReference CameraRecording { get => cameraRecording; set => cameraRecording = value; }
    public EventReference GraffitiSpray { get => graffitiSpray; set => graffitiSpray = value; }

    [field: Header("UI SFX")]
    [SerializeField] private EventReference voxCookin;
    [SerializeField] private EventReference voxBallin;
    [SerializeField] private EventReference voxAwesome;
    [SerializeField] private EventReference voxStylish;
    [SerializeField] private EventReference phoneActivate;
    [SerializeField] private EventReference phoneDeactivate;
    [SerializeField] private EventReference phoneHover;
    [SerializeField] private EventReference phoneClick;
    [SerializeField] private EventReference phoneExit;
    [SerializeField] private EventReference mainMenuHover;
    [SerializeField] private EventReference mainMenuClick;
    [SerializeField] private EventReference mainMenuExit;
    [SerializeField] private EventReference resultsScreenPointsIncrease;
    [SerializeField] private EventReference wastedActivate;
    public EventReference VOXCookin { get => voxCookin; set => voxCookin = value; }
    public EventReference VOXBallin { get => voxBallin; set => voxBallin = value; }
    public EventReference VOXAwesome { get => voxAwesome; set => voxAwesome = value; }
    public EventReference VOXStylish { get => voxStylish; set => voxStylish = value; }
    public EventReference PhoneActivate { get => phoneActivate; set => phoneActivate = value; }
    public EventReference PhoneDeactivate { get => phoneDeactivate; set => phoneDeactivate = value; }
    public EventReference PhoneHover { get => phoneHover; set => phoneHover = value; }
    public EventReference PhoneClick { get => phoneClick; set => phoneClick = value; }
    public EventReference PhoneExit { get => phoneExit; set => phoneExit = value; }
    public EventReference MainMenuHover { get => mainMenuHover; set => mainMenuHover = value; }
    public EventReference MainMenuClick { get => mainMenuClick; set => mainMenuClick = value; }
    public EventReference MainMenuExit { get => mainMenuExit; set => mainMenuExit = value; }
    public EventReference ResultsScreenPointsIncrease { get => resultsScreenPointsIncrease; set => resultsScreenPointsIncrease = value; }
    public EventReference WastedActivate { get => wastedActivate; set => wastedActivate = value; }



    public static FMODEvents instance { get; private set; }

    private void Awake()
    {
        if (instance != null) { Debug.LogError("found more than one FMODEvents"); }
        instance = this; 
    }

    
}
