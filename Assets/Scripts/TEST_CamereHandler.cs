using System;
using Unity.Cinemachine;
using UnityEngine;

public class TEST_CamereHandler : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private CinemachineCamera cameraFollowingPlayer;
    [SerializeField] private float closestOrthographicSize;
    [SerializeField] private float farthestOrthographicSize;
    [SerializeField] private float zoomSmoothing;

    private float _currentZoom;
    private float _zoomVelocity;
    private void Update()
    {
        Zoom(Mathf.Abs(playerMovement.AppliedMovementSpeed));
    }

    private void Zoom(float currentSpeed)
    {
        var targetZoom = Mathf.Lerp(closestOrthographicSize, farthestOrthographicSize, currentSpeed / 10f);
        _currentZoom = Mathf.SmoothDamp(_currentZoom, targetZoom, ref _zoomVelocity, zoomSmoothing);
        cameraFollowingPlayer.Lens.OrthographicSize = _currentZoom;
    }
}
