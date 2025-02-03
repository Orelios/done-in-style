using System.Collections.Generic;
using UnityEngine;

public class TEMP_Railing : MonoBehaviour
{
    public List<Vector2> ColliderPoints = new();
    private EdgeCollider2D _collider;

    private void Start()
    {
        _collider = GetComponent<EdgeCollider2D>();
        
        foreach (var points in _collider.points)
        {
            ColliderPoints.Add(points);
        }
    }
}
