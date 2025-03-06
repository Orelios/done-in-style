using UnityEngine;
using System.Collections.Generic;

public class Zipline : MonoBehaviour
{
    [SerializeField] private float zipSpeed = 10f;
    private EdgeCollider2D edgeCollider;
    private List<Vector2> ziplinePoints;
    private Dictionary<Transform, Coroutine> activeZiplines = new Dictionary<Transform, Coroutine>();

    private void Awake()
    {
        edgeCollider = GetComponent<EdgeCollider2D>();
        ziplinePoints = new List<Vector2>(edgeCollider.points);
        for (int i = 0; i < ziplinePoints.Count; i++)
        {
            ziplinePoints[i] += (Vector2)transform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !activeZiplines.ContainsKey(collision.transform))
        {
            Transform player = collision.transform;
            player.GetComponent<PlayerMovement>().enabled = false;
            player.GetComponent<Rigidbody2D>().gravityScale = 0;
            //player.GetComponent<Rigidbody2D>().simulated = false; don't do this
            Vector2 playerPos = player.position;
            int closestIndex = FindClosestPointIndex(playerPos);
            bool movingForward = DetermineDirection(playerPos, closestIndex);

            Coroutine ziplineCoroutine = StartCoroutine(MovePlayerAlongZipline(player, closestIndex, movingForward));
            activeZiplines[player] = ziplineCoroutine;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && activeZiplines.ContainsKey(collision.transform))
        {
            StopCoroutine(activeZiplines[collision.transform]);
            activeZiplines.Remove(collision.transform);
            //collision.gameObject.GetComponent<PlayerMovement>().enabled = true;
            //collision.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
            //collision.GetComponent<Rigidbody2D>().simulated = true;
        }
    }

    private int FindClosestPointIndex(Vector2 position)
    {
        int closestIndex = 0;
        float minDistance = Vector2.Distance(position, ziplinePoints[0]);

        for (int i = 1; i < ziplinePoints.Count; i++)
        {
            float distance = Vector2.Distance(position, ziplinePoints[i]);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestIndex = i;
            }
        }
        return closestIndex;
    }

    private bool DetermineDirection(Vector2 playerPos, int closestIndex)
    {
        if (closestIndex == ziplinePoints.Count - 1) return false;
        if (closestIndex == 0) return true;
        return Vector2.Distance(playerPos, ziplinePoints[closestIndex + 1]) < Vector2.Distance(playerPos, ziplinePoints[closestIndex - 1]);
    }

    private IEnumerator<WaitForFixedUpdate> MovePlayerAlongZipline(Transform player, int startIndex, bool forward)
    {
        //player.GetComponent<Rigidbody2D>().gravityScale = 0;
        //Debug.Log("rb disabled");
        //player.gameObject.GetComponent<PlayerMovement>().enabled = false;
        //player.GetComponent<Rigidbody2D>().simulated = false;
        int index = startIndex;
        while (index >= 0 && index < ziplinePoints.Count)
        {
            Debug.Log("ziplineIndex = " + index);
            Vector2 target = ziplinePoints[index];
            while ((Vector2)player.position != target)
            {
                player.position = Vector2.MoveTowards(player.position, target, zipSpeed * Time.fixedDeltaTime);
                yield return new WaitForFixedUpdate();
            }
            index += forward ? 1 : -1;
        }
        //Debug.Log("rb enabled");
        player.gameObject.GetComponent<PlayerMovement>().enabled = true;
        player.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
        //player.GetComponent<Rigidbody2D>().simulated = true;
        activeZiplines.Remove(player);
    }
}
