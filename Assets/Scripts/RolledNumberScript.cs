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
    

    void Update()
    {
        if (diceRollScript != false)
        {
            if (diceRollScript.islanded)
               rolledNumberText.text = diceRollScript.diceFaceNum.ToString();

            else
                rolledNumberText.text = "?";
        }
        else
            Debug.LogError("DiceRollScript not found in a scene");
    }
}
