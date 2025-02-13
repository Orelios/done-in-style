using System;
using System.Collections.Generic;
using UnityEngine;

public class Railing : MonoBehaviour
{
    public List<Vector2> ColliderPoints = new();
    private EdgeCollider2D _collider;
    public bool hasTricked = false;
    private bool _hasGivenScore = false;

    private void Awake()
    {
        _collider = GetComponent<EdgeCollider2D>();
        
        foreach (var points in _collider.points)
        {
            ColliderPoints.Add(points);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && other.gameObject.TryGetComponent<PlayerRailGrind>(out var playerRailGrind))
        {
            if (Mathf.Abs(other.gameObject.GetComponent<PlayerMovement>().Rb.linearVelocityX) > playerRailGrind.MinimumSpeedToGrind)
            { 
                Physics2D.IgnoreCollision(other.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>(), false);
                playerRailGrind.EnableRailing(this);
            }
            else
            {
                Physics2D.IgnoreCollision(other.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            }
            if (!_hasGivenScore)
            {
                other.gameObject.GetComponent<PlayerTricks>().AddScoreAndRank();
                _hasGivenScore = true;
            }
            other.gameObject.GetComponent<PlayerTricks>().DisableCanTrick();
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && other.gameObject.TryGetComponent<PlayerRailGrind>(out var playerRailGrind))
        {
            if (playerRailGrind.IsOnRail)
            {
                playerRailGrind.DisableRailing();
                if (hasTricked != true)
                {
                    other.gameObject.GetComponent<PlayerTricks>().EnableTrick(gameObject);
                }
            }
        }
    }
}
