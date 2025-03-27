using System.Collections;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    private PlayerInputManager _playerInputManager;
    private PlayerTricks _playerTricks;
    private PlayerRailing _rail;
    private JumpPad _jumpPad;
    private float movingSpeedVFX = 5f;
    public GameObject moving, landing, jumping, springBoard, hurt, groundPoundLand, groundPoundDive, dash, doubleJump, rail, wallRiding;
    private ParticleSystem movingVFX, landingVFX, jumpingVFX, springBoardVFX, hurtVFX, groundPoundLandVFX, groundPoundDiveVFX, dashVFX, doubleJumpVFX, railVFX, wallRidingVFX;
    [SerializeField] private bool isWaitingToLand = true, isWaitingToJump = false, isOnRailOrSpringboard = false;
    void Start()
    {
        _playerMovement = transform.GetComponentInParent<PlayerMovement>();
        _playerInputManager = transform.GetComponentInParent<PlayerInputManager>();
        _playerTricks = transform.GetComponentInParent<PlayerTricks>();
        _rail = transform.GetComponentInParent<PlayerRailing>();
        _jumpPad = transform.GetComponentInParent<JumpPad>();

        movingVFX = moving.GetComponent<ParticleSystem>();
        landingVFX = landing.GetComponent<ParticleSystem>();
        jumpingVFX = jumping.GetComponent<ParticleSystem>();
        springBoardVFX = springBoard.GetComponent<ParticleSystem>();
        hurtVFX = hurt.GetComponent<ParticleSystem>();
        groundPoundLandVFX = groundPoundLand.GetComponent<ParticleSystem>();
        groundPoundDiveVFX = groundPoundDive.GetComponent<ParticleSystem>();
        dashVFX = dash.GetComponent<ParticleSystem>();
        doubleJumpVFX = doubleJump.GetComponent<ParticleSystem>();
        railVFX = rail.GetComponent<ParticleSystem>();
        wallRidingVFX = wallRiding.GetComponent<ParticleSystem>();

        landingVFX.Stop();
        jumpingVFX.Stop();
        springBoardVFX.Stop();
        hurtVFX.Stop();
        groundPoundLandVFX.Stop();
        groundPoundDiveVFX.Stop();
        dashVFX.Stop();
        doubleJumpVFX.Stop();
        railVFX.Stop();
        wallRidingVFX.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        MovementUpdate();
        GroundAndJumpCheckers();
    }

    private void MovementUpdate()
    {
        if (_rail.IsMovingOnRail && _playerMovement.PlayerOnRailing())
        {
            if (!railVFX.isPlaying) { railVFX.Play(); }
            if (wallRidingVFX.isPlaying) { wallRidingVFX.Stop(); }
            if (movingVFX.isPlaying) { movingVFX.Stop(); }
        }
        else if (_playerTricks.IsWallRiding && !_playerTricks.CanDestroy)
        {
            if (!wallRidingVFX.isPlaying) {  wallRidingVFX.Play(); }
            if (railVFX.isPlaying) { railVFX.Stop(); }
        }
        else
        {
            if (railVFX.isPlaying) { railVFX.Stop(); }
            if (wallRidingVFX.isPlaying) { wallRidingVFX.Stop(); }

            if (!movingVFX.isPlaying)
            {
                if (Mathf.Abs(_playerMovement.Rb.linearVelocityX) >= movingSpeedVFX && _playerMovement.IsGrounded())
                {
                    movingVFX.Play();
                }
            }
            else if (movingVFX.isPlaying)
            {
                if (Mathf.Abs(_playerMovement.Rb.linearVelocityX) < movingSpeedVFX || _playerMovement.IsGrounded() == false)
                {
                    movingVFX.Stop();
                }
            }
        }
    }


    #region Ground and Jump
    private void GroundAndJumpCheckers()
    {
        if (!_playerMovement.IsGrounded() && isWaitingToLand)
        {
            //Debug.Log("onAir");
            isWaitingToLand = false;
            StartCoroutine(DetectLanding());
        }

        if (_playerMovement.IsGrounded() && !isWaitingToJump)
        {
            //Debug.Log("onGround");
            isWaitingToJump = true;
            StartCoroutine(DetectJump());
            //Debug.Log("Detect Jump");
        }
    }

    private IEnumerator DetectLanding()
    {
        while (!_playerMovement.IsGrounded())
        {
            yield return null;
        }
        if (!isOnRailOrSpringboard)
        {
            landingVFX.Play();
        }
        isWaitingToLand = true;
        //Debug.Log("Landed");
    }

    private IEnumerator DetectJump()
    {
        while (!Input.GetKeyDown(KeyCode.UpArrow) && _playerMovement.IsGrounded())
        {
            yield return null;
        }
        if (_playerTricks.isOnJumpPad)
        {
            springBoardVFX.Play();
            isWaitingToJump = false;
            //Debug.Log("SpringBoard");
        }
        else if (!_playerTricks.isOnJumpPad)//onAir but not on JumpPad/SpringBoard
        {
            jumpingVFX.Play();
            isWaitingToJump = false;
            //Debug.Log("Jumped");
        }
    }
    #endregion

    #region Colliders
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Rail") || collision.gameObject.CompareTag("Springboard"))
        {
            isOnRailOrSpringboard = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Rail") || collision.gameObject.CompareTag("Springboard"))
        {
            isOnRailOrSpringboard = false;
        }
    }
    #endregion

    #region Call Functions
    public void CallHurtVFX()
    {
        hurtVFX.Play();
    }

    public void CallGroundPoundLandVFX()
    {
        groundPoundLandVFX.Play();
    }

    public void CallGroundPoundDiveVFX()
    {
        groundPoundDiveVFX.Play();
    }

    public void CallDashVFX()
    {
        dashVFX.Play();
    }

    public void CallDoubleJumpVFX()
    {
        doubleJumpVFX.Play();
    }

    public void CallJumpingVFX()
    {
        jumpingVFX.Play();
    }

    public void SetOnRailOrSpringboard(bool b)
    {
        isOnRailOrSpringboard = b;
    }

    #endregion
}