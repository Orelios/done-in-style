using System;
using System.Collections;
using UnityEngine;

public class CameraFollowAnchor : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;

    [SerializeField] private float flipTime;
    
    private Coroutine _flipCameraCoroutine;
    private PlayerMovement _playerMovement;
    private bool _isFacingRight;

    private void Awake()
    {
        _playerMovement = playerTransform.gameObject.GetComponent<PlayerMovement>();
        _isFacingRight = _playerMovement.IsFacingRight;
    }

    private void Update()
    {
        //anchor follows player position
        transform.position = playerTransform.position;
    }

    public void FlipCamera()
    {
        _flipCameraCoroutine = StartCoroutine(FlipLerpCamera());
    }

    private IEnumerator FlipLerpCamera()
    {
        float startRotation = transform.localEulerAngles.y;
        float endRotation = DetermineEndRotation();

        float elapsedTime = 0f;
        while (elapsedTime < flipTime)
        {
            elapsedTime += Time.deltaTime;
            var yRotation = Mathf.Lerp(startRotation, endRotation, elapsedTime / flipTime);
            transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
            
            yield return null;
        }
    }

    private float DetermineEndRotation()    
    {
        _isFacingRight = !_isFacingRight;
        
        return _isFacingRight == true ? 0 : 180f;
    }
}
