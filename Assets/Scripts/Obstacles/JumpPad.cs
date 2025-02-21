using System.Collections;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] private float BounceHeight;
    public bool hasTricked = false;
    private bool _hasGivenScore = false;
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
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * BounceHeight, ForceMode2D.Impulse);

            collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity = 
                Vector2.ClampMagnitude(collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity, BounceHeight);
            /*
            if (collision.gameObject.GetComponent<PlayerTricks>().canTrick == false && !hasTricked)
            {
                collision.gameObject.GetComponent<PlayerTricks>().EnableTrick(gameObject);
            }
            */
            
        }
        //Debug.Log(collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
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
        }
        if (!hasTricked)
        {
            collision.gameObject.GetComponent<PlayerTricks>().DisableCanTrick();
            collision.gameObject.GetComponent<PlayerTricks>().EnableTrick(gameObject);
        }
        
    }

    
}
