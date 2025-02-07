using UnityEngine;

public class TiltingPlatform : MonoBehaviour
{
    [SerializeField] private float maxTiltAngle = 15f;  // Maximum tilt angle in degrees

    private Vector2 platformCenter;  // The center of the platform
    private float platformWidth;     // The width of the platform (calculated from its scale)
    private Transform player;        // The player's transform (assigned when the player collides)

    void Start()
    {
        // Get the center of the platform (relative to its position)
        platformCenter = transform.position;

        // Automatically calculate the platform width based on its scale (localScale.x)
        platformWidth = transform.localScale.x;
    }

    void Update()
    {
        // Only tilt the platform if the player is on it
        if (player != null)
        {
            TiltPlatform();
        }
    }

    void TiltPlatform()
    {
        // Get the player's position relative to the platform's center
        float playerDistance = Mathf.Abs(player.position.x - platformCenter.x);

        // Normalize the player's distance to a range of 0 to 1, with 1 being at the edge
        float distanceFactor = Mathf.Clamp01(playerDistance / (platformWidth / 2));

        // Calculate the tilt angle based on how far the player is from the center
        float tiltAngle = distanceFactor * maxTiltAngle;

        // Apply the tilt by changing the platform's Z rotation
        transform.rotation = Quaternion.Euler(0, 0, (tiltAngle * Mathf.Sign(player.position.x - platformCenter.x)) * -1f);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the colliding object has the "Player" tag
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.transform;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // Check if the player exits the platform, reset player reference
        if (collision.gameObject.CompareTag("Player"))
        {
            player = null;
        }
    }
}
