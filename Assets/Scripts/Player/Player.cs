using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    //TODO: set up all the common variables/components/functions needed for all Player scripts here
    private SpriteRenderer _playerSprite;
    public SpriteRenderer Sprite => _playerSprite;
    private Rigidbody2D _playerRb;
    public Rigidbody2D Rigidbody => _playerRb;
    private Collider2D _playerCollider;
    public Collider2D Collider => _playerCollider;
    private Animator _playerAnimator;
    public Animator Animator => _playerAnimator;
    
    private void Awake()
    {
        _playerSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        _playerRb = GetComponent<Rigidbody2D>();
        _playerCollider = GetComponent<Collider2D>();
        _playerAnimator = GetComponent<Animator>();
    }
}
