/*
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Rendering.Universal;

public class ChromaticAberrationEffect : MonoBehaviour
{
    public PostProcessVolume postProcessVolume;
    private ChromaticAberration chromaticAberration;
    private bool isCoroutineRunning = false;

    void Start()
    {
        if (postProcessVolume == null)
        {
            postProcessVolume = GetComponent<PostProcessVolume>();
        }

        // Get the ChromaticAberration effect from the post process volume
        if (postProcessVolume.profile.TryGetSettings(out chromaticAberration))
        {
            StartCoroutine(ChromaticAberration());
        }
    }

    IEnumerator ChromaticAberration()
    {
        // Gradually change the intensity from 0 to 1 over 1 second
        float time = 0f;
        while (time < 1f)
        {
            chromaticAberration.intensity.value = Mathf.Lerp(0f, 1f, time);
            time += Time.deltaTime;
            yield return null;
        }
        chromaticAberration.intensity.value = 1f;

        // Gradually change the intensity from 1 to 0 over 1 second
        time = 0f;
        while (time < 1f)
        {
            chromaticAberration.intensity.value = Mathf.Lerp(1f, 0f, time);
            time += Time.deltaTime;
            yield return null;
        }
        chromaticAberration.intensity.value = 0f;
    }
}
*/
