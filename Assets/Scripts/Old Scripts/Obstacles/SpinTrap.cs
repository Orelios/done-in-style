using UnityEngine;

public class SpinTrap : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private float rotationSpeed;
    [SerializeField] private bool clockwiseRotation;
    //[SerializeField] private float iFramesTime;
    //private 

    [Header("Damage Components")]
    [SerializeField] private TEMP_ScoreCalculator scoreCalculator;
    [SerializeField] private int damage;
    private float _rotZ; 
    void Update()
    {
        Rotate(); 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<TEMP_PlayerIFrames>() && !collision.gameObject.GetComponent<TEMP_PlayerIFrames>().IsHit)
        {
            scoreCalculator.DecreaseScore(damage);
            scoreCalculator.GetComponent<Temp_RankCalculator>().DecreaseStylishPoints();
            collision.gameObject.GetComponent<TEMP_PlayerIFrames>().PlayerHit();
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
