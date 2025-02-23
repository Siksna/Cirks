using UnityEngine;
using TMPro;
using System.IO;  

public class WinScreenController : MonoBehaviour
{
    public GameObject winScreenUI;  
    public TextMeshProUGUI winnerText;  
    public TextMeshProUGUI statsText;  

    private string leaderboardFilePath = "Assets/Resources/leaderboard.txt"; 

    
    public void ShowWinScreen(GameObject winningPlayer, int diceRolls, float timeElapsed)
    {
        winScreenUI.SetActive(true);  

        
        NameScript nameScript = winningPlayer.GetComponent<NameScript>();
        string playerName = "Player";
        if (nameScript != null)
        {
            playerName = nameScript.GetPlayerName();  
            winnerText.text = $"{playerName} Wins!";  
        }
        else
        {
            winnerText.text = $"Player Wins!";  
        }

        statsText.text = $"Total Rolls: {diceRolls}\nTime: {timeElapsed:F2} seconds";  

        
        SaveWinData(playerName, diceRolls, timeElapsed);
    }

    
    private void SaveWinData(string playerName, int diceRolls, float timeElapsed)
    {
        
        string data = $"{playerName} - Rolls: {diceRolls}, Time: {timeElapsed:F2} seconds\n";

        if (!File.Exists(leaderboardFilePath))
        {
            File.Create(leaderboardFilePath).Close(); 
        }

        using (StreamWriter writer = new StreamWriter(leaderboardFilePath, true))
        {
            writer.WriteLine(data);  
        }

        Debug.Log("Win data saved to leaderboard.");
    }

    public void RestartGame()
    {
        Time.timeScale = 1;  
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);  
    }

    public void DisplayLeaderboard()
    {
        if (File.Exists(leaderboardFilePath))
        {
            string[] leaderboardData = File.ReadAllLines(leaderboardFilePath);
            string leaderboardText = "Leaderboard:\n";

            foreach (var entry in leaderboardData)
            {
                leaderboardText += entry + "\n";  
            }

            Debug.Log(leaderboardText); 
        }
        else
        {
            Debug.Log("No leaderboard data found.");
        }
    }
}
