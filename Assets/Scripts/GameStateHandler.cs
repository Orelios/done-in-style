using System;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private bool _isGameplay;
    private bool _isGamePaused;
    private bool _isGameOver;
    private bool _isResultScreen;
    private bool _isTitleScreen;

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

        _gameplayState = new GameplayState(_player);
        _pausedState = new PausedState(_player);
        _gameOverState = new GameOverState(_player);
        _levelResultState = new LevelResultState(_player);
        _titleScreenState = new TitleScreenState(_player);
        
        NormalTransition(_stateMachine, _titleScreenState, _gameplayState, new FuncPredicate(() => !_isTitleScreen));
        
        NormalTransition(_stateMachine, _gameplayState, _gameOverState, new FuncPredicate(()=> _player.Health.CurrentHealth < 0));
        NormalTransition(_stateMachine, _gameplayState, _pausedState, new FuncPredicate(()=> _isGamePaused));
        //TODO: gameplayState to levelResultState when player reaches level end point
        
        NormalTransition(_stateMachine, _pausedState, _gameplayState, new FuncPredicate(()=> !_isGamePaused));
        
        //TODO: gameOverState to gameplayState when player restarts level
        
        //TODO: levelResultState to gameplayState when player restarts level or goes to next level
    }
    
    private void NormalTransition(StateMachine stateMachine, IState currentState, IState nextState, IPredicate condition) => stateMachine.AddNormalTransition(currentState, nextState, condition);
    private void AnyTransition(StateMachine stateMachine, IState nextState, IPredicate condition) => stateMachine.AddAnyTransition(nextState, condition);
    
    private void SetState()
    {
        _isGameplay = false; 
        _isGamePaused = false; 
        _isGameOver = false; 
        _isResultScreen = false; 
        _isTitleScreen = true;
        
        switch (screenType)
        {
            case EScreenType.TitleScreen:
                _stateMachine.SetState(_titleScreenState);
                _isTitleScreen = true;
                FindFirstObjectByType<PlayerInputManager>().EnableUserInterfaceControls();
                break;
            case EScreenType.Gameplay:
                _stateMachine.SetState(_gameplayState);
                _isGameplay = true;
                FindFirstObjectByType<PlayerInputManager>().EnableGameplayControls();
                break;
            case EScreenType.ResultsScreen:
                _stateMachine.SetState(_levelResultState);
                _isResultScreen = true;
                FindFirstObjectByType<PlayerInputManager>().EnableUserInterfaceControls();
                break;
        }
        
        Debug.Log($"Current Screen: {screenType}\n Current State: {CurrentGameState}");
    }

    public void StartTitleScreen()
    {
        _isGameplay = false; 
        _isGamePaused = false; 
        _isGameOver = false; 
        _isResultScreen = false; 
        _isTitleScreen = true;
    
    //SetState(_titleScreenState);
    FindFirstObjectByType<PlayerInputManager>().EnableUserInterfaceControls();
    }
    
    public void StartGameplay()
    {
        _isGameplay = true;
        _isTitleScreen = false;
        FindFirstObjectByType<PlayerInputManager>().EnableGameplayControls();
    }
    
    public void PauseGame()
    { 
        _isGamePaused = true;
        _isGameplay = false;
        _pauseMenuNavigator.OpenMainInterface();
        
    }
    public void ResumeGame()
    { 
        _isGamePaused = false;
        _isGameplay = true;
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
    ResultsScreen
}
