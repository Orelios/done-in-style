using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameStateHandler : MonoBehaviour
{
    private static GameStateHandler _instance;
    public static GameStateHandler Instance => _instance;

    [SerializeField] private EScreenType screenType;
    public EScreenType ScreenType => screenType;
    
    private Player _player;
    private PauseMenuNavigator _pauseMenuNavigator;
    private StateMachine _stateMachine;
    private GameplayState _gameplayState;
    private PausedState _pausedState;
    private GameOverState _gameOverState;
    private LevelResultState _levelResultState;
    private TitleScreenState _titleScreenState;
    
    public bool IsGameplay;
    public bool IsGamePaused;
    public bool IsGameOver;
    public bool IsResultScreen;
    public bool IsTitleScreen;

    public Type CurrentGameState => _stateMachine.GetCurrentState().GetType();

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);   
        }
        else
        {
            _instance = this;
        }
        
        //DontDestroyOnLoad(gameObject);
    }
    
    private void Start()
    {
        _player = FindFirstObjectByType<Player>();
        _pauseMenuNavigator = FindFirstObjectByType<PauseMenuNavigator>();
        InitializeStateMachine();

        //SetState(_gameplayState);
        //StartTitleScreen();
        SetState();
    }

    private void Update()
    {
        _stateMachine?.Update();
    }

    private void FixedUpdate()
    {
        _stateMachine?.FixedUpdate();
    }

    private void InitializeStateMachine()
    {
        _stateMachine = new();

        _gameplayState = new GameplayState(this, _player);
        _pausedState = new PausedState(this, _player);
        _gameOverState = new GameOverState(this, _player);
        _levelResultState = new LevelResultState(this, _player);
        _titleScreenState = new TitleScreenState(this, _player);
        
        NormalTransition(_stateMachine, _titleScreenState, _gameplayState, new FuncPredicate(() => !IsTitleScreen));
        
        NormalTransition(_stateMachine, _gameplayState, _gameOverState, new FuncPredicate(()=> _player.Health.CurrentHealth < 1));
        NormalTransition(_stateMachine, _gameplayState, _pausedState, new FuncPredicate(()=> IsGamePaused));
        //TODO: gameplayState to levelResultState when player reaches level end point
        
        NormalTransition(_stateMachine, _pausedState, _gameplayState, new FuncPredicate(()=> !IsGamePaused));
        
        //TODO: gameOverState to gameplayState when player restarts level
        NormalTransition(_stateMachine, _gameOverState, _gameplayState, new FuncPredicate(()=> !IsGameOver));
        
        //TODO: levelResultState to gameplayState when player restarts level or goes to next level
    }
    
    private void NormalTransition(StateMachine stateMachine, IState currentState, IState nextState, IPredicate condition) => stateMachine.AddNormalTransition(currentState, nextState, condition);
    private void AnyTransition(StateMachine stateMachine, IState nextState, IPredicate condition) => stateMachine.AddAnyTransition(nextState, condition);
    
    private void SetState()
    {
        IsGameplay = false; 
        IsGamePaused = false; 
        IsGameOver = false; 
        IsResultScreen = false; 
        IsTitleScreen = false;
        
        switch (screenType)
        {
            case EScreenType.TitleScreen:
                _stateMachine.SetState(_titleScreenState);
                IsTitleScreen = true;
                FindFirstObjectByType<PlayerInputManager>().EnableUserInterfaceControls();
                break;
            case EScreenType.Gameplay:
                _stateMachine.SetState(_gameplayState);
                IsGameplay = true;
                FindFirstObjectByType<PlayerInputManager>().EnableGameplayControls();
                break;
            case EScreenType.GameOver:
                _stateMachine.SetState(_gameOverState);
                IsGameOver = true;
                FindFirstObjectByType<PlayerInputManager>().EnableUserInterfaceControls();
                break;
            case EScreenType.ResultsScreen:
                _stateMachine.SetState(_levelResultState);
                IsResultScreen = true;
                FindFirstObjectByType<PlayerInputManager>().EnableUserInterfaceControls();
                break;
        }
        
        Debug.Log($"Current Screen: {screenType}\n Current State: {CurrentGameState}");
    }

    public void StartTitleScreen()
    {
        IsGameplay = false; 
        IsGamePaused = false; 
        IsGameOver = false; 
        IsResultScreen = false; 
        IsTitleScreen = true;
    
    //SetState(_titleScreenState);
    FindFirstObjectByType<PlayerInputManager>().EnableUserInterfaceControls();
    }
    
    public void StartGameplay()
    {
        IsGameplay = true;
        IsTitleScreen = false;
        FindFirstObjectByType<PlayerInputManager>().EnableGameplayControls();
    }
    
    public void PauseGame()
    { 
        IsGamePaused = true;
        IsGameplay = false;
        _pauseMenuNavigator.OpenMainInterface();
        
    }
    public void ResumeGame()
    { 
        IsGamePaused = false;
        IsGameplay = true;
        FindFirstObjectByType<PlayerInputManager>().EnableGameplayControls();
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void AssignValues(Player player, PauseMenuNavigator pauseMenuNavigator)
    {
        _player = player;
        _pauseMenuNavigator = pauseMenuNavigator;
    }
}

public enum EScreenType
{
    TitleScreen,
    Gameplay,
    GameOver,
    ResultsScreen
}
