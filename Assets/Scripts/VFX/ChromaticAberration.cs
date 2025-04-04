using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ChromaticAberration : MonoBehaviour
{
    UnityEngine.Rendering.Universal.ChromaticAberration chromaticAberration;
    //private float intensityVal = 0f;
    [SerializeField] private float aberrationDuration = 1f;
    private Coroutine ChromaticAberrationCor;
    void Start()
    {
        //intensityVal = 0.5f;
        //UnityEngine.Rendering.VolumeProfile profile = GameObject.Find("Post Processing").GetComponent<UnityEngine.Rendering.Volume>().profile;
        UnityEngine.Rendering.VolumeProfile profile = gameObject.GetComponent<UnityEngine.Rendering.Volume>().profile;
        profile.TryGet(out chromaticAberration);
        //chromaticAberration.intensity.Override(intensityVal);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyUp(KeyCode.Q))
        {
            StartChromaticAberration();
        }
        */
    }

    public void StartChromaticAberration()
    {
        ChromaticAberrationCor = StartCoroutine(ChromaticAberrationCoroutine());
    }

    private IEnumerator ChromaticAberrationCoroutine()
    {
        /*
        // Gradually change the intensity from 0 to 1 over 1 second
        float time = 0f;
        while (time < 1f)
        {
            chromaticAberration.intensity.value = Mathf.Lerp(0f, 1f, time);
            time += Time.deltaTime;
            //Debug.Log("intensity = " + chromaticAberration.intensity.value);
            yield return null;
        }
        chromaticAberration.intensity.value = 1f;
        */

        // Gradually change the intensity from 1 to 0 over 1 second
        chromaticAberration.intensity.value = 1f;
        float time = 0f;
        while (time < aberrationDuration)
        {
            chromaticAberration.intensity.value = Mathf.Lerp(1f, 0f, time);
            time += Time.deltaTime;
            //Debug.Log("intensity = " + chromaticAberration.intensity.value);
            yield return null;
        }
        chromaticAberration.intensity.value = 0f;
        ChromaticAberrationCor = null;
    }
}
