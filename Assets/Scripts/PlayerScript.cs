using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public GameObject[] playerPrefabs;
    int characterIndex;
    public GameObject spawnPoint;
    int[] otherPlayers;
    int index;

    private const string textFileName = "playerNames";

    // Waypoints array
    public Transform[] waypoints;  // Set this in the inspector or dynamically find waypoints in the scene

    List<GameObject> allPlayers = new List<GameObject>();

    void Start()
    {
        int characterIndex = PlayerPrefs.GetInt("SelectedCharacter", 0);

        // Spawn main player at the first waypoint
        GameObject mainCharacter = Instantiate(playerPrefabs[characterIndex], waypoints[0].position, Quaternion.identity);
        mainCharacter.GetComponent<NameScript>().SetPlayerName(PlayerPrefs.GetString("PlayerName"));

        // Set waypoints in PlayerMovement for main character
        PlayerMovement mainPlayerMovement = mainCharacter.GetComponent<PlayerMovement>();
        mainPlayerMovement.InitializeWaypoints(waypoints);  // Initialize waypoints

        Debug.Log(mainPlayerMovement.waypoints);
        // Set the initial waypoint position for the player
        mainPlayerMovement.SetWaypoint(waypoints[0].position);
        allPlayers.Add(mainCharacter);

        Debug.Log($"Main Player Spawned: {mainCharacter.name} at {waypoints[0].position}");

        int otherPlayerCount = PlayerPrefs.GetInt("PlayerCount") - 1;
        string[] nameArray = ReadLinesFromFile("playerNames");

        // Instantiate other players at the first waypoint
        for (int i = 0; i < otherPlayerCount; i++)
        {
            int index = Random.Range(0, playerPrefabs.Length);
            GameObject character = Instantiate(playerPrefabs[index], waypoints[0].position, Quaternion.identity);
            character.GetComponent<NameScript>().SetPlayerName(nameArray[Random.Range(0, nameArray.Length)]);

            // Set waypoints for the other players
            PlayerMovement characterMovement = character.GetComponent<PlayerMovement>();
            characterMovement.InitializeWaypoints(waypoints); // Initialize waypoints

            characterMovement.SetWaypoint(waypoints[0].position);
            allPlayers.Add(character);

            Debug.Log($"Other Player Spawned: {character.name} at {waypoints[0].position}");
        }
        Debug.Log(mainPlayerMovement.waypoints);


        // Now, assign waypoints to players
        AssignWaypointsToPlayers(allPlayers);
        Debug.Log(mainPlayerMovement.waypoints);
    }

    string[] ReadLinesFromFile(string fileName)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(fileName);

        if (textAsset != null)
            return textAsset.text.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        else
            Debug.LogError("File not found: " + fileName);
        return new string[0];
    }

    void AssignWaypointsToPlayers(List<GameObject> allPlayers)
    {
        int waypointIndex = 0;

        foreach (GameObject player in allPlayers)
        {
            if (waypointIndex < waypoints.Length)
            {
                // Assign the current waypoint to the player
                player.GetComponent<PlayerMovement>().SetWaypoint(waypoints[waypointIndex].position, waypointIndex);

                Debug.Log($"{player.name} assigned to Waypoint {waypointIndex} at {waypoints[waypointIndex].position}");
            }
            else
            {
                Debug.LogWarning("Not enough waypoints for all players.");
                break;
            }
        }
    }
}
