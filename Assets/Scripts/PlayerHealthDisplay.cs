using UnityEngine;
using TMPro;

public class PlayerHealthDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerHealthDisplay;
    [SerializeField] private PlayerHealth PlayerHealth; 
    private void Awake()
    {
        playerHealthDisplay.text = $"Health: {PlayerHealth.Health: 0}";
    }

    public void TakeDamaged() 
    { 
        PlayerHealth.DecreaseHealth();
        playerHealthDisplay.text = $"Health: {PlayerHealth.Health: 0}";
    }
}
