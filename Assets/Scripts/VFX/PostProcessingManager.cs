using UnityEngine;
using UnityEngine.UI;

public class PostProcessingManager : MonoBehaviour
{
    [SerializeField] public ChromaticAberration chromaticAberration;
    [SerializeField] private bool chromaticAberrationOn = true;
    public bool ChromaticAberrationOn => chromaticAberrationOn;

    [SerializeField] public GameObject motionBlur;
    [SerializeField] private bool motionBlurOn = true;
    public bool MotionBlurOn => motionBlurOn;

    private void Start()
    {
        if (chromaticAberration == null)
        {
            //chromaticAberration = GameObject.Find("/Chromatic Aberration").GetComponent<ChromaticAberration>(); //doesn't work
            chromaticAberration = GameObject.FindFirstObjectByType<ChromaticAberration>();
        }
        if (motionBlur == null)
        {
            motionBlur = GameObject.Find("/Motion Blur"); //doesn't work
        }
        if (PlayerPrefs.HasKey("KeyChromaticAberration"))
        {
            LoadChromaticAberration();
        }
        if (PlayerPrefs.HasKey("KeyMotionBlur"))
        {
            LoadMotionBlur();
        }
    }

    public void ToggleChromaticAberration(Image image)
    {
        chromaticAberrationOn = !chromaticAberrationOn;
        image?.gameObject.SetActive(chromaticAberrationOn);
    }

    public void ToggleMotionBlur(Image image)
    {
        motionBlurOn = !motionBlurOn;
        image?.gameObject.SetActive(motionBlurOn);
    }

    public void SaveChromaticAberration()
    {
        PlayerPrefs.SetInt("KeyChromaticAberration", chromaticAberrationOn ? 1 : 0);
    }

    public void LoadChromaticAberration()
    {
        if (PlayerPrefs.GetInt("KeyChromaticAberration") == 1)
        {
            chromaticAberrationOn = true;
        }
        else if (PlayerPrefs.GetInt("KeyChromaticAberration") == 0)
        {
            chromaticAberrationOn = false;
        }
    }

    public void SaveMotionBlur()
    {
        PlayerPrefs.SetInt("KeyMotionBlur", motionBlurOn ? 1 : 0);
    }

    public void LoadMotionBlur()
    {
        if (PlayerPrefs.GetInt("KeyMotionBlur") == 1)
        {
            motionBlurOn = true;
        }
        else if (PlayerPrefs.GetInt("KeyMotionBlur") == 0)
        {
            motionBlurOn = false;
        }
    }
}
