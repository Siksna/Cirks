using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DiceRollScript : MonoBehaviour
{
    Rigidbody rigidbody;
    Vector3 position;
    [SerializeField] private float maxRandForceVal, startRollingForce;
    float forceX, forceY, forceZ;
    public int diceFaceNum; // Changed from string to int
    public bool islanded = false;
    public bool firstThrow = false;

    public GameObject player; // Reference to the player object

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.isKinematic = true;
        position = transform.position;
        transform.rotation = new Quaternion(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360), 0);
    }

    public void RollDice()
    {
        // Call MovePlayer after dice lands
        StartCoroutine(WaitForDiceToLand());
        
        rigidbody.isKinematic = false;
        forceX = Random.Range(0, maxRandForceVal);
        forceY = Random.Range(0, maxRandForceVal);
        forceZ = Random.Range(0, maxRandForceVal);
        rigidbody.AddForce(Vector3.up * Random.Range(200, startRollingForce));
        rigidbody.AddTorque(forceX, forceY, forceZ);
    }

    IEnumerator WaitForDiceToLand()
    {
        yield return new WaitUntil(() => islanded);
        
        // Ensure diceFaceNum is set correctly before calling MovePlayer
        if (player != null)
        {
            player.GetComponent<PlayerMovement>().MovePlayer(diceFaceNum); // HEHREREEEEEE
        }
        else
        {
            Debug.LogError("Player reference is missing in DiceRollScript!");
        }
    }

    public void ResetDice()
    {
        rigidbody.isKinematic = true;
        firstThrow = false;
        islanded = false;
        transform.position = position;
    }

    private void Update()
    {
        if (rigidbody != null)
        {
            if (Input.GetMouseButton(0) && islanded || Input.GetMouseButton(0) && !firstThrow)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider != null && hit.collider.gameObject == this.gameObject)
                    {
                        if (!firstThrow)
                            firstThrow = true;

                        RollDice();
                    }
                }
            }
        }
    }
}
