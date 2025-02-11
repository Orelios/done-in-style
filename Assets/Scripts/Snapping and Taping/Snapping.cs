using System;
using UnityEngine;

public class Snapping : MonoBehaviour
{
    private PlayerTricks _playerTricks;
    [SerializeField] private GameObject _ui;
    void Awake()
    {
        if (_ui == null)
        {
            _ui = GameObject.Find("UI/Player/SnappingUI/CameraUI");
        }
    }

    void Update()
    {
        if (_ui != null)
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S))
            {
                _ui.SetActive(false);
                gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _playerTricks = other.GetComponent<PlayerTricks>();
            _playerTricks.isSnapping = true;
            _ui.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //_playerTricks = other.GetComponent<PlayerTricks>();
            _playerTricks.isSnapping = false;
            //_playerTricks = null;
            _ui.SetActive(false);
        }
    }
}
