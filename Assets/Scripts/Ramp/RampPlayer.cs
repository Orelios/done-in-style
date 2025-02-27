using System.Collections;
using UnityEngine;

public class RampPlayer : MonoBehaviour
{
    private GameObject rampLeft;    // Left point of the ramp (starting point in one direction)
    private GameObject rampRight;   // Right point of the ramp (starting point in the other direction)

    private Vector2 startPosition, endPosition;

    private bool isOnRamp = false, wasRecentlyOnRamp = false, isGoingUpRamp = false, hasExitedRamp = true;
    public bool HasExitedRamp => hasExitedRamp;

    [HideInInspector] public bool isRamping = false;
    private bool isMovingRight = false;  // Indicates if the player is moving toward the right (RampRight)

    private float _rampCooldownTimer, _rampCooldownDuration = 1f;

    [SerializeField] private float _rampMomentumGravity = .01f, _rampSpeedStartMultiplier = 2f, _rampSpeedEndMultiplier = 3f;

    private Vector2 _rampLastVelocity;

    private bool _isColliding = false;
    public bool IsColliding => _isColliding;

    float normSpeed;
    float rampSpeed;    
    private PlayerConfigsSO _playerConfigsSO;
    private PlayerMovement _playerMovement;
    private PlayerTricks _playertricks;
    //public bool hasTricked = false;
    [HideInInspector] public Ramp ramp;
    private VFXManager _vfx;
    void Start()
    {

        //NOTE!!! Update to use PlayerMovement script that has PlayerConfigsSO when available
        //Disable movement when isRamping == true in PlayerMovement script (if using new new version)


        //_playerConfigsSO = GetComponent<PlayerConfigsSO>();
        _playertricks = GetComponent<PlayerTricks>();
        _playerMovement = GetComponent<PlayerMovement>();
        normSpeed = _playerMovement.BaseSpeed;
        rampSpeed = normSpeed * _rampSpeedEndMultiplier;
        _vfx = GetComponentInChildren<VFXManager>();
    }

    // Update is called once per frame
    void Update()
    {
        normSpeed = _playerMovement.BaseSpeed;
        rampSpeed = normSpeed * _rampSpeedEndMultiplier;
    }

    // When the player enters a ramp trigger (RampLeft or RampRight), determine the direction of movement
    void OnTriggerEnter2D(Collider2D other)
    {
        
        /*
        if (other.gameObject.CompareTag("Player") && other.gameObject.GetComponent<PlayerGearSwapper>().CurrentGearEquipped.DaredevilGearType == EDaredevilGearType.Skateboard)
        {
            //ramp shenangaingis
        }
        */
        
        if ((other.CompareTag("RampLeft") || other.CompareTag("RampRight")) && !wasRecentlyOnRamp && !isRamping && hasExitedRamp)
        {
            /*
            if (other.transform.parent.TryGetComponent<Ramp>(out var varRamp))
            {
                ramp = varRamp;
            }
            if (other.transform.parent.TryGetComponent<RampRail>(out var varRampRail))
            {
                ramp = varRampRail;
            }
            */
            ramp = other.transform.parent.GetComponent<Ramp>();
            rampLeft = ramp.leftMarker;
            rampRight = ramp.rightMarker;

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

        if (other.CompareTag("RampExit"))
        {
            hasExitedRamp = true;
            //_vfx.CallJumpingVFX();
            _vfx.CallGroundPoundLandVFX();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("RampSurface"))
        {
            isOnRamp = true;
            hasExitedRamp = false;
            isGoingUpRamp = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("RampSurface"))
        {
            isOnRamp = false;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        _isColliding = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        _isColliding = false;
    }
    

    // Coroutine to gradually increase velocity based on the player's position along the ramp
    IEnumerator IncreaseSpeedOnRamp()
    {
        isGoingUpRamp = false;

        // Define the start and end points based on direction
        //Vector2 startPosition = isMovingRight ? rampLeft.transform.position : rampRight.transform.position;
        //Vector2 endPosition = isMovingRight ? rampRight.transform.position : rampLeft.transform.position;

        if (ramp.isRampRail)
        {
            startPosition = isMovingRight ? ramp.leftTarget.transform.position : ramp.rightTarget.transform.position;
            endPosition = isMovingRight ? ramp.rightTarget.transform.position : ramp.leftTarget.transform.position;
        }
        else if (!ramp.isRampRail)
        {
            startPosition = isMovingRight ? rampLeft.transform.position : rampRight.transform.position;
            endPosition = isMovingRight ? rampRight.transform.position : rampLeft.transform.position;
        }

        // Calculate the full length of the ramp
        float journeyLength = Vector2.Distance(startPosition, endPosition);

        isRamping = true;

        while (isRamping)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                isRamping = false;
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

            // Calculate how far the player has traveled along the ramp
            float progress = Mathf.InverseLerp(journeyLength, 0, distanceToTarget);

            // Calculate the desired speed based on how far along the ramp the player is
            float targetSpeed = Mathf.Lerp((normSpeed * _rampSpeedStartMultiplier), rampSpeed, progress);

            // Apply the calculated speed to the player's velocity (on the x and y axes of the ramp)
            Vector2 targetVelocity = new Vector2(targetSpeed * (isMovingRight ? 1 : -1), _playerMovement.Rb.linearVelocity.y);
            _playerMovement.Rb.linearVelocity = targetVelocity;

            // Stop the coroutine when the player reaches the target (either RampRight or RampLeft)
            if (isMovingRight && Vector2.Distance(transform.position, rampRight.transform.position) < 1f)
            {
                isRamping = false;
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
                isRamping = false;
                isGoingUpRamp = false;
                _playerMovement.Rb.gravityScale = _playerMovement.BaseGravity;
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
        isRamping = false;
        if (ramp.hasGivenScore == false)
        {
            _playertricks.AddScoreAndRank();
            ramp.hasGivenScore = true;
        }
        if (ramp.hasTricked != true)
        {
            _playertricks.EnableTrick(ramp.gameObject);
        }
        
        StartCoroutine(RampCooldownTimer());
        //Debug.Log("Coroutine ends");
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
        //Debug.Log("Cooldown ends");
    }

    IEnumerator RampPreserveMomentum()
    {
        while (!Input.anyKey)
        {
            if (_isColliding && _rampCooldownTimer >= .1f) { break; }
            float lastYVelocity = _playerMovement.Rb.linearVelocity.y;
            _playerMovement.Rb.linearVelocity = new Vector2(_rampLastVelocity.x, (lastYVelocity - _rampMomentumGravity));
            yield return null;
        }
        //Debug.Log("Momentum ends");
    }
}
