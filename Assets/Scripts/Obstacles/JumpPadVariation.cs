using UnityEngine;

public class JumpPadVariation : MonoBehaviour
{
    [Header("Jump Pad Variation components")]
    [SerializeField] private Transform target;
    [SerializeField] private float bounceHeight;
    [SerializeField] private float maxHeight;

    public bool hasTricked = false;
    private bool _hasGivenScore = false;
    public Graffiti graffiti;
    //public bool isOnJumpPad = false;

    private PlayerTricks _playerTricks;

    private Vector2 _direction;
    private void Update()
    {
        FindTrajectory();
    }

    private void FindTrajectory()
    {
        Vector2 targetPos = target.position;
        _direction = targetPos - (Vector2)transform.position;

        //RaycastHit2D hit = Physics2D.Raycast(transform.position, _direction, bouncePadRange, LayerMask.GetMask(targetLayerName));
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        collision.gameObject.GetComponent<Rigidbody2D>().AddForce(_direction * bounceHeight, ForceMode2D.Impulse);
        Debug.Log(_direction * bounceHeight);

        collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity =
                Vector2.ClampMagnitude(collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity, maxHeight);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.SpringBoard, this.transform.position);

        if (collision.gameObject.TryGetComponent<PlayerTricks>(out var playerTricks))
        {
            _playerTricks = playerTricks;
            if (!playerTricks.isOnJumpPad)
            {
                playerTricks.isOnJumpPad = true;
                _playerTricks.CallJumpPadTimer();
            }
        }

        if (!_hasGivenScore)
        {
            collision.gameObject.GetComponent<PlayerTricks>().AddScoreAndRank();
            _hasGivenScore = true;
            if (graffiti != null)
            {
                graffiti.StartGraffiti();
            }
        }
        if (!hasTricked)
        {
            collision.gameObject.GetComponent<PlayerTricks>().DisableCanTrick();
            collision.gameObject.GetComponent<PlayerTricks>().EnableTrick(gameObject);
        }

    }
}
