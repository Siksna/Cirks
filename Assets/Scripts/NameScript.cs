using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class NameScript : MonoBehaviour
{
    public TextMeshPro tMP;

    void Awake()
    {
        tMP = transform.Find("NameField").gameObject.GetComponent<TextMeshPro>();
    }

    public void SetPlayerName(string name)
    {
        tMP.text = name;
        tMP.color = new Color32((byte)Random.Range(0, 255),
        (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), 255);
    }

    public string GetPlayerName()
    {
        return tMP.text;
    }
}

