using UnityEngine;

public class MovePoint : MonoBehaviour
{
    public int index;
    private PlayerMovement _playerMovement;
    void Start()
    {
        _playerMovement = GameObject.Find("/Player").GetComponent<PlayerMovement>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && _playerMovement.isMovingToTargetPoint)
        {
            //GetComponentInParent<MoveToPoints>().TeleportToPoint(index);
            GetComponentInParent<MoveToPoints>().TargetNextPoint(index);
            //GetComponentInParent<MoveToPoints>().SetSpeed();
        }
    }
}
