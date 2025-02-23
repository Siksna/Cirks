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
    public int diceFaceNum;
    public bool islanded = false;
    public bool firstThrow = false;

    public TurnManager turnManager;
    public GameObject player;

    private bool canRoll = true;  
    [SerializeField] private float waitTime = 0.1f;  


    private void Start()
    {
        turnManager = FindObjectOfType<TurnManager>();

        if (turnManager != null)
        {
            turnManager.SetFirstPlayer();
            player = turnManager.GetCurrentPlayer();
        }
        else
        {
            Debug.LogError("TurnManager not found in the scene!");
        }
    }

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
        if (!canRoll) return; 

        player = turnManager.GetCurrentPlayer();

        if (player == null)
        {
            Debug.LogError("Current player is null!");
            return;
        }

        Debug.Log($"{player.name} is rolling the dice.");

        rigidbody.isKinematic = false;
        islanded = false;  

        forceX = Random.Range(0, maxRandForceVal);
        forceY = Random.Range(0, maxRandForceVal);
        forceZ = Random.Range(0, maxRandForceVal);
        rigidbody.AddForce(Vector3.up * Random.Range(800, startRollingForce));
        rigidbody.AddTorque(forceX, forceY, forceZ);

        canRoll = false;  

        StartCoroutine(WaitForDiceToLand());
    }

    IEnumerator WaitForDiceToLand()
    {
        yield return new WaitUntil(() => rigidbody.IsSleeping()); 

        islanded = true;
        Debug.Log("Dice landed! Result: " + diceFaceNum);

        if (player != null)
        {
            PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();

            if (playerMovement != null)
            {
                Debug.Log($"{player.name} is moving {diceFaceNum} spaces.");
                playerMovement.MovePlayer(diceFaceNum);

                yield return new WaitUntil(() => !playerMovement.IsMoving);
            }

            yield return new WaitForSeconds(1f);
            islanded = false; 
            turnManager.NextTurn();
        }
        else
        {
            Debug.LogError("Player reference is missing in DiceRollScript!");
        }

        yield return new WaitForSeconds(waitTime); 
        canRoll = true; 
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
            if ((Input.GetMouseButton(0) && islanded) || (Input.GetMouseButton(0) && !firstThrow))
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
