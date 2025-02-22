using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideDetectionScript : MonoBehaviour
{
    DiceRollScript diceRollScript;

    private void Awake()
    {
        diceRollScript = FindObjectOfType<DiceRollScript>();
    }

    private void OnTriggerStay(Collider sideCollider)
    {
        if (diceRollScript != null)
        {
            if (diceRollScript.GetComponent<Rigidbody>().velocity == Vector3.zero)
            {
                diceRollScript.islanded = true;

                // Convert the side name (string) to an integer
                if (int.TryParse(sideCollider.name, out int diceValue))
                {
                    diceRollScript.diceFaceNum = diceValue;
                }
                else
                {
                    Debug.LogError($"Failed to parse dice face number: {sideCollider.name}");
                }
            }
            else
            {
                diceRollScript.islanded = false;
            }
        }
        else
        {
            Debug.LogError("DiceRollScript not found in scene.");
        }
    }
}
