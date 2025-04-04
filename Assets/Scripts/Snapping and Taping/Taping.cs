using System;
using System.Collections;
using UnityEngine;

public class Taping : MonoBehaviour
{
    private PlayerTricks _playerTricks;
    [SerializeField] private GameObject _ui;
    void Awake()
    {
        if (_ui == null)
        {
            _ui = GameObject.Find("UI/Player/TapingUI/CameraUI");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_ui != null && _playerTricks != null)
        {
            if (_playerTricks.canTape && !_playerTricks.isTaping && (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S)))
            {
                StartCoroutine(StartTaping());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _playerTricks = other.GetComponent<PlayerTricks>();
            //_playerTricks.isTaping = true;
            _playerTricks.canTape = true;
            if (!_playerTricks.isTaping)
            {
                _ui.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //_playerTricks.isTaping = false;
            _playerTricks.canTape = false;
            if (!_playerTricks.isTaping)
            {
                _ui.SetActive(false);
                _playerTricks = null;
            }
            
        }
    }

    IEnumerator StartTaping()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.CameraRecording, this.transform.position);
        _playerTricks.isTaping = true;
        yield return new WaitForSeconds(_playerTricks.tapingDuration);
        _playerTricks.isTaping = false;
        _ui.SetActive(false);
        gameObject.SetActive(false);
    }
}
