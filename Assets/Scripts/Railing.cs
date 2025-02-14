using UnityEngine;

public class Railing : MonoBehaviour
{
    private Collider2D _collider;
    public Collider2D Collider => _collider;
    private bool _canGeneratePoints;
    public bool CanGeneratePoints => _canGeneratePoints;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _canGeneratePoints = true;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent<PlayerRailGrind>(out var playerRailGrind))
        {
            if (Mathf.Abs(other.gameObject.GetComponent<PlayerMovement>().Rb.linearVelocityX) >= playerRailGrind.MinimumSpeedToGrind)
            { 
                Physics2D.IgnoreCollision(other.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>(), false);
                playerRailGrind.EnableRailGrinding(this);     
            }
            else
            {
                Physics2D.IgnoreCollision(other.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent<PlayerRailGrind>(out var playerRailGrind))
        {
            if (playerRailGrind.IsOnRail)
            {
                playerRailGrind.DisableRailGrinding();
                _canGeneratePoints = false;
            }
        }
    }
}
