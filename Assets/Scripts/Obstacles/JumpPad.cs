using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] private float BounceHeight;
    public bool hasTricked = false;

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
        if (collision.gameObject.GetComponent<PlayerMovement>())
        { 
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * BounceHeight, ForceMode2D.Impulse);

            collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity = 
                Vector2.ClampMagnitude(collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity, BounceHeight);
            if (collision.gameObject.GetComponent<PlayerTricks>().canTrick == false && !hasTricked)
            {
                collision.gameObject.GetComponent<PlayerTricks>().EnableTrick(gameObject);
            }
            
        }
        //Debug.Log(collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.GetComponent<PlayerTricks>().AddScoreAndRank();
    }
}
