using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPoints : MonoBehaviour
{
    //[SerializeField] private float startSpeed = 10f;
    [SerializeField] private List<GameObject> movePoints = new List<GameObject>();
    private Vector2 targetPos;
    private PlayerMovement _playerMovement;
    private int index;
    private float startSpeed, elapsedTime, moveSpeed;
    [SerializeField] private float maxSpeed, accelerationDuration = 1f;
    [SerializeField] private bool isMovingToTarget = false;
    //[SerializeField] private bool localIsFacingRight = true;

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
        _playerMovement = GameObject.Find("/Player").GetComponent<PlayerMovement>();
    }
    
    private void Update()
    {
        /*
        if (isMovingToTarget)
        {
            if (localIsFacingRight != _playerMovement.IsFacingRight)
            {
                localIsFacingRight = _playerMovement.IsFacingRight;
            }
        }
        */
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            StopAllCoroutines();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && !isMovingToTarget)
        {
            _playerMovement = other.gameObject.GetComponent<PlayerMovement>();
            _playerMovement.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
            _playerMovement.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            //localIsFacingRight = _playerMovement.IsFacingRight;
            SetSpeed();
            DetrmineTargetPoint();
            StartCoroutine(MoveToTarget());
            elapsedTime = 0;
        }
    }

    public void SetSpeed()
    {
        startSpeed = _playerMovement.Rb.linearVelocityX;
        moveSpeed = startSpeed;
        elapsedTime = 0;
    }

    public void DetrmineTargetPoint()
    {
        if (_playerMovement.IsFacingRight)
        {
            for (int i = 0; i < movePoints.Count; i++)
            {
                if (movePoints[i].transform.position.x > _playerMovement.transform.position.x)
                {
                    targetPos = movePoints[i].transform.position;
                    Debug.Log(movePoints[i].gameObject.name);
                    index = i;
                    break;
                }
            }
        }
        else if (!_playerMovement.IsFacingRight)
        {
            for (int i = movePoints.Count - 1; i >= 0; i--)
            {
                if (movePoints[i].transform.position.x < _playerMovement.transform.position.x)
                {
                    targetPos = movePoints[i].transform.position;
                    Debug.Log(movePoints[i].gameObject.name + " reversed");
                    index = i;
                    break;
                }
            }
        }

    }

    public void TargetNextPoint(int i)
    {
        _playerMovement.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        if (!isMovingToTarget)
        {
            SetSpeed();
        }

        if (_playerMovement.IsFacingRight)
        {
            if (i >= movePoints.Count - 1)
            {
                //end of list, no next target
                StopAllCoroutines();
                isMovingToTarget = false;
                _playerMovement.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
                _playerMovement.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                Debug.Log(" end");
            }
            else
            {
                StopAllCoroutines();
                isMovingToTarget = false;
                targetPos = movePoints[i + 1].transform.position;
                Debug.Log(movePoints[i].gameObject.name);
                StartCoroutine(MoveToTarget());
            }
        }
        else if (!_playerMovement.IsFacingRight)
        {
            if (i <= 0)
            {
                //start of list, no next target
                StopAllCoroutines();
                isMovingToTarget = false;
                _playerMovement.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
                _playerMovement.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                Debug.Log(" end");
            }
            else
            {
                StopAllCoroutines();
                isMovingToTarget = false;
                targetPos = movePoints[i - 1].transform.position;
                Debug.Log(movePoints[i].gameObject.name + " reversed");
                StartCoroutine(MoveToTarget());
            }
        }

    }

    private IEnumerator MoveToTarget()
    {
        isMovingToTarget = true;
        Vector2 currentTarget = targetPos;
        while (_playerMovement.transform.position.x != currentTarget.x)
        {
            _playerMovement.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
            _playerMovement.Rb.linearVelocityY = 0;
            _playerMovement.Rb.linearVelocityX = 0;

            moveSpeed = Mathf.Lerp(startSpeed, maxSpeed, elapsedTime / accelerationDuration);
            if (elapsedTime < accelerationDuration)
            {
                elapsedTime += Time.deltaTime;
            }
            else if (elapsedTime >= accelerationDuration)
            {
                elapsedTime = accelerationDuration;
            }
            
            _playerMovement.transform.position = Vector2.MoveTowards(_playerMovement.transform.position, currentTarget, moveSpeed * Time.fixedDeltaTime);
            yield return null;
        }
        isMovingToTarget = false;
        /* not used because task of choosing next target is based on int parameter passed by MovePoint child
        if ((_playerMovement.IsFacingRight && index < movePoints.Count-1) || (!_playerMovement.IsFacingRight && index <= 0))
        {
            //TargetNextPoint();
        }
        else
        {
            _playerMovement.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
            Debug.Log("end reached");
        }
        */
    }
    
    private Coroutine movementCoroutine;
    
    public void MoveToTarget(Vector2 target)
    {
        StopTargetedMovement();
        movementCoroutine = StartCoroutine(MoveToTargetCoroutine(target));
    }

    public void StopTargetedMovement()
    {
        if (movementCoroutine != null)
        {
            StopCoroutine(movementCoroutine);
            movementCoroutine = null;
        }
    }

    IEnumerator MoveToTargetCoroutine(Vector2 target)
    {
        // do your logic here

        // at the end of the coroutine:
        movementCoroutine = null;
        yield return null;
    }

}
