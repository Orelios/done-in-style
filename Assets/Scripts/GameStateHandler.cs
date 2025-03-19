using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateHandler : MonoBehaviour
{
    private Player _player;
    private PauseMenuNavigator _pauseMenuNavigator;
    private StateMachine _stateMachine;
    private bool _isGameplay;
    private bool _isGamePaused;
    private bool _isGameOver;
    private bool _isGameWon;

    public Type CurrentGameState => _stateMachine.GetCurrentState().GetType();
    private void Start()
    {
        _player = FindFirstObjectByType<Player>();
        _pauseMenuNavigator = FindFirstObjectByType<PauseMenuNavigator>();
        InitializeStateMachine();
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

        var gameplayState = new GameplayState(_player);
        var pausedState = new PausedState(_player);
        var gameOverState = new GameOverState(_player);
        var levelResultState = new LevelResultState(_player);
        var titleScreenState = new TitleScreenState(_player);
        
        NormalTransition(_stateMachine, gameplayState, gameOverState, new FuncPredicate(()=> _player.Health.CurrentHealth < 0));
        NormalTransition(_stateMachine, gameplayState, pausedState, new FuncPredicate(()=> _isGamePaused));
        //TODO: gameplayState to levelResultState when player reaches level end point
        
        NormalTransition(_stateMachine, pausedState, gameplayState, new FuncPredicate(()=> !_isGamePaused));
        
        //TODO: gameOverState to gameplayState when player restarts level
        
        //TODO: levelResultState to gameplayState when player restarts level or goes to next level
        
        _stateMachine.SetState(gameplayState);
    }
    
    private void NormalTransition(StateMachine stateMachine, IState currentState, IState nextState, IPredicate condition) => stateMachine.AddNormalTransition(currentState, nextState, condition);
    private void AnyTransition(StateMachine stateMachine, IState nextState, IPredicate condition) => stateMachine.AddAnyTransition(nextState, condition);
    
    public void PauseGame()
    { 
        _isGamePaused = true;
        _pauseMenuNavigator.OpenMainInterface();
        
    }
    public void ResumeGame()
    { 
        _isGamePaused = false;
        FindFirstObjectByType<PlayerInputManager>().EnableGameplayControls();
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
