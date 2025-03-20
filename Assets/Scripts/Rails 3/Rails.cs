using UnityEngine;

public class Rails : MonoBehaviour
{
    private RailsParent _railsParent;

    private void Start()
    {
        _railsParent = GetComponentInParent<RailsParent>(); 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        _railsParent.GiveScore();
        
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent<PlayerRailing>(out var playerRailing))
        {
           
            playerRailing.IsMovingOnRail = true;
            playerRailing.MoveForward();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerRailing>(out var playerRailing))
        {
            playerRailing.IsMovingOnRail = false;
            
        }
    }


}
