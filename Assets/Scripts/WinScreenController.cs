using UnityEngine;
using TMPro;
using System.IO;  // Include System.IO to access file handling functionality

public class WinScreenController : MonoBehaviour
{
    public GameObject winScreenUI;  // UI Panel to show the win screen
    public TextMeshProUGUI winnerText;  // Text to display the winner's name
    public TextMeshProUGUI statsText;  // Text to display the stats (dice rolls and time)

    private string leaderboardFilePath = "Assets/Resources/leaderboard.txt"; // File path for the leaderboard

    // This will be called when any player wins
    public void ShowWinScreen(GameObject winningPlayer, int diceRolls, float timeElapsed)
    {
        winScreenUI.SetActive(true);  // Activate the win screen

        // Get the player's username using NameScript
        NameScript nameScript = winningPlayer.GetComponent<NameScript>();
        string playerName = "Player";
        if (nameScript != null)
        {
            playerName = nameScript.GetPlayerName();  // Retrieve the player's name from the NameScript
            winnerText.text = $"{playerName} Wins!";  // Set the winner's name
        }
        else
        {
            winnerText.text = $"Player Wins!";  // Fallback in case NameScript is missing
        }

        statsText.text = $"Total Rolls: {diceRolls}\nTime: {timeElapsed:F2} seconds";  // Set the stats

        // Save the win data to a text file
        SaveWinData(playerName, diceRolls, timeElapsed);
    }

    // Save the win data to a text file (Appends to the file)
    private void SaveWinData(string playerName, int diceRolls, float timeElapsed)
    {
        // Format the data to be written
        string data = $"{playerName} - Rolls: {diceRolls}, Time: {timeElapsed:F2} seconds\n";

        // Check if the leaderboard file exists, create it if not
        if (!File.Exists(leaderboardFilePath))
        {
            File.Create(leaderboardFilePath).Close(); // Create the file and close the stream
        }

        // Write the data to the file (append mode)
        using (StreamWriter writer = new StreamWriter(leaderboardFilePath, true))
        {
            writer.WriteLine(data);  // Write the data to the file
        }

        Debug.Log("Win data saved to leaderboard.");
    }

    // This method will be called to restart the game
    public void RestartGame()
    {
        Time.timeScale = 1;  // Unpause the game
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);  // Reload the scene (restart the game)
    }

    // Optional: Method to read and display the leaderboard from the file
    public void DisplayLeaderboard()
    {
        if (File.Exists(leaderboardFilePath))
        {
            string[] leaderboardData = File.ReadAllLines(leaderboardFilePath);
            string leaderboardText = "Leaderboard:\n";

            foreach (var entry in leaderboardData)
            {
                leaderboardText += entry + "\n";  // Append each entry to the leaderboard display
            }

            // Optionally, display the leaderboard somewhere on the screen (e.g., in a UI Text element)
            Debug.Log(leaderboardText); // Or set it to a UI Text field to show the leaderboard
        }
        else
        {
            Debug.Log("No leaderboard data found.");
        }
    }
}
