using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
    [SerializeField] private Vector2 startPos;
    [SerializeField] private Vector2 targetPos;
    [SerializeField] private float floatTime = 1f;

    private float timer = 0f;
    private bool isMovingUp = true;

    void Start()
    {
        startPos = transform.position;
        targetPos = new Vector2(startPos.x, startPos.y + 0.4f);
    }

    void Update()
    {
        timer += Time.deltaTime;
        float t = timer / floatTime;

        if (isMovingUp)
        {
            transform.position = Vector2.Lerp(startPos, targetPos, t);
        }
        else
        {
            transform.position = Vector2.Lerp(targetPos, startPos, t);
        }

        if (t >= 1f)
        {
            timer = 0f;
            isMovingUp = (isMovingUp ? false : true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth))
        {
            if(collision.GetComponent<PlayerHealth>().CurrentHealth != collision.GetComponent<PlayerHealth>().MaxHealth) 
            {
                AudioManager.instance.PlayOneShot(FMODEvents.instance.HealthPickup, this.transform.position);
                collision.GetComponent<PlayerHealth>().IncreaseHealth();
                Destroy(gameObject);
            }

        }
    }
}
