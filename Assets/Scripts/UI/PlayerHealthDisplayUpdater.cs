using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthDisplayUpdater : MonoBehaviour
{
    [Header("Health Bar")]
    [SerializeField] private Image healthBar;
    [SerializeField] private List<Sprite> healthBars = new();

    [Header("Player Portrait")]
    [SerializeField] private Image playerPortrait;
    [SerializeField] private List<Sprite> playerPortraits = new();

    public void UpdatePlayerHealthBarDisplay(int health)
    {
        healthBar.sprite = healthBars[Mathf.Max(0, health - 1)];
        healthBar.SetNativeSize();
    }
    
    public void UpdatePlayerPortraitDisplay(int health)
    {
        playerPortrait.sprite = playerPortraits[Mathf.Max(0, health - 1)];
        playerPortrait.SetNativeSize();
    }
}
