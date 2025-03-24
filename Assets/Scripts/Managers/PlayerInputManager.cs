using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerInputManager : MonoBehaviour
{
    //Players basic HorizontalMovement input
    public float HorizontalMovement { get; private set; }
    public void Move(InputAction.CallbackContext context)
    { HorizontalMovement = context.ReadValue<float>(); }

    //Player Jump
    public bool IsJumping { get; private set; }

    [SerializeField] private InputActionAsset playerControls;
    
    [SerializeField] private string gameplayActionMapName;
    [SerializeField] private string userInterfaceActionMapName;
    
    private InputActionMap _gameplayActionMap;
    private InputActionMap _userInterfaceActionMap;

    private void Awake()
    {
        _gameplayActionMap = playerControls.FindActionMap(gameplayActionMapName);
        _userInterfaceActionMap = playerControls.FindActionMap(userInterfaceActionMapName);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started) { IsJumping = true; }
        else if (context.canceled) { IsJumping = false; }
    }

    public void Next(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            //next dialogue line
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            EnableUserInterfaceControls();
        }
    }

    public void EnableGameplayControls()
    {
        _gameplayActionMap.Enable();
        _userInterfaceActionMap.Disable();
    }

    public void OnBack(InputAction.CallbackContext context)
    {
        if (context.started && GameStateHandler.Instance.ScreenType != EScreenType.TitleScreen)
        {
            FindFirstObjectByType<PauseMenuNavigator>().Back();
        }
    }
    
    public void EnableUserInterfaceControls()
    {
        _gameplayActionMap.Disable();
        _userInterfaceActionMap.Enable();
    }
}