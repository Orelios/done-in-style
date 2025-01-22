using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] private float BounceHeight; 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerMovement>()) 
        { collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * BounceHeight, ForceMode2D.Impulse); } 
    }
}
