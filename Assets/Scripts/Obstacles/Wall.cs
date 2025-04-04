using System.Xml.Serialization;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public bool hasTricked = false;
    public bool _hasGivenScore = false;
    public bool _canWallRide = false;
    public Graffiti graffiti;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<PlayerTricks>(out PlayerTricks playerTricks))
        {
            playerTricks.IsWallRiding = true;
            _canWallRide = true;
            playerTricks.CanDoubleJump();
            playerTricks.GetWall(this);
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerTricks>(out PlayerTricks playerTricks))
        {
            playerTricks.IsWallRiding = false;
            playerTricks._playerSkatingWallRide.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            _canWallRide = false;
            playerTricks.ResetWallRideValues();
            if (!hasTricked) { playerTricks.EnableTrick(gameObject); }
            //playerTricks.NullWall();
        }
    }
}
