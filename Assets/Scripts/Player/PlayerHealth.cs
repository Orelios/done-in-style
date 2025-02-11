using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]private int maxHealth;
    [SerializeField] private EndScreen endScreen; 
    [SerializeField] private TextMeshProUGUI playerHealthDisplay;
    private int _health;
    public int Health { get => _health; set => _health = value; }
    private void Awake()
    {
        _health = maxHealth;
        playerHealthDisplay.text = $"Health: {_health: 0}";
    }

    public void DecreaseHealth() 
    { 
        if(_health != 0) 
        { 
            _health -= 1;
            playerHealthDisplay.text = $"Health: {_health: 0}";
        } 
        if(_health <= 0) 
        { 
            endScreen.Toggle(true);
            endScreen.EndScreenText("You ran out of health...");
        }
    }
    public void IncreaseHealth() 
    { 
        if (_health != 0) 
        { 
            _health += 1;
            playerHealthDisplay.text = $"Health: {_health: 0}";
        } 
    }
}
