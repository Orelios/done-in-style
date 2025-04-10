using UnityEngine;
using UnityEngine.InputSystem;

public class BindingResetter : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActionAsset;
    
    public void ResetAllBindings()
    {
        foreach (var map in inputActionAsset)
        {
            map.RemoveAllBindingOverrides();
        }
    }
}
