using System;
using System.Collections;
using System.Threading.Tasks;
using Unity.Cinemachine;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CinemachineCamera cameraFollowingPlayer;
    [SerializeField] private Player player;
    
    [Header("Camera Zoom Configs")] 
    [SerializeField] private float maxZoomIn;
    [SerializeField] private float maxZoomOut;
    [SerializeField] private float zoomOutSpeed;
    [SerializeField] private float zoomInSpeed;
    [SerializeField] private float idleTimeThreshold;
    
    private float _targetZoom;
    private float _currentZoom;
    private float _smoothing;
    private float _idleTime;
    private bool _isMoving;

    private void Start()
    {
        if (cameraFollowingPlayer != null)
        {
            _currentZoom = cameraFollowingPlayer.Lens.OrthographicSize;
        }
    }

    private void Update()
    {
        _isMoving = Mathf.Abs(player.Movement.Rb.linearVelocityX) > 0.1f || Mathf.Abs(player.Movement.Rb.linearVelocityY) > 0.1f;
        _idleTime = _isMoving ? 0f : _idleTime += Time.deltaTime;
        
        _currentZoom = _idleTime >= idleTimeThreshold ? ZoomIn() : ZoomOut(_isMoving);
        
        cameraFollowingPlayer.Lens.OrthographicSize = _currentZoom;
    }

    private float ZoomIn()
    {
        return Mathf.SmoothDamp(_currentZoom, maxZoomIn, ref _smoothing, zoomInSpeed);
    }
    
    private float ZoomOut(bool isMoving)
    {
        var targetZoom = isMoving ? maxZoomOut : _currentZoom;
        return Mathf.SmoothDamp(_currentZoom, targetZoom, ref _smoothing, zoomOutSpeed);
    }
}
