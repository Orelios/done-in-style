using UnityEngine;

public class DestructibleObjects : MonoBehaviour
{
    [SerializeField] private EDaredevilGearType gearType; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<GearTricks>())
        {
            if (collision.gameObject.GetComponent<PlayerGearSwapper>().CurrentGearEquipped.DaredevilGearType == gearType)
            {
                //TODO: ADD POINTS WHEN DESTROYED
                TimeHandler.SlowDownTime();
                Destroy(gameObject);
            }
        }
    }
}
