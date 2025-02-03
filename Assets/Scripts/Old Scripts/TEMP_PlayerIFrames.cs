using System.Collections;
using UnityEngine;

public class TEMP_PlayerIFrames : MonoBehaviour
{
    [SerializeField] private float playerInvincibilityTime; 
    private bool _isHit; 
    public bool IsHit => _isHit;

    private void Awake()
    {
        _isHit = false;
    }

    public void PlayerHit()
    {
        StartCoroutine(PlayerInvincibility()); 
    }

    private IEnumerator PlayerInvincibility()
    {
        _isHit = true;
        yield return new WaitForSeconds(playerInvincibilityTime); 
        _isHit = false;
    }
}
