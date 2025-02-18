using System;
using System.Linq;
using TMPro;
using UnityEngine;

[DefaultExecutionOrder(10)]
public class PlayerDebugVisualization : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentSuperStateText;
    [SerializeField] private TextMeshProUGUI currentSubStateText;
    [SerializeField] private TextMeshProUGUI currentSpeedText;
    [SerializeField] private TextMeshProUGUI accelerationText;
    [SerializeField] private TextMeshProUGUI maxSpeedText;
    
    /*private PlayerMovement _playerMovement;
    private Rigidbody2D _rigidbody2D;*/
    private Player _player;

    private void Awake()
    {
        /*_playerMovement = GetComponent<PlayerMovement>();
        _rigidbody2D = GetComponent<Rigidbody2D>();*/
        
        _player = GetComponent<Player>();
    }

    private void Update()
    {
        UpdateMovementDebugUI();
    }

    private void UpdateMovementDebugUI()
    {
        currentSuperStateText.text = $"Super State: {string.Concat(_player.CurrentSuperState.Name.Select(x => char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' ')}";
        currentSubStateText.text = $"Sub State: {string.Concat(_player.CurrentSubState.Name.Select(x => char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' ')}";
        currentSpeedText.text = $"Speed: {Mathf.Abs(_player.Rigidbody.linearVelocityX):n2}";
        accelerationText.text = $"Acceleration: {_player.Movement.AppliedAcceleration:n1}";
        maxSpeedText.text = $"Max Speed: {_player.Movement.AppliedMaxMovementSpeed}";
    }
}
