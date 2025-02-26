using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    [SerializeField] private EventReference test; 
    public EventReference Test { get => test; set => test = value; }
    public static FMODEvents instance { get; private set; }

    private void Awake()
    {
        if (instance != null) { Debug.LogError("found more than one FMODEvents"); }
    }

    
}
