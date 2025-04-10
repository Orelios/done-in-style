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
            chromaticAberration = GameObject.Find("/Chromatic Aberration").GetComponent<ChromaticAberration>(); //doesn't work
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
}
