using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq; // Needed for List operations

public class GameManager : MonoBehaviour
{
    public List<PlayerMovement> players = new List<PlayerMovement>();
    public List<Transform> boardPositions = new List<Transform>();
    private int currentPlayerIndex = 0;
    public DiceRollScript diceRollScript;
    public TMP_Text turnText;

    private void Start()
    {
        StartCoroutine(FindPlayers());
    }

    private IEnumerator FindPlayers()
{
    // Keep checking until at least one player is found
    while (players.Count == 0)
    {
        yield return new WaitForSeconds(0.5f); // Delay to allow spawning
        players = FindObjectsOfType<PlayerMovement>().ToList();
    }

    Debug.Log("Players found: " + players.Count);

    foreach (var player in players)
    {
        Debug.Log("Found player: " + player.name);  // Logs the name of each player found
    }

    UpdateTurnUI();
}


    public void OnDiceClicked()
    {
        if (players.Count == 0) return; // Ensure players exist
        if (!players[currentPlayerIndex].IsMoving)
        {
            diceRollScript.RollDice();
            StartCoroutine(WaitForDiceResult());
        }
    }

    private IEnumerator WaitForDiceResult()
    {
        yield return new WaitUntil(() => diceRollScript.islanded);

        int roll;
        if (int.TryParse(diceRollScript.diceFaceNum, out roll))
        {
            players[currentPlayerIndex].Move(roll);
        }
        else
        {
            Debug.LogError("Invalid dice roll result: " + diceRollScript.diceFaceNum);
        }

        yield return new WaitUntil(() => !players[currentPlayerIndex].IsMoving);
        NextTurn();
    }

    private void NextTurn()
    {
        currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
        UpdateTurnUI();
    }

    private void UpdateTurnUI()
    {
        turnText.text = "Player " + (currentPlayerIndex + 1) + "'s Turn";
    }
}
