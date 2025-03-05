using System.Collections;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] private float bounceHeightX;
    [SerializeField] private float bounceHeightY;
    [SerializeField] private float maxHeight;
    public bool hasTricked = false;
    private bool _hasGivenScore = false;
    public Graffiti graffiti;
    //public bool isOnJumpPad = false;
    
    private PlayerTricks _playerTricks;

    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerMovement>() && 
            collision.gameObject.GetComponent<PlayerGearSwapper>().
            CurrentGearEquipped.DaredevilGearType == EDaredevilGearType.PogoStick) 
        { collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * BounceHeight, ForceMode2D.Impulse); } 
    }
    */
    private void OnCollisionStay2D(Collision2D collision)
    {
        if( collision.gameObject.TryGetComponent<PlayerTricks>(out var playerTricks))
        {
            if (!playerTricks.isOnJumpPad)
            {
                playerTricks.isOnJumpPad = true;
                _playerTricks.CallJumpPadTimer();
            }
        }
        

        if (collision.gameObject.GetComponent<PlayerMovement>())
        {
            //collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * BounceHeight, ForceMode2D.Impulse);
            collision.gameObject.GetComponent<Rigidbody2D>().AddForceX(bounceHeightX, ForceMode2D.Impulse);
            collision.gameObject.GetComponent<Rigidbody2D>().AddForceY(bounceHeightY, ForceMode2D.Impulse);

            collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity =
                Vector2.ClampMagnitude(collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity, maxHeight);
            
        }
        //Debug.Log(collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.SpringBoard, this.transform.position);

        if (collision.gameObject.TryGetComponent<PlayerTricks>(out var playerTricks))
        {
            _playerTricks = playerTricks;
            if (!playerTricks.isOnJumpPad)
            {
                playerTricks.isOnJumpPad = true;
                _playerTricks.CallJumpPadTimer();
            }
        }

        if (!_hasGivenScore)
        {
            collision.gameObject.GetComponent<PlayerTricks>().AddScoreAndRank();
            _hasGivenScore = true;
            if (graffiti != null)
            {
                graffiti.StartGraffiti();
            }
        }
        if (!hasTricked)
        {
            collision.gameObject.GetComponent<PlayerTricks>().DisableCanTrick();
            collision.gameObject.GetComponent<PlayerTricks>().EnableTrick(gameObject);
        }
        
    }

    
}
