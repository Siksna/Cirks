using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnManager : MonoBehaviour
{
    public List<GameObject> allPlayers = new List<GameObject>();
    public GameObject currentPlayer;
    private int currentPlayerIndex = 0;

    public TextMeshProUGUI playerTurnText; // Reference to TextMeshPro UI element

    void Start()
    {
        if (allPlayers.Count > 0)
        {
            currentPlayer = allPlayers[currentPlayerIndex];
            Debug.Log($"Starting game with {allPlayers.Count} players. First player: {currentPlayer.name}");

            UpdatePlayerTurnText();  // Display current player's name
        }
        else
        {
            Debug.LogError("No players found in TurnManager!");
        }
    }

    public void SetFirstPlayer()
    {
        if (allPlayers.Count > 0)
        {
            currentPlayerIndex = 0; // Set to the first player
            Debug.Log("First player set: " + allPlayers[currentPlayerIndex].name);
        }
        else
        {
            Debug.LogError("No players found in TurnManager!");
        }
    }

    public GameObject GetCurrentPlayer()
    {
        return currentPlayer;
    }

    public void NextTurn()
    {
        currentPlayerIndex = (currentPlayerIndex + 1) % allPlayers.Count;
        currentPlayer = allPlayers[currentPlayerIndex];

        UpdatePlayerTurnText();
    }

    private void UpdatePlayerTurnText()
    {
        if (playerTurnText != null && currentPlayer != null)
        {
            NameScript nameScript = currentPlayer.GetComponent<NameScript>();
            if (nameScript != null)
            {
                string playerName = nameScript.GetPlayerName();

                playerTurnText.text = $"{playerName} turn";

                playerTurnText.color = nameScript.tMP.color;
            }
            else
            {
                Debug.LogWarning("NameScript component not found on current player.");
            }
        }
        else
        {
            Debug.LogWarning("TextMeshPro UI reference or currentPlayer is missing.");
        }
    }
}
