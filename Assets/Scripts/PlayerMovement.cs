using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Vector3 targetWaypoint;
    public bool IsMoving { get; private set; } = false;
    public int currentWaypointIndex { get; private set; } = 0;
    public DiceRollScript diceRollScript;
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

        if (diceRollScript != null && !diceRollScript.islanded)
        {
            Debug.LogWarning("Dice hasn't landed yet! Waiting...");
            return;
        }

        int nextWaypointIndex = currentWaypointIndex + diceRoll;

        // If there aren't enough waypoints, move to the last waypoint and then move back
        if (nextWaypointIndex >= waypoints.Length)
        {
            // Move to the last valid waypoint
            nextWaypointIndex = waypoints.Length - 1;

            // Calculate how much we need to move back after reaching the last waypoint
            int remainingDiceRoll = nextWaypointIndex + 1 - (currentWaypointIndex + diceRoll);
            Debug.Log(gameObject.name + " reached the end and will move back by " + remainingDiceRoll + " spaces.");

            // Start the movement to the last waypoint
            StartCoroutine(MoveThroughWaypoints(nextWaypointIndex, remainingDiceRoll));
        }
        else if (nextWaypointIndex < waypoints.Length && diceRoll != 0)
        {
            // Normal movement without exceeding the waypoints
            StartCoroutine(MoveThroughWaypoints(nextWaypointIndex));
        }
        else
        {
            Debug.Log(gameObject.name + " cannot move further! Staying at " + currentWaypointIndex);
        }
    }


    private IEnumerator MoveThroughWaypoints(int targetWaypointIndex, int remainingDiceRoll = 0)
    {
        IsMoving = true;
        float moveSpeed = 6f;

        int startWaypoint = currentWaypointIndex;

        // First, move to the last valid waypoint (if we reached the end)
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
        }

        // If there are remaining dice rolls after reaching the end, move back
        if (remainingDiceRoll > 0)
        {
            for (int i = targetWaypointIndex - 1; i >= targetWaypointIndex - remainingDiceRoll; i--)
            {
                Vector3 targetPosition = waypoints[i].position;
                while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                    yield return null;
                }

                transform.position = targetPosition;
                currentWaypointIndex = i;
            }
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

}
