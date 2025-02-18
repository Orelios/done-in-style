using System.Collections;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    private PlayerInputManager _playerInputManager;
    private float movingSpeedVFX = 5f;
    public GameObject moving, landing, jumping, hurt;
    private ParticleSystem movingVFX, landingVFX, jumpingVFX, hurtVFX;
    [SerializeField] private bool isWaitingToLand = true, isWaitingToJump = false;
    void Start()
    {
        _playerMovement = transform.GetComponentInParent<PlayerMovement>();
        _playerInputManager = transform.GetComponentInParent<PlayerInputManager>();
        movingVFX = moving.GetComponent<ParticleSystem>();
        landingVFX = landing.GetComponent<ParticleSystem>();
        jumpingVFX = jumping.GetComponent<ParticleSystem>();
        hurtVFX = hurt.GetComponent<ParticleSystem>();
        landingVFX.Stop();
        jumpingVFX.Stop();
        hurtVFX.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        #region Moving
        //if (moving.activeSelf == false)
        //if (movingVFX.emission.enabled == false)
        if (!movingVFX.isPlaying)
        {
            if (Mathf.Abs(_playerMovement.Rb.linearVelocityX) >= movingSpeedVFX && _playerMovement.IsGrounded())
            {
                //moving.SetActive(true);
                movingVFX.Play();
            }
        }
        //else if (movingVFX.emission.enabled)
        else if (movingVFX.isPlaying)
        {
            if (Mathf.Abs(_playerMovement.Rb.linearVelocityX) < movingSpeedVFX || _playerMovement.IsGrounded() == false)
            {
                //moving.SetActive(false);
                movingVFX.Stop();
            }
        }
        #endregion
        //if (_playerInputManager.IsJumping)
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
        //yield return null;
        /*if (_playerMovement.IsGrounded())
        {
            landingVFX.Play();
            isWaitingToLand = true;
            Debug.Log("Landed");
        }*/
        while (!_playerMovement.IsGrounded())
        {
            yield return null;
        }
        landingVFX.Play();
        isWaitingToLand = true;
        Debug.Log("Landed");
    }

    private IEnumerator DetectJump()
    {
        //yield return null;
        /*if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            jumpingVFX.Play();
            isWaitingToJump = false;
            Debug.Log("Jumped");
        }
        */
        while (!Input.GetKeyDown(KeyCode.UpArrow))
        {
            yield return null;
        }
        jumpingVFX.Play();
        isWaitingToJump = false;
        Debug.Log("Jumped");
    }

    public void CallHurtVFX()
    {
        hurtVFX.Play();
    }
}
