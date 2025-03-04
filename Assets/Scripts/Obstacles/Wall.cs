using System.Xml.Serialization;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public bool hasTricked = false;
    public bool _hasGivenScore = false;
    public Graffiti graffiti;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<PlayerTricks>(out PlayerTricks playerTricks))
        {
            playerTricks.IsWallRiding = true;
            playerTricks.GetWall(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerTricks>(out PlayerTricks playerTricks))
        {
            playerTricks.IsWallRiding = false;
            playerTricks.IsPressingDown = false; 
            if (!hasTricked) { playerTricks.EnableTrick(gameObject); }
            //playerTricks.NullWall();
        }
    }
}
