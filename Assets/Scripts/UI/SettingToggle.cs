using UnityEngine;
using UnityEngine.InputSystem;

public class SettingToggle : MonoBehaviour
{
    [SerializeField] private GameObject volumeSettings;
    [SerializeField] private GameObject keyRebindSettings;
    [SerializeField] private GameObject graphicsSettings;
    [SerializeField] private GameObject settings; 
    private bool _settingToggle, _volumeToggle, _keyRebindToggle, _graphicsToggle, _pauseMenuToggle;
    private void Start()
    {
        _settingToggle = false;
        _volumeToggle = false;
        _keyRebindToggle = false;
        _graphicsToggle = false;
        _pauseMenuToggle = false;
    }

    public void VolumeSetting(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!_pauseMenuToggle) { _pauseMenuToggle = true; }
            else { _pauseMenuToggle = false; }
        }

        if (_pauseMenuToggle) { transform.GetChild(0).gameObject.SetActive(true); }
        else { transform.GetChild(0).gameObject.SetActive(false); }
    }

    public void OpenVolumeSetting()
    {
        if (!_volumeToggle) { _volumeToggle = true; }
        else { _volumeToggle = false; }

        if (_volumeToggle) { volumeSettings.gameObject.SetActive(true); }
        else { volumeSettings.gameObject.SetActive(false); }
    }
    public void OpenKeyRebindSetting()
    {
        if (!_keyRebindToggle) { _keyRebindToggle = true; }
        else { _keyRebindToggle = false; }

        if (_keyRebindToggle) { keyRebindSettings.gameObject.SetActive(true); }
        else { keyRebindSettings.gameObject.SetActive(false); }
    }
    public void OpenGraphicsSetting()
    {
        if (!_graphicsToggle) { _graphicsToggle = true; }
        else { _graphicsToggle = false; }

        if (_graphicsToggle) { graphicsSettings.gameObject.SetActive(true); }
        else { graphicsSettings.gameObject.SetActive(false); }
    }

    public void OpenSetting()
    {
        if (!_settingToggle) { _settingToggle = true; }
        else { _settingToggle = false; }

        if (_settingToggle) { settings.gameObject.SetActive(true); }
        else { settings.gameObject.SetActive(false); }
    }
}
