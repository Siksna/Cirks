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

    private int totalDiceRolls = 0;  // Track the total dice rolls
    private float startTime;  // Start time to calculate elapsed time

    private WinScreenController winScreenController;  // Reference to WinScreenController

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
        totalDiceRolls++;  // Increment dice roll count

        // Check if the player reached the last waypoint (36th waypoint)
        if (nextWaypointIndex >= waypoints.Length - 1)
        {
            nextWaypointIndex = 36; // Ensure they stop exactly at waypoint 36
            StartCoroutine(MoveThroughWaypoints(nextWaypointIndex, true)); // Trigger win condition
        }
        else if (nextWaypointIndex < waypoints.Length && diceRoll != 0)
        {
            StartCoroutine(MoveThroughWaypoints(nextWaypointIndex));
        }
        else
        {
            Debug.Log(gameObject.name + " cannot move further! Staying at " + currentWaypointIndex);
        }
    }

    private IEnumerator MoveThroughWaypoints(int targetWaypointIndex, bool hasWon = false)
    {
        IsMoving = true;
        float moveSpeed = 6f;
        int startWaypoint = currentWaypointIndex;

        // Check if the target index goes beyond the available waypoints
        if (targetWaypointIndex >= waypoints.Length)
        {
            targetWaypointIndex = waypoints.Length - 1; // Stay at the last waypoint if going beyond
        }

        // Move forward to the target waypoint
        for (int i = startWaypoint + 1; i <= targetWaypointIndex; i++)
        {
            Vector3 targetPosition = waypoints[i].position;
            Debug.Log(targetPosition + " Moving...");

            while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }

            transform.position = targetPosition;
            currentWaypointIndex = i;
        }

        // Trigger the win condition if the player has won
        if (hasWon)
        {
            if (winScreenController == null)
            {
                // Find the WinScreenController dynamically if not already assigned
                winScreenController = FindObjectOfType<WinScreenController>();
            }

            if (winScreenController != null)
            {
                float elapsedTime = Time.time - startTime;  // Calculate the elapsed time
                winScreenController.ShowWinScreen(gameObject, totalDiceRolls, elapsedTime);  // Display the win screen
            }
            else
            {
                Debug.LogError("WinScreenController not found in the scene!");
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
        startTime = Time.time; // Start timer when game begins
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
