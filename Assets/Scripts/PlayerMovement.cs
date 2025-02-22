using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Vector3 targetWaypoint;
    private bool isMoving = false;
    public int currentWaypointIndex { get; private set; } = 0;  // Track the player's waypoint

    public Transform[] waypoints; // Waypoints reference

    // This function initializes the waypoints array from PlayerScript
    public void InitializeWaypoints(Transform[] waypointsArray)
    {
        if (waypointsArray == null || waypointsArray.Length == 0)
        {
            Debug.LogError("No waypoints provided!");
            return;
        }

        waypoints = waypointsArray; // Set the waypoints reference from PlayerScript
        Debug.Log("Waypoints initialized with " + waypoints.Length + " waypoints.");
        // Log all waypoint positions
        foreach (var waypoint in waypoints)
        {
            Debug.Log("Waypoint position: " + waypoint.position);
        }



    }

    public void MovePlayer(int diceRoll)
    {
        if (waypoints == null || waypoints.Length == 0)
        {
            Debug.LogError("Waypoints not set in PlayerMovement!     ..." + waypoints);
            return;
        }

        int nextWaypointIndex = currentWaypointIndex + diceRoll;

        // Ensure the player doesn't go out of bounds
        if (nextWaypointIndex < waypoints.Length)
        {
            SetWaypoint(waypoints[nextWaypointIndex].position, nextWaypointIndex);
        }
        else
        {
            Debug.Log(gameObject.name + " cannot move further! Staying at " + currentWaypointIndex);
        }
    }

    public void SetWaypoint(Vector3 waypoint, int waypointIndex = 0)
    {
        targetWaypoint = waypoint;
        currentWaypointIndex = waypointIndex;
        isMoving = true;
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
        if (isMoving)
        {
            float step = Time.deltaTime * 3f;
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint, step);

            if (Vector3.Distance(transform.position, targetWaypoint) < 0.01f) // Small threshold for precision
            {
                isMoving = false;
                Debug.Log(gameObject.name + " reached waypoint " + currentWaypointIndex);
            }
        }
    }
}
