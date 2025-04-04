using TMPro;
using UnityEngine;
using FMOD.Studio;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]private int maxHealth;
    public int MaxHealth { get => maxHealth; set => maxHealth = value;  }
    [SerializeField] private EndScreen endScreen; 
    [SerializeField] private TextMeshProUGUI playerHealthDisplay;
    [SerializeField] private PlayerHealthDisplayUpdater playerHealthDisplayUpdater;
    private VFXManager _vfx;
    private int _currentHealth;
    public int CurrentHealth { get => _currentHealth; set => _currentHealth = value; }
    private PlayerMovement playerMovement;
    [SerializeField] private ChromaticAberration chromaticAberration;
    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        _currentHealth = maxHealth;
        playerHealthDisplay.text = $"Health: {_currentHealth: 0}";
        playerHealthDisplayUpdater.UpdatePlayerHealthBarDisplay(_currentHealth);
        playerHealthDisplayUpdater.UpdatePlayerPortraitDisplay(_currentHealth);
        _vfx = GetComponentInChildren<VFXManager>();
        if (chromaticAberration == null)
        {
            chromaticAberration = GameObject.Find("/Chromatic Aberration").GetComponent<ChromaticAberration>();
        }
    }

    public void DecreaseHealth() 
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.PlayerHurt, this.transform.position);
        _vfx.CallHurtVFX();
        chromaticAberration.StartChromaticAberration();
        if(_currentHealth != 0) 
        { 
            _currentHealth -= 1;
            playerHealthDisplayUpdater.UpdatePlayerHealthBarDisplay(_currentHealth);
            playerHealthDisplayUpdater.UpdatePlayerPortraitDisplay(_currentHealth);
            playerHealthDisplay.text = $"Health: {_currentHealth: 0}";
        } 
        if(_currentHealth <= 0) 
        {
            playerMovement._playerSkatingGround.stop(STOP_MODE.ALLOWFADEOUT);
            playerMovement._playerSkatingAir.stop(STOP_MODE.ALLOWFADEOUT);
            AudioManager.instance.musicEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            AudioManager.instance.ambienceEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            AudioManager.instance.musicEventInstance.release();
            AudioManager.instance.ambienceEventInstance.release();
            endScreen.Toggle(true);
            endScreen.EndScreenText("You ran out of health...");
        }
    }
    public void IncreaseHealth() 
    { 
        if (_currentHealth != 0) 
        { 
            _currentHealth += 1;
            playerHealthDisplayUpdater.UpdatePlayerHealthBarDisplay(_currentHealth);
            playerHealthDisplayUpdater.UpdatePlayerPortraitDisplay(_currentHealth);
            playerHealthDisplay.text = $"Health: {_currentHealth: 0}";
        } 
    }
}
