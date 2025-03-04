using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Moving : MonoBehaviour
{
    /*// Editable variables in Inspector
    [SerializeField] private Vector2 targetPos;  // Target position to move towards
    [SerializeField] private float speed = 2f;   // Speed of the platform movement
    [SerializeField] private float moveDelay = 0.5f; //Delay before platform moves back after reaching destination

    private Transform player;
    private Vector2 currentPos;  // The current position of the platform
    private float timeToMoveBack; // Time tracker to wait before moving back

    private bool isMovingToTarget = true; // Flag to check direction of movement

    // Start is called before the first frame update
    void Start()
    {
        // Register the initial position of the platform as currentPos
        currentPos = transform.position;

        if (targetPos == Vector2.zero) // If targetPos is not set, log a warning
        {
            Debug.LogWarning("Target position for " + gameObject.name + " is not set in the Inspector!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (targetPos == Vector2.zero) return; // Do nothing if no target position

        if (isMovingToTarget)
        {
            // Move the platform towards the target position
            float step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, targetPos, step);

            // If the platform reaches the target position
            if ((Vector2)transform.position == targetPos)
            {
                // Swap currentPos and targetPos
                Vector2 temp = currentPos;
                currentPos = targetPos;
                targetPos = temp;

                // Start a timer before moving back
                timeToMoveBack = Time.time + moveDelay;
                isMovingToTarget = false; // Stop moving towards target
            }
        }
        else
        {
            // Check if it's time to start moving back after delay
            if (Time.time >= timeToMoveBack)
            {
                isMovingToTarget = true; // Start moving to the new target
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth)
            && collision.gameObject.GetComponent<PlayerInputManager>().HorizontalMovement == 0)
        {
            collision.transform.SetParent(transform); // Attach player to platform when standing on it
            //Debug.Log("working");
            //collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        }
        if(collision.gameObject.GetComponent<PlayerInputManager>().HorizontalMovement != 0)
        {
            collision.transform.SetParent(null);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth))
        {
            collision.transform.SetParent(null); // Detach player from platform when they leave it
        }
    }*/

    /*
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the colliding object has the "Player" tag
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.transform;

            player.transform.parent = transform;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // Check if the colliding object has the "Player" tag
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.transform;

            player.transform.parent = null;
        }
    }
    */

    [SerializeField] private GameObject objectToMove;
    
    public List<Transform> TravelPoints;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float delayBeforeMoving;
    
    private int _currentPointIndex;
    private bool _isMoving;

    private void Start()
    {
        if (TravelPoints.Count < 2)
        {
            Debug.LogError($"{name} requires at least 2 travel points.");
        }
    }

    private void Update()
    {
        if (!_isMoving)
        {
            StartCoroutine(MoveRoutine());
        }
    }

    private IEnumerator MoveRoutine()
    {
        _isMoving = true;
        var targetPosition = new Vector3(TravelPoints[_currentPointIndex].position.x, TravelPoints[_currentPointIndex].position.y, transform.position.z);

        while (Vector3.Distance(objectToMove.transform.position, targetPosition) > 0.1f)
        {
            objectToMove.transform.transform.position = Vector3.MoveTowards(objectToMove.transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
        
        yield return new WaitForSeconds(delayBeforeMoving);
        
        _currentPointIndex = (_currentPointIndex + 1) % TravelPoints.Count;
        _isMoving = false;
    }

    //Create a new travel point, set its position as the platform's, set it as a child of the platform, and include its Transform in the list of travel points 
    public void AddNewTravelPoint()
    {
        var newTravelPoint = new GameObject($"Travel Point {TravelPoints.Count + 1}")
        {
            transform =
            {
                position = transform.position,
                parent = transform.GetChild(1)
            }
        };
        
        TravelPoints.Add(newTravelPoint.transform);
    }
}

[CustomEditor(typeof(Moving))]
public class MovingPlatformEditor : Editor
{
    Moving Moving => target as Moving;
    
    private void OnSceneGUI()
    {
        Handles.color = Color.magenta;

        if (Selection.activeGameObject == Moving.gameObject)
        {
            foreach (var point in Moving.TravelPoints.Where(point => point != null))
            {
                EditorGUI.BeginChangeCheck();
                
                var newPosition = Handles.FreeMoveHandle(point.position, 1f, Vector3.zero, Handles.SphereHandleCap);

                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(point, "Move Travel Point");
                    point.position = newPosition;
                }
            }
        }
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        if (GUILayout.Button("Add New Travel Point"))
        {
            Moving.AddNewTravelPoint();
        }
    }
}
