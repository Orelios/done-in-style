using UnityEngine;

public class DestructibleObjects : MonoBehaviour
{
    [SerializeField] private EDaredevilGearType gearType; 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerGearSwapper>().CurrentGearEquipped.DaredevilGearType == gearType)
        {
            Destroy(gameObject); 
        }
    }
}
