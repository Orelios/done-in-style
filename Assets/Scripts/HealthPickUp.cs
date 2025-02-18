using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth))
        {
            if(collision.GetComponent<PlayerHealth>().Health != collision.GetComponent<PlayerHealth>().MaxHealth) 
            { 
                collision.GetComponent<PlayerHealth>().IncreaseHealth();
                Destroy(gameObject);
            }

        }
    }
}
