using TMPro;
using UnityEngine;
using FMOD.Studio;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]private int maxHealth;
    public int MaxHealth { get => maxHealth; set => maxHealth = value;  }
    [SerializeField] private EndScreen endScreen; 
    [SerializeField] private TextMeshProUGUI playerHealthDisplay;
    private VFXManager _vfx;
    private int _health;
    public int Health { get => _health; set => _health = value; }
    private PlayerMovement playerMovement;
    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        _health = maxHealth;
        playerHealthDisplay.text = $"Health: {_health: 0}";
        _vfx = GetComponentInChildren<VFXManager>();
    }

    public void DecreaseHealth() 
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.PlayerHurt, this.transform.position);
        _vfx.CallHurtVFX();
        if(_health != 0) 
        { 
            _health -= 1;
            playerHealthDisplay.text = $"Health: {_health: 0}";
        } 
        if(_health <= 0) 
        {
            playerMovement._playerSkatingGround.stop(STOP_MODE.ALLOWFADEOUT);
            playerMovement._playerSkatingAir.stop(STOP_MODE.ALLOWFADEOUT);
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
