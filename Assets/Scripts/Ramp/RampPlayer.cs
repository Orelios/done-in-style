using System.Collections;
using UnityEngine;

public class RampPlayer : MonoBehaviour
{
    private PlayerConfigsSO _playerConfigsSO;
    private PlayerGearSwapper _playerGearSwapper;
    private PlayerMovement _playerMovement;
    void Start()
    {

        //NOTE!!! Update to use PlayerMovement script that has PlayerConfigsSO when available


        //_playerConfigsSO = GetComponent<PlayerConfigsSO>();
        _playerGearSwapper = GetComponent<PlayerGearSwapper>();
        _playerMovement = GetComponent<PlayerMovement>();
        normSpeed = _playerMovement.BaseSpeed * _playerGearSwapper.HorizontalMovementMultiplier;
        rampSpeed = normSpeed * _rampSpeedMultiplier;
    }

    // Update is called once per frame
    void Update()
    {
        normSpeed = _playerMovement.BaseSpeed * _playerGearSwapper.HorizontalMovementMultiplier;
        rampSpeed = normSpeed * _rampSpeedMultiplier;
        //Debug.Log("BaseSpeed = " + _playerMovement.BaseSpeed);
    }

    public GameObject rampLeft;    // Left point of the ramp (starting point in one direction)
    public GameObject rampRight;   // Right point of the ramp (starting point in the other direction)

    public bool isOnRamp = false, wasRecentlyOnRamp = false, isGoingUpRamp = false;
    public bool isMovingRight = false;  // Indicates if the player is moving toward the right (RampRight)

    private float _rampCooldownTimer, _rampCooldownDuration = 0.5f;

    [SerializeField] private float _rampMomentumGravity = .01f, _rampSpeedMultiplier = 3f;

    private Vector2 _rampLastVelocity;

    [SerializeField] private bool _isColliding = false;

    // Define normal speed and rampSpeed
    float normSpeed;
    float rampSpeed;

    // When the player enters a ramp trigger (RampLeft or RampRight), determine the direction of movement
    void OnTriggerEnter2D(Collider2D other)
    {
        Ramp ramp = other.transform.parent.GetComponent<Ramp>();
        rampLeft = ramp.leftMarker;
        rampRight = ramp.rightMarker;
        /*
        if (other.gameObject.CompareTag("Player") && other.gameObject.GetComponent<PlayerGearSwapper>().CurrentGearEquipped.DaredevilGearType == EDaredevilGearType.Skateboard)
        {
            //ramp shenangaingis
        }
        */
        if ((other.CompareTag("RampLeft") || other.CompareTag("RampRight")) && wasRecentlyOnRamp == false)
        {
            isOnRamp = true;
            wasRecentlyOnRamp = true;
            if (other.CompareTag("RampLeft"))
            {
                // Player is starting at RampLeft, moving toward RampRight
                isMovingRight = true;
            }
            else if (other.CompareTag("RampRight"))
            {
                // Player is starting at RampRight, moving toward RampLeft
                isMovingRight = false;
            }

            StartCoroutine(IncreaseSpeedOnRamp());
        }

        if (other.CompareTag("GoingUpRamp"))
        {
            isGoingUpRamp = true;
        }
    }

    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        _isColliding = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        _isColliding = false;
    }
    */

    // Coroutine to gradually increase velocity based on the player's position along the ramp
    IEnumerator IncreaseSpeedOnRamp()
    {
        //Debug.Log("Coroutine start");
        // Define the start and end points based on direction
        Vector2 startPosition = isMovingRight ? rampLeft.transform.position : rampRight.transform.position;
        Vector2 endPosition = isMovingRight ? rampRight.transform.position : rampLeft.transform.position;

        // Calculate the full length of the ramp
        float journeyLength = Vector2.Distance(startPosition, endPosition);

        while (isOnRamp)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                isOnRamp = false;
                isGoingUpRamp = false;
                _playerMovement.Rb.gravityScale = _playerMovement.BaseGravity;
                StartCoroutine(RampCooldownTimer());
                break;
            }

            // Increase Gravity to prevent flying only when going down
            if (!isGoingUpRamp)
            {
                _playerMovement.Rb.gravityScale = _playerMovement.BaseGravity * 10;
            }
            else
            {
                _playerMovement.Rb.gravityScale = _playerMovement.BaseGravity;
            }

            // Measure the current distance to the target end (RampRight or RampLeft)
            float distanceToTarget = isMovingRight ?
                Vector2.Distance(transform.position, rampRight.transform.position) :
                Vector2.Distance(transform.position, rampLeft.transform.position);
            //Debug.Log("distanceToTarget = " + distanceToTarget);

            // Calculate how far the player has traveled along the ramp
            float progress = Mathf.InverseLerp(journeyLength, 0, distanceToTarget);
            //Debug.Log("progress = " + progress);

            // Calculate the desired speed based on how far along the ramp the player is
            float targetSpeed = Mathf.Lerp(rampSpeed, normSpeed, progress);

            // Apply the calculated speed to the player's velocity (on the x and y axes of the ramp)
            Vector2 targetVelocity = new Vector2(targetSpeed * (isMovingRight ? 1 : -1), _playerMovement.Rb.linearVelocity.y);
            _playerMovement.Rb.linearVelocity = targetVelocity;
            //Debug.Log("velocity = " + _playerMovement.Rb.linearVelocityX);

            // Stop the coroutine when the player reaches the target (either RampRight or RampLeft)
            if (isMovingRight && Vector2.Distance(transform.position, rampRight.transform.position) < 1f)
            {
                isOnRamp = false;
                isGoingUpRamp = false;
                _playerMovement.Rb.gravityScale = _playerMovement.BaseGravity;
                _rampLastVelocity = _playerMovement.Rb.linearVelocity;
                rampLeft = null;
                rampRight = null;
                StartCoroutine(RampCooldownTimer());
                StartCoroutine(RampPreserveMomentum());
                break;
            }
            else if (!isMovingRight && Vector2.Distance(transform.position, rampLeft.transform.position) < 1f)
            {
                isOnRamp = false;
                isGoingUpRamp = false;
                _playerMovement.Rb.gravityScale = _playerConfigsSO.BaseGravity;
                _rampLastVelocity = _playerMovement.Rb.linearVelocity;
                rampLeft = null;
                rampRight = null;
                StartCoroutine(RampCooldownTimer());
                StartCoroutine(RampPreserveMomentum());
                break;
            }

            // Wait for the next frame
            yield return null;
        }
        Debug.Log("Coroutine ends");
    }

    IEnumerator RampCooldownTimer()
    {
        _rampCooldownTimer = 0;
        while (_rampCooldownTimer < _rampCooldownDuration)
        {
            _rampCooldownTimer += Time.deltaTime;
            yield return null;
        }
        if (_rampCooldownTimer >= _rampCooldownDuration)
        {
            wasRecentlyOnRamp = false;
        }
    }

    IEnumerator RampPreserveMomentum()
    {
        while (!Input.anyKey)
        {
            float lastYVelocity = _playerMovement.Rb.linearVelocity.y;
            _playerMovement.Rb.linearVelocity = new Vector2(_rampLastVelocity.x, (lastYVelocity - _rampMomentumGravity));
            yield return null;
        }
        //_playerMovement.Rb.linearVelocity = new Vector2(0, 0);
        /*if (_playerMovement.IsGrounded())
        {
            _playerMovement.Rb.linearVelocity = new Vector2(0, 0);
        }*/
    }
}
