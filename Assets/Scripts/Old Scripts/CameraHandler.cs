using System;
using System.Collections;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [Header("Components")]
    [Tooltip("Attach here the Camera used to follow the Player; must be a Cinemachine Position Composer")]
    [SerializeField] private CinemachinePositionComposer cameraFollowingPlayer;
    [Tooltip("Attach here the PlayerMovement script from the Player")]
    [SerializeField] private PlayerMovement playerMovement;
    
    [Header("Camera Configs")]
    [Tooltip("Insert here the Camera offset when the Player is currently grounded")]
    [SerializeField] private Vector3 groundedCameraOffset;
    [Tooltip("Insert here the Camera offset when the Player is currently falling")]
    [SerializeField] private Vector3 fallingCameraOffset;
    [Tooltip("Insert here how long the Camera pans in between the offset values")]
    [SerializeField] private float offsetPanTime;
    [Tooltip("Insert here the minimum Y Velocity before the camera starts to pan; set as negative to denote falling")]
    [SerializeField]  private float yVelocityThreshold;
    public float YVelocityThreshold {get => yVelocityThreshold;  set => yVelocityThreshold = value;}
    
    public bool IsPanningCoroutineActive { get; private set; }

    public void LerpCameraPanning(bool isPlayerFalling)
    {
        StartCoroutine(LerpCameraPanningRoutine(isPlayerFalling));
    }

    private IEnumerator LerpCameraPanningRoutine(bool isPlayerFalling)
    {
        IsPanningCoroutineActive = true;

        float startingYOffset = cameraFollowingPlayer.TargetOffset.y;
        float endingYOffset = 0;

        endingYOffset = isPlayerFalling ? fallingCameraOffset.y : groundedCameraOffset.y;
        
        float elapsedTime = 0;
        while (elapsedTime < offsetPanTime)
        {
            elapsedTime += Time.deltaTime;
            
            float panning = Mathf.Lerp(startingYOffset, endingYOffset, (elapsedTime / offsetPanTime));
            cameraFollowingPlayer.TargetOffset = new Vector3(cameraFollowingPlayer.TargetOffset.x, panning, cameraFollowingPlayer.TargetOffset.z);
            
            yield return null;
        }
        
        IsPanningCoroutineActive = false;
    }
}
