using System;
using System.Collections;
using System.IO;
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
        NormalTransition(_stateMachine, _gameplayState, _levelResultState, new FuncPredicate(()=> IsResultScreen));
        
        NormalTransition(_stateMachine, _pausedState, _gameplayState, new FuncPredicate(()=> !IsGamePaused));
        
        NormalTransition(_stateMachine, _gameOverState, _gameplayState, new FuncPredicate(()=> !IsGameOver));
        
        NormalTransition(_stateMachine, _levelResultState, _gameplayState, new FuncPredicate(()=> !IsResultScreen));
    }
    
    private void NormalTransition(StateMachine stateMachine, IState currentState, IState nextState, IPredicate condition) => stateMachine.AddNormalTransition(currentState, nextState, condition);
    private void AnyTransition(StateMachine stateMachine, IState nextState, IPredicate condition) => stateMachine.AddAnyTransition(nextState, condition);
    
    private void SetState()
    {
        ResetFlags();
        
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
    }
    
    public async void StartGameplay()
    {
        ResetFlags();
        IsGameplay = true;
        
        StopAudio();
        //SceneManager.LoadScene(2);
        await SceneLoader.LoadScene(SceneLoader.LoadingScreenHash, Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(3)));
    }

    public async void ReturnToMainMenu()
    {
        ResetFlags();
        IsTitleScreen = true;
        
        GameplayData.Reset();
        StopAudio();
        //SceneManager.LoadScene("TitleScreen");
        await SceneLoader.LoadScene(SceneLoader.LoadingScreenHash, SceneLoader.TitleScreenHash);
    }
    
    public void PauseGame()
    {
        ResetFlags();
        IsGamePaused = true;
        _player.GetComponent<PlayerMovement>()._playerMovement.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);        
        _pauseMenuNavigator.OpenMainInterface();
    }
    public void ResumeGame()
    {
        ResetFlags();
        IsGameplay = true;
        _player.GetComponent<PlayerMovement>()._playerMovement.start();
        FindFirstObjectByType<PlayerInputManager>().EnableGameplayControls();
    }

    public async void RestartLevel()
    {
        StopAudio();
        
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        await SceneLoader.LoadScene(SceneLoader.LoadingScreenHash, SceneManager.GetActiveScene().name);
    }

    public async void FinishLevel()
    {
        ResetFlags();
        IsResultScreen = true;

        _player.GetComponent<PlayerMovement>()._playerMovement.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        StopAudio();
        FindFirstObjectByType<PlayerInputManager>().EnableUserInterfaceControls();
        FindFirstObjectByType<ScoreCalculator>().IncreaseScoreOnLevelClear(FindFirstObjectByType<TimeHandler>().ElapsedTime);
        //SceneManager.LoadScene("ResultsScreen");
        await SceneLoader.LoadScene(SceneLoader.LoadingScreenHash, SceneLoader.ResultsScreenHash);
    }

    private void ResetFlags()
    {
        IsGameplay = false; 
        IsGamePaused = false; 
        IsGameOver = false; 
        IsResultScreen = false; 
        IsTitleScreen = false;
    }

    private void StopAudio()
    {
        AudioManager.instance.musicEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        AudioManager.instance.ambienceEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        AudioManager.instance.musicEventInstance.release();
        AudioManager.instance.ambienceEventInstance.release();
    }
}


public enum EScreenType
{
    TitleScreen,
    Gameplay,
    GameOver,
    ResultsScreen
}
