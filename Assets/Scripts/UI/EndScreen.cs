using UnityEngine;
using TMPro;
public class EndScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI endScreenText; 
    [SerializeField] private SceneHandler sceneHandler;
    [SerializeField] private bool endScreenToggle;

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
            //Time.timeScale = 0; 
        }
        else 
        { 
            transform.GetChild(0).gameObject.SetActive(false);
            //Time.timeScale = 1;
        }

    }

    public void EndScreenText(string endScreenText)
    {
        this.endScreenText.text = endScreenText; 
    }
}
