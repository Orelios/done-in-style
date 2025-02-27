using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    public void GroundMovementSound() 
    { 
        AudioManager.instance.PlayOneShot(FMODEvents.instance.PlayerStep, this.transform.position); 
    }
}
