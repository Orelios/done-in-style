using UnityEngine;
using TMPro;
using FMOD.Studio;
public class EndScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI endScreenText; 
    [SerializeField] private SceneHandler sceneHandler;
    [SerializeField] private bool endScreenToggle;
    [SerializeField] private PlayerMovement playerMovement;

    private void Awake()
    {
        endScreenToggle = false; 
    }
    public void Toggle(bool toggle)
    {
        endScreenToggle = toggle;

        if (endScreenToggle)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            playerMovement._playerSkatingGround.stop(STOP_MODE.ALLOWFADEOUT);
            playerMovement._playerSkatingAir.stop(STOP_MODE.ALLOWFADEOUT);
            Time.timeScale = 0; 
        }
        else 
        { 
            transform.GetChild(0).gameObject.SetActive(false);
            playerMovement._playerSkatingGround.stop(STOP_MODE.ALLOWFADEOUT);
            playerMovement._playerSkatingAir.stop(STOP_MODE.ALLOWFADEOUT);
            Time.timeScale = 1;
        }

    }

    public void EndScreenText(string endScreenText)
    {
        this.endScreenText.text = endScreenText; 
    }
}
