using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Onomatopoeia : MonoBehaviour
{
    [SerializeField] private List<Sprite> Letters = new List<Sprite>();
    [SerializeField] private float typingSpeed = 0.1f;
    [SerializeField] private float enlargeTime = 0.25f, enlargeBGTime = 0.75f;
    [SerializeField] private float smallScaleFactor = 0.5f;
    [SerializeField] private float disableTime = 1.5f;
    [SerializeField] private GameObject player;
    [SerializeField] private Vector3 offsetPosition; 
    
    private void Start()
    {
        //StartCoroutine(TypeImages());
        GetComponent<Canvas>().worldCamera = Camera.main;
        DisableChildren();
    }

    public void StartTypingVFX()
    {
        EnableChildren();
        GoToPlayerLocation(); 
        StartCoroutine(TypeImages());
    }

    private IEnumerator TypeImages()
    {
        Transform[] children = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            children[i] = transform.GetChild(i);
            Color imageColor = children[i].GetComponent<Image>().color;
            imageColor.a = 0f;
            children[i].GetComponent<Image>().color = imageColor;
        }

        for (int i = 0; i < children.Length; i++)
        {
            Image image = children[i].GetComponent<Image>();
            if (image != null)
            {
                image.sprite = Letters[i];
                if (i == 0)
                {
                    StartCoroutine(AnimateBG(children[i], image));
                }
                else
                {
                    StartCoroutine(AnimateSize(children[i], image));
                }
            }
            yield return new WaitForSecondsRealtime(typingSpeed);
        }

        yield return new WaitForSeconds(disableTime);
        foreach (Transform child in children)
        {
            child.gameObject.SetActive(false);
        }
    }

    private IEnumerator AnimateSize(Transform child, Image image)
    {
        Vector3 originalSize = child.localScale;
        Vector3 smallSize = originalSize * smallScaleFactor;
        float elapsedTime = 0f;

        Color imageColor = image.color;
        imageColor.a = 1f;
        image.color = imageColor;

        child.localScale = smallSize;
        while (elapsedTime < enlargeTime)
        {
            child.localScale = Vector3.Lerp(smallSize, originalSize, elapsedTime / enlargeTime);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }
        child.localScale = originalSize;
    }
    private IEnumerator AnimateBG(Transform child, Image image)
    {
        Vector3 originalSize = child.localScale;
        Vector3 smallSize = originalSize * smallScaleFactor;
        float elapsedTime = 0f;

        Color imageColor = image.color;
        imageColor.a = 1f;
        image.color = imageColor;

        child.localScale = smallSize;
        while (elapsedTime < enlargeBGTime)
        {
            child.localScale = Vector3.Lerp(smallSize, originalSize, elapsedTime / enlargeBGTime);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }
        child.localScale = originalSize;
    }

    private void DisableChildren()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    private void EnableChildren()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    private void GoToPlayerLocation()
    {

        gameObject.transform.position = player.GetComponent<Transform>().position + offsetPosition; 
    }
}
