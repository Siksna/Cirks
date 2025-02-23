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

    public Transform[] waypoints; 

    List<GameObject> allPlayers = new List<GameObject>();

    void Start()
{
    TurnManager turnManager = FindObjectOfType<TurnManager>(); 

    int characterIndex = PlayerPrefs.GetInt("SelectedCharacter", 0);
    GameObject mainCharacter = Instantiate(playerPrefabs[characterIndex], waypoints[0].position, Quaternion.identity);
    mainCharacter.GetComponent<NameScript>().SetPlayerName(PlayerPrefs.GetString("PlayerName"));
    mainCharacter.GetComponent<PlayerMovement>().InitializeWaypoints(waypoints);

   
    allPlayers.Add(mainCharacter);
     turnManager.allPlayers.Add(mainCharacter);


        int otherPlayerCount = PlayerPrefs.GetInt("PlayerCount") - 1;
    string[] nameArray = ReadLinesFromFile("playerNames");

    for (int i = 0; i < otherPlayerCount; i++)
    {
        int index = Random.Range(0, playerPrefabs.Length);
        GameObject character = Instantiate(playerPrefabs[index], waypoints[0].position, Quaternion.identity);
        character.GetComponent<NameScript>().SetPlayerName(nameArray[Random.Range(0, nameArray.Length)]);
        character.GetComponent<PlayerMovement>().InitializeWaypoints(waypoints);

       
        allPlayers.Add(character);
        turnManager.allPlayers.Add(character); 
    }

    turnManager.currentPlayer = turnManager.allPlayers[0]; 
    Debug.Log("All players added to TurnManager");



        AssignWaypointsToPlayers(allPlayers);
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
