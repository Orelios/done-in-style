using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    // Editable variables in Inspector
    [SerializeField] private Vector2 targetPos;  // Target position to move towards
    [SerializeField] private float speed = 2f;   // Speed of the platform movement
    [SerializeField] private float moveDelay = 0.5f; //Delay before platform moves back after reaching destination

    private Transform player;
    private Vector2 currentPos;  // The current position of the platform
    private float timeToMoveBack; // Time tracker to wait before moving back

    private bool isMovingToTarget = true; // Flag to check direction of movement

    // Start is called before the first frame update
    void Start()
    {
        // Register the initial position of the platform as currentPos
        currentPos = transform.position;

        if (targetPos == Vector2.zero) // If targetPos is not set, log a warning
        {
            Debug.LogWarning("Target position for " + gameObject.name + " is not set in the Inspector!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (targetPos == Vector2.zero) return; // Do nothing if no target position

        if (isMovingToTarget)
        {
            // Move the platform towards the target position
            float step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, targetPos, step);

            // If the platform reaches the target position
            if ((Vector2)transform.position == targetPos)
            {
                // Swap currentPos and targetPos
                Vector2 temp = currentPos;
                currentPos = targetPos;
                targetPos = temp;

                // Start a timer before moving back
                timeToMoveBack = Time.time + moveDelay;
                isMovingToTarget = false; // Stop moving towards target
            }
        }
        else
        {
            // Check if it's time to start moving back after delay
            if (Time.time >= timeToMoveBack)
            {
                isMovingToTarget = true; // Start moving to the new target
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth)
            && collision.gameObject.GetComponent<PlayerInputManager>().HorizontalMovement == 0)
        {
            collision.transform.SetParent(transform); // Attach player to platform when standing on it
            Debug.Log("working");
            //collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        }
        if(collision.gameObject.GetComponent<PlayerInputManager>().HorizontalMovement != 0)
        {
            collision.transform.SetParent(null);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth))
        {
            collision.transform.SetParent(null); // Detach player from platform when they leave it
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
