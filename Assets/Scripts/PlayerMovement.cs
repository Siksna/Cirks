using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Vector3 targetWaypoint;
    public bool IsMoving { get; private set; } = false;
    public int currentWaypointIndex { get; private set; } = 0;  

    public Transform[] waypoints;

    public void InitializeWaypoints(Transform[] waypointsArray)
    {
        if (waypointsArray == null || waypointsArray.Length == 0)
        {
            Debug.LogError("No waypoints provided!");
            return;
        }

        waypoints = waypointsArray;
        Debug.Log("Waypoints initialized with " + waypoints.Length + " waypoints.");
        foreach (var waypoint in waypoints)
        {
            Debug.Log("Waypoint position: " + waypoint.position);
        }



    }

    public void MovePlayer(int diceRoll)
    {
        if (waypoints == null || waypoints.Length == 0)
        {
            Debug.LogError("Waypoints not set in PlayerMovement!");
            return;
        }

        int nextWaypointIndex = currentWaypointIndex + diceRoll;

        if (nextWaypointIndex < waypoints.Length)
        {
            StartCoroutine(MoveThroughWaypoints(nextWaypointIndex));
        }
        else
        {
            Debug.Log(gameObject.name + " cannot move further! Staying at " + currentWaypointIndex);
        }
    }

    private IEnumerator MoveThroughWaypoints(int targetWaypointIndex)
    {
        IsMoving = true;
        float moveSpeed = 3f;

        
        int startWaypoint = currentWaypointIndex;

        for (int i = startWaypoint + 1; i <= targetWaypointIndex; i++)
        {
            Vector3 targetPosition = waypoints[i].position;

            while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }

            
            transform.position = targetPosition;
            currentWaypointIndex = i;  

            Debug.Log(gameObject.name + " reached waypoint " + currentWaypointIndex);
        }

        IsMoving = false;
    }



    public void SetWaypoint(Vector3 waypoint, int waypointIndex = 0)
    {
        targetWaypoint = waypoint;
        currentWaypointIndex = waypointIndex;
        IsMoving = true;
    }

    void Start()
    {
        if (waypoints == null || waypoints.Length == 0)
        {
            Debug.LogError("Waypoints not set in PlayerMovement at the start!");
        }
        else
        {
            Debug.Log("Waypoints set in PlayerMovement. Ready to move.");
        }
    }

    void Update()
    {
        if (IsMoving)
        {
            float step = Time.deltaTime * 3f;
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint, step);

            if (Vector3.Distance(transform.position, targetWaypoint) < 0.01f) 
            {
                IsMoving = false;
                Debug.Log(gameObject.name + " reached waypoint " + currentWaypointIndex);
            }
        }
    }
}
