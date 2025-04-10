using System.Collections.Generic;
using UnityEngine;

public class TestOpenSettings : MonoBehaviour
{
    public List<GameObject> objects = new List<GameObject>();
    void Start()
    {
        foreach (GameObject obj in objects)
        {
            obj.SetActive(true);
        }
        /*
        foreach (GameObject obj in objects)
        {
            obj.SetActive(false);
        }
        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
