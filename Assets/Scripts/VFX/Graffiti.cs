using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Graffiti : MonoBehaviour
{
    public List<Sprite> originalImgs = new List<Sprite>();
    public List<Sprite> replacementImgs = new List<Sprite>();
    public GameObject origObj;
    public GameObject replacementObj;
    public GameObject particle;
    private ParticleSystem particleVFX;

    [SerializeField] private float fadeDuration = 2f;

    private void Start()
    {
        if (origObj == null)
        {
            origObj = transform.GetChild(0).gameObject;
        }
        if (replacementObj == null)
        {
            replacementObj = transform.GetChild(1).gameObject;
        }
        origObj.GetComponent<Image>().sprite = originalImgs[Random.Range(0, originalImgs.Count)];
        replacementObj.GetComponent<Image>().sprite = replacementImgs[Random.Range(0, replacementImgs.Count)];
        particleVFX = particle.GetComponent<ParticleSystem>();
        particleVFX.Stop();
        //StartGraffiti();
    }

    public void StartGraffiti()
    {
        StartCoroutine(ReplaceGraffiti());
    }

    private IEnumerator ReplaceGraffiti()
    {
        particleVFX.Play();
        float elapsedTime = 0f;
        Image origImage = origObj.GetComponent<Image>();
        Image replaceImage = replacementObj.GetComponent<Image>();
        Color origColor = origImage.color;
        Color replaceColor = replaceImage.color;

        replaceColor.a = 0f;
        replaceImage.color = replaceColor;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);

            origColor.a = 1f - alpha;
            replaceColor.a = alpha;

            origImage.color = origColor;
            replaceImage.color = replaceColor;

            yield return null;
        }
        particleVFX.Stop();
    }
}
