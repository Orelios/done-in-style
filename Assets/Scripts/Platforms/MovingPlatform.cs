using System.Collections;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    // Editable variables in Inspector
    [SerializeField] private Vector2 targetPos;  // Target position to move towards
    [SerializeField] private float speed = 2f;   // Speed of the platform movement
    [SerializeField] private float moveDelay = 0.5f; //Delay before platform moves back after reaching destination

    private Transform player;

    // The current position will be automatically set to the platform's initial position in the scene
    private Vector2 currentPos;

    // Start is called before the first frame update
    void Start()
    {
        // Register the initial position of the platform as currentPos
        currentPos = transform.position;

        // Start the coroutine that handles the movement between the two positions
        if (targetPos != Vector2.zero) // If targetPos is set, begin the movement
        {
            StartCoroutine(MovePlatform());
        }
        else
        {
            Debug.LogWarning("Target position for " + gameObject.name + " is not set in the Inspector!");
        }
    }

    // Coroutine to move the platform between currentPos and targetPos infinitely
    private IEnumerator MovePlatform()
    {
        while (true)
        {
            // Move from currentPos to targetPos
            float journeyLength = Vector2.Distance(currentPos, targetPos);
            float startTime = Time.time;

            while (Vector2.Distance(transform.position, targetPos) > 0.1f)
            {
                float distanceCovered = (Time.time - startTime) * speed;
                float fractionOfJourney = distanceCovered / journeyLength;
                transform.position = Vector2.Lerp(currentPos, targetPos, fractionOfJourney);
                yield return null;
            }

            // Once the platform reaches the target position, swap currentPos and targetPos
            Vector2 temp = currentPos;
            currentPos = targetPos;
            targetPos = temp;

            // Wait for a short moment before moving again (optional)
            yield return new WaitForSeconds(moveDelay);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth))
        {
            collision.transform.SetParent(transform); // Attach player to platform
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth))
        {
            collision.transform.SetParent(null); // Detach player from platform
        }
    }
    /*
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the colliding object has the "Player" tag
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.transform;

            player.transform.parent = transform;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // Check if the colliding object has the "Player" tag
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.transform;

            player.transform.parent = null;
        }
    }
    */
}
