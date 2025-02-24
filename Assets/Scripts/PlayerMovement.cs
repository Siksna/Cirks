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

    private int totalDiceRolls = 0;  
    private float startTime;  

    private WinScreenController winScreenController;  

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
        totalDiceRolls++;  

        if (nextWaypointIndex >= waypoints.Length - 1)
        {
            nextWaypointIndex = 36; 
            StartCoroutine(MoveThroughWaypoints(nextWaypointIndex, true)); 
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
        float moveSpeed = 3f;
        int startWaypoint = currentWaypointIndex;

        if (targetWaypointIndex >= waypoints.Length)
        {
            targetWaypointIndex = waypoints.Length - 1; 
        }

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

        if (hasWon)
        {
            if (winScreenController == null)
            {
                winScreenController = FindObjectOfType<WinScreenController>();
            }

            if (winScreenController != null)
            {
                float elapsedTime = Time.time - startTime;  
                winScreenController.ShowWinScreen(gameObject, totalDiceRolls, elapsedTime);  
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
        startTime = Time.time; 
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
