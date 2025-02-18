using System.Collections;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    private PlayerInputManager _playerInputManager;
    private float movingSpeedVFX = 5f;
    public GameObject moving, landing;
    private ParticleSystem movingVFX, landingVFX;
    private bool isWaitingToLand = false;
    void Start()
    {
        _playerMovement = transform.GetComponentInParent<PlayerMovement>();
        _playerInputManager = transform.GetComponentInParent<PlayerInputManager>();
        movingVFX = moving.GetComponent<ParticleSystem>();
        landingVFX = landing.GetComponent<ParticleSystem>();
        landingVFX.Stop();
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
        if (!_playerMovement.IsGrounded() && !isWaitingToLand)
        {
            isWaitingToLand = true;
            StartCoroutine(DetectLanding());
        }

    }

    private IEnumerator DetectLanding()
    {
        //yield return new WaitForSeconds(0.1f);
        yield return null;
        if (_playerMovement.IsGrounded())
        {
            landingVFX.Play();
        }
        isWaitingToLand = false;
        //yield return null;
        //yield return new WaitForSeconds(0.5f);
    }
}
