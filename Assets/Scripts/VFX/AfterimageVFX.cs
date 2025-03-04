using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterimageVFX : MonoBehaviour
{
    [SerializeField] private float minSpeed = 1f;
    [SerializeField] private float interval = 0.2f;
    [SerializeField] private float lifetime = 2f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private float lastAfterimageTime;
    private Transform afterimageParent;

    private Player _player;
    private RankCalculator _rankCalculator;
    [SerializeField] private List<Color> colorList = new List<Color>();
    private int currentIndex = 1; // To track the current index in the list


    void Start()
    {
        _player = GetComponentInParent<Player>();
        rb = GetComponentInParent<Rigidbody2D>();
        //rb = _player.Rigidbody;
        spriteRenderer = transform.parent.GetChild(0).GetComponent<SpriteRenderer>();
        //  spriteRenderer = _player.Sprite;

        GameObject parentObj = GameObject.Find("Afterimages");
        if (parentObj == null)
        {
            parentObj = new GameObject("Afterimages");
        }
        afterimageParent = parentObj.transform;

        _rankCalculator = GameObject.Find("Score System").GetComponent<RankCalculator>();
        colorList.Add(new Color(1f, 1f, 1f, 0f)); //invisible
        colorList.Add(Color.red);
        colorList.Add(new Color(1f, 0.647f, 0f));   // Orange (RGB)
        colorList.Add(Color.yellow);
        colorList.Add(Color.green);
        colorList.Add(Color.blue);
    }

    void Update()
    {
        if (Mathf.Abs(rb.linearVelocity.x) >= minSpeed && Time.time >= lastAfterimageTime + interval && _rankCalculator.CurrentStylishRankIndex > 0)
        {
            CreateAfterimage();
            lastAfterimageTime = Time.time;
        }
    }

    void CreateAfterimage()
    {
        GameObject afterimage = new GameObject("Afterimage");
        afterimage.transform.position = transform.position;
        afterimage.transform.rotation = transform.rotation;
        afterimage.transform.parent = afterimageParent;

        SpriteRenderer afterimageRenderer = afterimage.AddComponent<SpriteRenderer>();
        afterimageRenderer.sprite = spriteRenderer.sprite;
        //afterimageRenderer.color = new Color(1f, 1f, 1f, 1f);
        if (currentIndex > _rankCalculator.CurrentStylishRankIndex)
        {
            currentIndex = 1;
        }
        afterimageRenderer.color = colorList[currentIndex];
        CycleColors();
        afterimageRenderer.sortingLayerID = spriteRenderer.sortingLayerID;
        afterimageRenderer.sortingOrder = spriteRenderer.sortingOrder - 1;

        StartCoroutine(FadeAndDestroy(afterimageRenderer, afterimage));
    }

    IEnumerator FadeAndDestroy(SpriteRenderer renderer, GameObject afterimage)
    {
        float elapsedTime = 0f;
        Color startColor = renderer.color;

        if (renderer.color.a > 0)
        {
            while (elapsedTime < lifetime)
            {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, elapsedTime / lifetime);
                renderer.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
                yield return null;
            }
        }
        

        Destroy(afterimage);
    }

    private void CycleColors()
    {
        currentIndex++;

        // If the index reaches the end of the list, reset it to red
        if (currentIndex >= colorList.Count)
        {
            currentIndex = 1;
        }
    }
}
