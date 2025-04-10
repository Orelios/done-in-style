using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int damage; 

    private ScoreCalculator _scoreCalculator;
    private RankCalculator _rankCalculator;
    private TEMP_DodgeTrap _dodgeTrap;
    [SerializeField] private int decreasePoints = 3;
    [SerializeField] private float rps = 60;

    private void Awake()
    {
        _scoreCalculator = FindFirstObjectByType<ScoreCalculator>();
        _rankCalculator = FindFirstObjectByType<RankCalculator>();
        _dodgeTrap = FindFirstObjectByType<TEMP_DodgeTrap>();
    }

    private void Update()
    {
        //transform.Rotate(0, 0, rps * Time.deltaTime); //rotates rps degrees per second around z axis
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*if (collision.gameObject.GetComponent<PlayerInvulnerability>() && !collision.gameObject.GetComponent<PlayerInvulnerability>().IsHit)
        {
            _scoreCalculator.DecreaseScoreInstant(damage);
            _scoreCalculator.GetComponent<RankCalculator>().DecreaseStylishPoints();
            collision.gameObject.GetComponent<PlayerInvulnerability>().DamagePlayer();
        }*/
        
        if (collision.gameObject.TryGetComponent<Player>(out var player) && !player.Invulnerability.IsInvulnerable)
        {
            _dodgeTrap.PlayerDamaged();
            _scoreCalculator.DecreaseScoreInstant(damage);
            _rankCalculator.DecreaseStylishPoints(decreasePoints);
            player.Invulnerability.DamagePlayer();
        }
        
        Destroy(gameObject);
        //else if(!collision.gameObject.TryGetComponent<Bullet>(out _)) { Destroy(gameObject); }
    }
}
