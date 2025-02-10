using System;
using TMPro;
using UnityEngine;

public class PlayerDebugVisualization : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentSpeedText;
    [SerializeField] private TextMeshProUGUI accelerationText;
    [SerializeField] private TextMeshProUGUI maxSpeedText;
    
    private PlayerMovement _playerMovement;
    private Rigidbody2D _rigidbody2D;

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        UpdateMovementDebugUI();
    }

    private void UpdateMovementDebugUI()
    {
        currentSpeedText.text = $"Current Speed: {Mathf.Abs(_rigidbody2D.linearVelocityX):n3}";
        accelerationText.text = $"Acceleration: {_playerMovement.AppliedAcceleration:n1}";
        maxSpeedText.text = $"Max Speed: {_playerMovement.AppliedMaxMovementSpeed}";
    }
}
