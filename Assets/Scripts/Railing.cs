using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Railing : MonoBehaviour
{
    public List<Vector2> ColliderPoints = new();
    private EdgeCollider2D _collider;
    public bool hasTricked = false;
    private bool _hasGivenScore = false;
    [SerializeField] private float maxScoredTime = 5f, scoredIntervals = 1f;
    private PlayerTricks _playerTricks;

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
            _playerTricks = other.gameObject.GetComponent<PlayerTricks>();
            if (Mathf.Abs(other.gameObject.GetComponent<PlayerMovement>().Rb.linearVelocityX) > playerRailGrind.MinimumSpeedToGrind)
            { 
                Physics2D.IgnoreCollision(other.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>(), false);
                playerRailGrind.EnableRailing(this);
                if (!_hasGivenScore)
                {
                    _playerTricks.AddScoreAndRank(); //Add once for entering
                    StartCoroutine(ScoredTime()); //Add every scoreInterval
                    _hasGivenScore = true;
                }
            }
            else
            {
                Physics2D.IgnoreCollision(other.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            }
            
            _playerTricks.DisableCanTrick();
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && other.gameObject.TryGetComponent<PlayerRailGrind>(out var playerRailGrind))
        {
            if (playerRailGrind.IsOnRail)
            {
                playerRailGrind.DisableRailing();
                StopCoroutine(ScoredTime());
                if (hasTricked != true)
                {
                    _playerTricks.EnableTrick(gameObject);
                }
            }
        }
    }

    private IEnumerator ScoredTime()
    {
        float scoredTimeEnd = Time.time + maxScoredTime;
        while (Time.time < scoredTimeEnd)
        {
            yield return new WaitForSeconds(scoredIntervals);
            _playerTricks.AddScoreAndRank();
        }
    }
}
