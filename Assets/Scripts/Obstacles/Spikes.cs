using System;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private float BounceHeight;
    [SerializeField] private int damage;
    
    private ScoreCalculator _scoreCalculator;
    private RankCalculator _rankCalculator;

    private void Awake()
    {
        _scoreCalculator = FindFirstObjectByType<ScoreCalculator>();
        _rankCalculator = FindFirstObjectByType<RankCalculator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*if (collision.gameObject.GetComponent<PlayerInvulnerability>() && !collision.gameObject.GetComponent<PlayerInvulnerability>().IsHit)
        {
            _scoreCalculator.DecreaseScoreInstant(damage);
            scoreCalculator.GetComponent<RankCalculator>().DecreaseStylishPoints();
            collision.gameObject.GetComponent<PlayerInvulnerability>().PlayerHit();
        }*/

        if (collision.gameObject.TryGetComponent<Player>(out var player) && !player.Invulnerability.IsInvulnerable)
        {
            _scoreCalculator.DecreaseScoreInstant(damage);
            _rankCalculator.DecreaseStylishPoints();
            player.Invulnerability.DamagePlayer();
        }
    }
}
