using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPoints : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private List<GameObject> movePoints = new List<GameObject>();
    private Vector2 targetPos;
    private PlayerMovement _playerMovement;
    private int index;

    private void Start()
    {
        int i = 0;
        // Get all child objects of Zipline and add them to the movePoints list
        foreach (Transform child in transform)
        {
            if (child.GetComponent<BoxCollider2D>() != null && child.GetComponent<BoxCollider2D>().isTrigger)
            {
                movePoints.Add(child.gameObject);
                child.gameObject.GetComponent<MovePoint>().index = i;
            }
            i++;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _playerMovement = other.gameObject.GetComponent<PlayerMovement>();
            _playerMovement.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        }
        TargetNextPoint();
        StartCoroutine(MoveToTarget());
    }

    public void TargetNextPoint()
    {
        if (_playerMovement.IsFacingRight)
        {
            for (int i = 0; i < movePoints.Count; i++)
            {
                if (movePoints[i].transform.position.x > _playerMovement.transform.position.x)
                {
                    targetPos = movePoints[i].transform.position;
                    index = i;
                    break;
                }
            }
        }
        else if (!_playerMovement.IsFacingRight)
        {
            for (int i = movePoints.Count-1; i >= 0; i--)
            {
                if (movePoints[i].transform.position.x < _playerMovement.transform.position.x)
                {
                    targetPos = movePoints[i].transform.position;
                    index = i;
                    break;
                }
            }
        }

    }

    private IEnumerator MoveToTarget()
    {
        while ((Vector2)_playerMovement.transform.position != targetPos)
        {
            _playerMovement.transform.position = Vector2.MoveTowards(_playerMovement.transform.position, targetPos, moveSpeed);
            yield return null;
        }
        if ((_playerMovement.IsFacingRight && index < movePoints.Count-1) || (!_playerMovement.IsFacingRight && index <= 0))
        {
            TargetNextPoint();
        }
        else
        {
            _playerMovement.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
            Debug.Log("end reached");
        }
    }

}
