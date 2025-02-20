using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public List<Transform> boardPositions; // List of all board spaces
    private int currentPositionIndex = 0; // Player's current position
    public float moveSpeed = 5f; // Speed of movement
    public bool IsMoving { get; private set; } // Check if the player is moving


    void Start()
{
    GameManager gameManager = FindObjectOfType<GameManager>();

    if (gameManager != null)
        boardPositions = gameManager.boardPositions;
    else
        Debug.LogError("GameManager not found!");
}



    public void Move(int steps)
    {
        if (boardPositions == null || boardPositions.Count == 0)
        {
            Debug.LogError("Board positions not assigned!");
            return;
        }

        int targetIndex = currentPositionIndex + steps;
        if (targetIndex >= boardPositions.Count)
            targetIndex = boardPositions.Count - 1; // Stay within board limits

        StartCoroutine(MoveToPosition(targetIndex));
    }

    private IEnumerator MoveToPosition(int targetIndex)
    {
        IsMoving = true;
        Vector3 targetPosition = boardPositions[targetIndex].position;

        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPosition;
        currentPositionIndex = targetIndex;
        IsMoving = false;
    }
}
