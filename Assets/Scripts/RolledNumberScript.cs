using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class RolledNumberScript : MonoBehaviour
{
    DiceRollScript diceRollScript;
    [SerializeField]
    TMP_Text rolledNumberText;

    private void Awake()
    {
        diceRollScript = FindObjectOfType<DiceRollScript>();
    }
    

    // Update is called once per frame
    void Update()
    {
        if (diceRollScript != false)
        {
            if (diceRollScript.islanded)
                rolledNumberText.text = diceRollScript.diceFaceNum;

            else
                rolledNumberText.text = "?";
        }
        else
            Debug.LogError("DiceRollScript not found in a scene");
    }
}
