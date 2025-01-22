using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem; 
public class PlayerInputManager : MonoBehaviour
{
    //Players basic movement input
    public Vector2 Movement {  get; private set; }
    public void Move(InputAction.CallbackContext context)
        {Movement = context.ReadValue<Vector2>(); }

    //Player Jump
    public bool Jumping { get; private set; }
    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started) { Jumping = true; }
        else if (context.canceled) { Jumping = false; }
    }
}
