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
    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started) { IsJumping = true; }
        else if (context.canceled) { IsJumping = false; }
    }
}