using System.Collections;
using UnityEngine;

public class TEMP_PlayerIFrames : MonoBehaviour
{
    [SerializeField] private float playerInvincibilityTime;
    [SerializeField] private float knockbackStrength; 
    private bool _isHit; 
    public bool IsHit => _isHit;

    private PlayerHealth _playerHealth; 
    private void Awake()
    {
        _isHit = false;
        _playerHealth = GetComponent<PlayerHealth>();
    }

    public void PlayerHit()
    {
        StartCoroutine(PlayerInvincibility());
    }

    private IEnumerator PlayerInvincibility()
    {
        _isHit = true;
        _playerHealth.DecreaseHealth();
        knockBack(); 
        yield return new WaitForSeconds(playerInvincibilityTime); 
        _isHit = false;
    }

    private void knockBack()
    {
        GetComponent<Rigidbody2D>().linearVelocity =
            new Vector2(GetComponent<Transform>().transform.rotation.y == 0 ? 1 - knockbackStrength : -1 * 
            -knockbackStrength, knockbackStrength);
        //Debug.Log(collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity);
    }
}
