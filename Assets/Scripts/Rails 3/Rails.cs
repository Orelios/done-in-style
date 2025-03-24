using UnityEngine;

public class Rails : MonoBehaviour
{
   
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
