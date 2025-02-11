using UnityEngine;

public class DestructibleObjects : MonoBehaviour
{
    [SerializeField] private EDaredevilGearType gearType; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerTricks>())
        {
            //TODO: ADD POINTS WHEN DESTROYED
            TimeHandler.SlowDownTime();
            Destroy(gameObject);
        }
    }
}
