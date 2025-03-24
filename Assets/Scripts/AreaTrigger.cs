using System;
using System.Collections.Generic;
using UnityEngine;

public class AreaTrigger : MonoBehaviour
{
    private List<ITriggerable> _objectsToTrigger = new();

    private void OnEnable()
    {
        foreach (var child in transform.GetComponentsInChildren<ITriggerable>())
        {
            _objectsToTrigger.Add(child);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (var triggerable in _objectsToTrigger)
            {
                triggerable.DoTrigger();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (var triggerable in _objectsToTrigger)
            {
                triggerable.StopTrigger();
            }
        }
    }
}
