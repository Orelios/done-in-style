using System.Collections;
using UnityEngine;

public class RegeneratingPlatform : MonoBehaviour
{
    [SerializeField] private float fadeTime = 1f;
    [SerializeField] private float reappearTime = 2f;

    private Collider2D platformCollider;
    private Renderer platformRenderer;
    private Material platformMaterial;
    private Color initialColor;

    private void Awake()
    {
        platformCollider = GetComponent<Collider2D>();
        platformRenderer = GetComponent<Renderer>();
        platformMaterial = platformRenderer.material;
        initialColor = platformMaterial.color;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(FadeOutAndReappear());
        }
    }

    private IEnumerator FadeOutAndReappear()
    {
        // Fade Out
        float elapsedTime = 0f;
        while (elapsedTime < fadeTime)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeTime);
            SetAlpha(alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        SetAlpha(0f);
        platformCollider.enabled = false;

        // Wait before reappearing
        yield return new WaitForSeconds(reappearTime);

        // Fade In
        elapsedTime = 0f;
        platformCollider.enabled = true;
        while (elapsedTime < fadeTime)
        {
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeTime);
            SetAlpha(alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        SetAlpha(1f);
    }

    private void SetAlpha(float alpha)
    {
        Color newColor = initialColor;
        newColor.a = alpha;
        platformMaterial.color = newColor;
    }
}
