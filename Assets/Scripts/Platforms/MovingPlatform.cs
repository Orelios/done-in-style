using System;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && other.gameObject.GetComponent<PlayerInputManager>().HorizontalMovement == 0)
        {
            other.transform.SetParent(transform);
            
            
        }
        
        if(other.gameObject.GetComponent<PlayerInputManager>().HorizontalMovement != 0)
        {
            other.transform.SetParent(null);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.SetParent(null);
        }
    }
}
