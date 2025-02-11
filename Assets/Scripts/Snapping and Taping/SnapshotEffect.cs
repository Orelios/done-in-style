using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SnapshotEffect : MonoBehaviour
{
    [SerializeField] private float retainSnapTime = 0.1f;
    [SerializeField] private float fadeTime = 0.3f;
    [SerializeField] private Image snapshotOverlay;

    private void Awake()
    {
        if (snapshotOverlay != null)
        {
            snapshotOverlay.color = new Color(1, 1, 1, 0); // Ensure it's fully transparent at start
        }
    }

    public void TriggerSnapshot()
    {
        if (snapshotOverlay != null)
        {
            StartCoroutine(SnapshotCoroutine());
        }
    }

    private IEnumerator SnapshotCoroutine()
    {
        // Brighten (Full white screen effect)
        snapshotOverlay.color = new Color(1, 1, 1, 1);

        // Hold the snapshot effect
        yield return new WaitForSeconds(retainSnapTime);

        // Fade out effect
        float elapsedTime = 0f;
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, elapsedTime / fadeTime);
            snapshotOverlay.color = new Color(1, 1, 1, alpha);
            yield return null;
        }

        snapshotOverlay.color = new Color(1, 1, 1, 0); // Ensure fully transparent at the end
    }
}
