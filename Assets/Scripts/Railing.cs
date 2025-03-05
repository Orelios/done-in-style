using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Railing : MonoBehaviour
{
    private Collider2D _railingCollider;
    public Collider2D Collider => _railingCollider;
    private bool _canGeneratePoints;
    public bool CanGeneratePoints => _canGeneratePoints;
    
    public bool hasTricked = false;
    private bool _canRailGrind;
    //private Collider2D _railingTrigger;
    private Player _player;
    private PlayerTricks _playerTricks;
    public Graffiti graffiti;

    private void Awake()
    {
        _railingCollider = GetComponent<Collider2D>();
        //_railingTrigger = transform.GetChild(0).GetComponent<Collider2D>();
        _canGeneratePoints = true;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        /*if (other.gameObject.TryGetComponent<PlayerRailGrind>(out var playerRailGrind))
        {
           _playerTricks = other.gameObject.GetComponent<PlayerTricks>();
           
            if (other.contacts[0].normal.y > 0 &&  Mathf.Abs(other.gameObject.GetComponent<PlayerMovement>().Rb.linearVelocityX) >= playerRailGrind.MinimumSpeedToGrind)
            { 
                playerRailGrind.EnableRailGrinding(this);   
            }
            else
            {
                Physics2D.IgnoreCollision(other.collider, _railingCollider);
                _canRail = false;
                return;
            }
            
            _playerTricks.DisableCanTrick();
        }*/

        if (other.gameObject.TryGetComponent<Player>(out var player))
        {
            _player = player;
            var contact = other.contacts[0];

            if (contact.normal == Vector2.down && Mathf.Abs(_player.Rigidbody.linearVelocityX) >= _player.RailGrind.MinimumSpeedToGrind)
            {
                Debug.Log("surface hit on top and can grind!");
                _canRailGrind = true;
                _player.RailGrind.EnableRailGrinding(this);
            }
            else
            {
                _canRailGrind = false;
                Debug.Log("nope");
            }
            
            _player.Tricks.DisableCanTrick();
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        /*if (other.gameObject.TryGetComponent<PlayerRailGrind>(out var playerRailGrind))
        {
            Physics2D.IgnoreCollision(other.collider, _railingCollider, false);
            _canRail = true;
            
            if (playerRailGrind.IsOnRail)
            {
                playerRailGrind.DisableRailGrinding();
                _canGeneratePoints = false;
                
                if (hasTricked != true)
                {
                    _playerTricks.EnableTrick(gameObject);
                }
            }
        }*/
        if (other.gameObject.TryGetComponent<Player>(out var player))
        {
            if (player.RailGrind.IsOnRail)
            {
                player.RailGrind.DisableRailGrinding();
                _canGeneratePoints = false;
            }
            
            if (hasTricked != true && _canRailGrind)
            {
                _player.Tricks.EnableTrick(gameObject);
            }
            
            _canRailGrind = true;
            _player = null;
        }
    }
}
