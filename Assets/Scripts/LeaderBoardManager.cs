using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderboardManager : MonoBehaviour
{
    public TextMeshProUGUI leaderboardText; 

    private List<PlayerData> leaderboard = new List<PlayerData>(); 

    [System.Serializable]
    public class PlayerData
    {
        public string playerName;
        public int diceRolls;
        public float timeElapsed;

        public PlayerData(string name, int rolls, float time)
        {
            playerName = name;
            diceRolls = rolls;
            timeElapsed = time;
        }
    }

    void LoadLeaderboard()
    {
        TextAsset leaderboardFile = Resources.Load<TextAsset>("leaderboard");

        if (leaderboardFile != null)
        {
            string[] lines = leaderboardFile.text.Split('\n');

            leaderboard.Clear();

            Debug.Log($"Leaderboard file found! {lines.Length} lines detected.");

            foreach (var line in lines)
            {
                string cleanLine = line.Trim(); 

                if (string.IsNullOrWhiteSpace(cleanLine))
                    continue; 

                Debug.Log($"Reading line: {cleanLine}");

                string[] parts = cleanLine.Split(new[] { " - " }, System.StringSplitOptions.None);

                if (parts.Length < 2)
                {
                    Debug.LogError($"Invalid format: {cleanLine}");
                    continue;
                }

                string playerName = parts[0].Trim(); 
                string statsPart = parts[1].Trim();

                int rollsStart = statsPart.IndexOf("Rolls:") + "Rolls:".Length;
                int rollsEnd = statsPart.IndexOf(",");
                if (rollsStart < "Rolls:".Length || rollsEnd == -1)
                {
                    Debug.LogError($"Failed to find 'Rolls' data in line: {cleanLine}");
                    continue;
                }
                string rollsString = statsPart.Substring(rollsStart, rollsEnd - rollsStart).Trim();

                int timeStart = statsPart.IndexOf("Time:") + "Time:".Length;
                int timeEnd = statsPart.IndexOf("seconds");
                if (timeStart < "Time:".Length || timeEnd == -1)
                {
                    Debug.LogError($"Failed to find 'Time' data in line: {cleanLine}");
                    continue;
                }
                string timeString = statsPart.Substring(timeStart, timeEnd - timeStart).Trim();

                if (int.TryParse(rollsString, out int diceRolls) && float.TryParse(timeString, out float timeElapsed))
                {
                    leaderboard.Add(new PlayerData(playerName, diceRolls, timeElapsed));
                    Debug.Log($"Added player: {playerName} - Rolls: {diceRolls}, Time: {timeElapsed}");
                }
                else
                {
                    Debug.LogError($"Failed to parse numbers in line: {cleanLine}");
                }
            }

            leaderboard.Sort((x, y) => x.diceRolls.CompareTo(y.diceRolls));
        }
        else
        {
            Debug.LogError("Leaderboard file not found in Resources!");
        }
    }



    void DisplayLeaderboard()
    {

        foreach (var player in leaderboard)
        {
            leaderboardText.text += $"{player.playerName} - Rolls: {player.diceRolls} - Time: {player.timeElapsed:F2}s\n";
        }
    }

    public void UpdateLeaderboardDisplay()
    {
        LoadLeaderboard(); 
        DisplayLeaderboard(); 
    }

    void Start()
    {
        UpdateLeaderboardDisplay(); 
    }
}
