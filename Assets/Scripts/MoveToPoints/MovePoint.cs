using UnityEngine;

public class MovePoint : MonoBehaviour
{
    public int index;
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponentInParent<MoveToPoints>().TargetNextPoint(index);
            //GetComponentInParent<MoveToPoints>().SetSpeed();
        }
    }
}
