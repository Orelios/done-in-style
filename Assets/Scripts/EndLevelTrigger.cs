using System;
using UnityEngine;

public class EndLevelTrigger : MonoBehaviour
{
    private LevelHandler _levelHandler;

    private void Start()
    {
        _levelHandler = FindFirstObjectByType<LevelHandler>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Player>(out _))
        {
            _levelHandler.EndLevel();
        }
    }
}