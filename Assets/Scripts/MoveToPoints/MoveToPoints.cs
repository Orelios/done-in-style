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
    private RampPlayer _rampPlayer;
    private PlayerTricks _playerTricks;
    private int index;
    private float startSpeed, elapsedTime, calculatedSpeed, actualSpeed;
    [SerializeField] private float maxSpeed = 20, accelerationDuration = 1f, speedDivider = 0.01f;
    //[SerializeField] private bool isMovingToTargetPoint = false;
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
        _rampPlayer = _playerMovement.GetComponent<RampPlayer>();
    }
    
    private void Update()
    {
        /*
        if (isMovingToTargetPoint)
        {
            if (localIsFacingRight != _playerMovement.IsFacingRight)
            {
                localIsFacingRight = _playerMovement.IsFacingRight;
            }
        }
        */
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            StopTargetedMovement();
            _playerMovement.isMovingToTargetPoint = false;
            MakeDynamic();
            _playerMovement.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
            //_playerMovement.Jump();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !_playerMovement.isMovingToTargetPoint)
        {
            _playerMovement = other.gameObject.GetComponent<PlayerMovement>();
            _playerMovement.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
            MakeKinematic();
            //localIsFacingRight = _playerMovement.IsFacingRight;
            SetSpeed();
            DetrmineTargetPoint(); // also starts movementCoroutine
            //StartCoroutine(MoveToTarget());
            elapsedTime = 0;
        }
    }

    public void SetSpeed()
    {
        startSpeed = Math.Abs(_playerMovement.Rb.linearVelocityX);
        //Debug.Log("startSpeed = " + startSpeed);
        //calculatedSpeed = startSpeed;
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
                    ZeroPlayerPhysics();
                    targetPos = movePoints[i].transform.position;
                    //Debug.Log("Initial Target: " + movePoints[i].gameObject.name);
                    index = i;
                    MoveToTarget(targetPos);
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
                    ZeroPlayerPhysics();
                    //_playerMovement.transform.position = movePoints[i].transform.position;
                    targetPos = movePoints[i].transform.position;
                    //Debug.Log("Initial Target: " + movePoints[i].gameObject.name + " reversed");
                    index = i;
                    MoveToTarget(targetPos);
                    break;
                }
            }
        }

    }

    public void TargetNextPoint(int i)
    {
        MakeKinematic();
        if (!_playerMovement.isMovingToTargetPoint) //happens only when colliding with points instead of edge collider (first time only)
        {
            SetSpeed();
        }

        if (_playerMovement.IsFacingRight)
        {
            if (i >= movePoints.Count - 1)
            {
                //end of list, no next target
                StopTargetedMovement();
                _playerMovement.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
                MakeDynamic();
                StartMomentum();
                //Debug.Log(" end");
            }
            else
            {
                StopTargetedMovement();
                ZeroPlayerPhysics();

                targetPos = movePoints[i + 1].transform.position;
                //Debug.Log(movePoints[i + 1].gameObject.name);
                MoveToTarget(targetPos);
            }
        }
        else if (!_playerMovement.IsFacingRight)
        {
            if (i <= 0)
            {
                //start of list, no next target
                StopTargetedMovement();
                _playerMovement.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
                MakeDynamic();
                StartMomentum();
                //Debug.Log(" reversed end");
            }
            else
            {
                StopTargetedMovement();
                ZeroPlayerPhysics();
                targetPos = movePoints[i - 1].transform.position;
                //Debug.Log(movePoints[i - 1].gameObject.name + " reversed");
                MoveToTarget(targetPos);
            }
        }

    }

    

    public void TeleportToPoint(int i)
    {
        _playerMovement.transform.position = movePoints[i].transform.position;
    }

    public void ZeroPlayerPhysics()
    {
        _playerMovement.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        _playerMovement.Rb.linearVelocityY = 0;
        //_playerMovement.Rb.linearVelocityX = 0;
    }

    public void MakeKinematic()
    {
        _playerMovement.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        //Debug.Log("Kinematic");
    }

    public void MakeDynamic()
    {
        _playerMovement.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        //Debug.Log("Dynamic");
    }

    /*
    private IEnumerator MoveToTarget()
    {
        isMovingToTargetPoint = true;
        Vector2 currentTarget = targetPos;
        while (_playerMovement.transform.position.x != currentTarget.x)
        {
            _playerMovement.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
            _playerMovement.Rb.linearVelocityY = 0;
            _playerMovement.Rb.linearVelocityX = 0;

            calculatedSpeed = Mathf.Lerp(startSpeed, maxSpeed, elapsedTime / accelerationDuration);
            if (elapsedTime < accelerationDuration)
            {
                elapsedTime += Time.deltaTime;
            }
            else if (elapsedTime >= accelerationDuration)
            {
                elapsedTime = accelerationDuration;
            }
            
            _playerMovement.transform.position = Vector2.MoveTowards(_playerMovement.transform.position, currentTarget, calculatedSpeed * Time.fixedDeltaTime);
            yield return null;
        }
        isMovingToTargetPoint = false;
         not used because task of choosing next target is based on int parameter passed by MovePoint child
        if ((_playerMovement.IsFacingRight && index < movePoints.Count-1) || (!_playerMovement.IsFacingRight && index <= 0))
        {
            //TargetNextPoint();
        }
        else
        {
            _playerMovement.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
            Debug.Log("end reached");
        }
        
    }
    */

    private Coroutine movementCoroutine, momentumCoroutine;
    
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
            _playerMovement.isMovingToTargetPoint = false;
        }
    }

    IEnumerator MoveToTargetCoroutine(Vector2 target)
    {
        // do your logic here
        _playerMovement.isMovingToTargetPoint = true;
        while (_playerMovement.transform.position.x != targetPos.x)
        {
            ZeroPlayerPhysics();

            calculatedSpeed = Mathf.Lerp(startSpeed, maxSpeed, elapsedTime / accelerationDuration);
            //Debug.Log("calculatedSpeed = " +  calculatedSpeed);
            actualSpeed = calculatedSpeed * speedDivider;
            if (elapsedTime < accelerationDuration)
            {
                elapsedTime += Time.deltaTime;
            }
            else if (elapsedTime >= accelerationDuration)
            {
                elapsedTime = accelerationDuration;
            }

            _playerMovement.transform.position = Vector2.MoveTowards(_playerMovement.transform.position, targetPos, actualSpeed);
            yield return null;
        }
        //isMovingToTargetPoint = false;

        // at the end of the coroutine:
        movementCoroutine = null;
        yield return null;
    }

    public void StartMomentum()
    {
        //Debug.Log("momentum next line");
        //momentumCoroutine = StartCoroutine(PreserveMomentum());
        //Debug.Log("momentum called");
    }

    public void StopMomentum()
    {
        if (momentumCoroutine != null)
        {
            StopCoroutine(momentumCoroutine);
            momentumCoroutine = null;
        }
    }

    IEnumerator PreserveMomentum()
    {
        //_playerMovement.Rb.linearVelocityX = actualSpeed;
        Debug.Log("momentum started");
        while (!Input.anyKeyDown)
        {
            if (_rampPlayer.IsColliding) { break; }
            //dashLastVelocity = _playerMovement.Rb.linearVelocity.y;
            //_playerMovement.Rb.linearVelocity -= _dashMomentumDecay;
            Vector2 lastVelocity = new Vector2(actualSpeed, 0f);
            if (lastVelocity.x > 0)
            {
                _playerMovement.Rb.linearVelocity = new Vector2(lastVelocity.x - _playerTricks.DashMomentumDecay.x, lastVelocity.y - _playerTricks.DashMomentumDecay.y);
            }
            else
            {
                _playerMovement.Rb.linearVelocity = new Vector2(lastVelocity.x + _playerTricks.DashMomentumDecay.x, lastVelocity.y - _playerTricks.DashMomentumDecay.y);
            }

            yield return null;
        }
    }
}
