using UnityEngine;
using UnityEngine.InputSystem;

public class VolumeSettingToggle : MonoBehaviour
{
    private bool _volumeSettingToggle; 
    private void Start()
    {
        _volumeSettingToggle = false;
    }

    public void VolumeSetting(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!_volumeSettingToggle) { _volumeSettingToggle = true; }
            else { _volumeSettingToggle = false; }
        }

        if (_volumeSettingToggle)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }

    }
}
