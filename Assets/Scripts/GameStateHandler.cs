using FMOD.Studio;
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;

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

    private  void Start()
    {
        _player = FindFirstObjectByType<Player>();
        _pauseMenuNavigator = FindFirstObjectByType<PauseMenuNavigator>();
        InitializeStateMachine();
        
        SetState();
        
        //TODO: Optimize in the scene flow
        if (GameplayData.LastLevelIndex < 3 && SceneManager.GetActiveScene().buildIndex > 2)
        {
            GameplayData.RecordLevel(SceneManager.GetActiveScene().name, SceneManager.GetActiveScene().buildIndex);
        }

        AudioManager.instance.PlayerRailing = AudioManager.instance.CreateInstance(FMODEvents.instance.PlayerRailGrinding);
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
        AudioManager.instance.InGameSFXBus.setMute(false);
        await SceneLoader.LoadScene(SceneLoader.LoadingScreenName, Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(3)), true);
    }

    public async void ReturnToMainMenu()
    {
        ResetFlags();
        IsTitleScreen = true;
        
        GameplayData.Reset();
        StopAudio();
        AudioManager.instance.InGameSFXBus.setMute(false);
        AudioManager.instance.musicEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        await SceneLoader.LoadScene(SceneLoader.LoadingScreenName, SceneLoader.TitleScreenName);
    }
    
    public void PauseGame()
    {
        StopAudio();
        ResetFlags();
        IsGamePaused = true;
        _player.GetComponent<PlayerMovement>()._playerMovement.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);        
        _pauseMenuNavigator.OpenPauseMenu();
        AudioManager.instance.PlayOneShotNoLocation(FMODEvents.instance.PhoneActivate);
    }
    public void ResumeGame()
    {
        AudioManager.instance.InGameSFXBus.setMute(false);
        AudioManager.instance.musicEventInstance.setPaused(false);
        AudioManager.instance.ambienceEventInstance.setPaused(false);
        //AudioManager.instance.InitializeMusic(FMODEvents.instance.SkateParkMusic1);
        //AudioManager.instance.InitializeAmbience(FMODEvents.instance.SkateParkAmbience);
        ResetFlags();
        IsGameplay = true;
        _player.GetComponent<PlayerMovement>()._playerMovement.start();
        FindFirstObjectByType<PlayerInputManager>().EnableGameplayControls();
        AudioManager.instance.PlayOneShotNoLocation(FMODEvents.instance.PhoneDeactivate);
    }

    public async void RestartLevel()
    {
        StopAudio();
        AudioManager.instance.InGameSFXBus.setMute(false);
        await SceneLoader.LoadScene(SceneLoader.LoadingScreenName, SceneManager.GetActiveScene().name);
    }

    public async void FinishLevel()
    {
        ResetFlags();
        IsResultScreen = true;

        _player.GetComponent<PlayerMovement>()._playerMovement.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        StopAudio();
        AudioManager.instance.InGameSFXBus.setMute(false);
        FindFirstObjectByType<PlayerInputManager>().EnableUserInterfaceControls();
        FindFirstObjectByType<ScoreCalculator>().IncreaseScoreOnLevelClear(FindFirstObjectByType<TimeHandler>().ElapsedTime);
        await SceneLoader.LoadScene(SceneLoader.LoadingScreenName, SceneLoader.ResultsScreenName);
    }

    public void QuitGame()
    {
        Application.Quit();
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
        AudioManager.instance.InGameSFXBus.setMute(true);
        AudioManager.instance.musicEventInstance.setPaused(true);
        AudioManager.instance.ambienceEventInstance.setPaused(true);
        //AudioManager.instance.musicEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        //AudioManager.instance.ambienceEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        AudioManager.instance.PlayerRailing.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
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
