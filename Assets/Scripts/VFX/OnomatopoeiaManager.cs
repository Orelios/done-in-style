using System.Collections.Generic;
using UnityEngine;

public class OnomatopoeiaManager : MonoBehaviour
{
    public List<GameObject> Canvases = new List<GameObject>();
    public GameObject onomatopoeia;

    private void Awake()
    {
        // Populate the list with all children except the first one
        for (int i = 1; i < transform.childCount; i++)
        {
            Canvases.Add(transform.GetChild(i).gameObject);
        }

        // Ensure there's something to select
        if (Canvases.Count > 0)
        {
            // Randomly select one from the list
            int randomIndex = Random.Range(0, Canvases.Count);
            onomatopoeia = Canvases[randomIndex];

            // Enable only the selected one
            foreach (GameObject canvas in Canvases)
            {
                canvas.SetActive(canvas == onomatopoeia);
            }
        }
    }
}