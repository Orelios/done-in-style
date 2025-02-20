using System.Xml.Serialization;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<PlayerTricks>(out PlayerTricks playerTricks))
        {
            collision.gameObject.GetComponent<PlayerTricks>().IsWallRiding = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerTricks>(out PlayerTricks playerTricks))
        {
            collision.gameObject.GetComponent<PlayerTricks>().IsWallRiding = false;
        }
    }
}
