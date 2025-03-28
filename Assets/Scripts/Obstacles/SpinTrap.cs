using UnityEngine;

public class SpinTrap : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private float rotationSpeed;
    [SerializeField] private bool clockwiseRotation;
    //[SerializeField] private float iFramesTime;
    //private 

    [Header("Damage Components")]
    [SerializeField] private ScoreCalculator scoreCalculator;
    [SerializeField] private int damage;
    [SerializeField] private int decreasePoints;
    private float _rotZ; 
    
    private ScoreCalculator _scoreCalculator;
    private RankCalculator _rankCalculator;
    [SerializeField] private TEMP_DodgeTrap _dodgeTrap;

    private void Awake()
    {
        _scoreCalculator = FindFirstObjectByType<ScoreCalculator>();
        _rankCalculator = FindFirstObjectByType<RankCalculator>();
    }
    
    void Update()
    {
        Rotate(); 
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out var player) && !player.Invulnerability.IsInvulnerable)
        {
            _dodgeTrap.PlayerDamaged();
            _scoreCalculator.DecreaseScoreInstant(damage);
            _rankCalculator.DecreaseStylishPoints(decreasePoints);
            player.Invulnerability.DamagePlayer();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*if (collision.gameObject.GetComponent<PlayerInvulnerability>() && !collision.gameObject.GetComponent<PlayerInvulnerability>().IsHit)
        {
            scoreCalculator.DecreaseScoreInstant(damage);
            scoreCalculator.GetComponent<RankCalculator>().DecreaseStylishPoints();
            collision.gameObject.GetComponent<PlayerInvulnerability>().DamagePlayer();
        }*/
        
        if (collision.gameObject.TryGetComponent<Player>(out var player) && !player.Invulnerability.IsInvulnerable)
        {
            _dodgeTrap.PlayerDamaged();
            _scoreCalculator.DecreaseScoreInstant(damage);
            _rankCalculator.DecreaseStylishPoints(decreasePoints);
            player.Invulnerability.DamagePlayer();
        }
    }

    private void Rotate()
    {
        if (!clockwiseRotation)
        {
            _rotZ += Time.deltaTime * rotationSpeed;
        }
        else
        {
            _rotZ += -Time.deltaTime * rotationSpeed;
        }

        transform.rotation = Quaternion.Euler(0, 0, _rotZ); 
    }
}
