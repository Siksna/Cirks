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

    public GameObject GetCurrentPlayer()
    {
        return currentPlayer;
    }

    public void NextTurn()
    {
        Debug.Log($"Switching turn... Current Index: {currentPlayerIndex}, Current Player: {currentPlayer.name}");

        currentPlayerIndex = (currentPlayerIndex + 1) % allPlayers.Count;
        currentPlayer = allPlayers[currentPlayerIndex];

        Debug.Log($"New Turn: {currentPlayer.name} (Index: {currentPlayerIndex})");

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

                playerTurnText.text = $"{playerName}'s Turn";
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
