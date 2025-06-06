using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    public float grip = 10.0f;
    public float rotationSpeed = 3.0f;
    public float digSpeedMultiplier = 1.0f;

    public Camera cam;

    private float hMovement;
    private float vMovement;
    public bool interact1;

    public int hitPoints;
    public int score;

    private MeshRenderer playerMesh;
    private Material playerMeshMaterial;
    private Rigidbody rigidB;

    public bool movementDisabled = false;
    public bool playerInteracting = false;
    private bool isInvulnerable = false;

    public float invulnerableTime = 3f;
    private float blinkingTimer;

    public delegate void OnGettingCaught();
    public event OnGettingCaught GettingCaught;

    public delegate void OnRespawn();
    public event OnRespawn Respawned;

    public delegate void Interact();
    public event Interact Interacting;


    public static PlayerController Instance { get; private set; }



    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        GettingCaught += EnemyCatchesPlayer;
        Respawned += Respawn;

        rigidB = GetComponent<Rigidbody>();
        playerMesh = transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();
        playerMeshMaterial = playerMesh.GetComponent<Renderer>().material;

        gameObject.tag = "Player";

        hitPoints = 3;
        score = 0;
    }

    private void Update()
    {
        hMovement = Input.GetAxis("Horizontal");
        vMovement = Input.GetAxis("Vertical");
        interact1 = Input.GetButton("Interact1");

        if (interact1)
        {
            playerInteracting = true;
        }
        else
        {
            playerInteracting = false;
        }

        PlayerMovement();
        blinkingTimer += Time.deltaTime;
    }

    private void PlayerMovement()
    {
        if (movementDisabled || playerInteracting)
        {
            return;
        }

        if (!(hMovement == 0 && vMovement == 0))
        {
            Vector3 newPosition = new Vector3(hMovement, 0f, vMovement);
            newPosition.Normalize();

            rigidB.MovePosition(rigidB.position + (newPosition * moveSpeed * Time.deltaTime));
            PlayerRotation(hMovement, vMovement);
        }
        else
        {
            rigidB.MovePosition(rigidB.position);
        }
    }

    private void PlayerRotation(float h, float v)
    {
        Vector3 direction = new Vector3(h * 10, 0f, v * 10);

        if (direction.magnitude == 0) { return; }
        direction = Quaternion.Euler(0, cam.gameObject.transform.rotation.eulerAngles.y, 0) * direction;

        var rotation = Quaternion.LookRotation(direction);

        rigidB.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed));
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" && isInvulnerable == false)
        {

            GettingCaught?.Invoke();
        }
    }


    private void EnemyCatchesPlayer()
    {
        isInvulnerable = true;
        movementDisabled = true;
        hitPoints -= 1;

        Invoke("Respawning", 3f);

    }

    private void Respawning()
    {
        Respawned?.Invoke();
    }


    private void Respawn()
    {
        transform.SetPositionAndRotation(GameManager.Instance.PlayerSpawn.position, GameManager.Instance.PlayerSpawn.rotation);
        StartCoroutine(InvulnerableTime());
    }

    private IEnumerator InvulnerableTime()
    {
        blinkingTimer = 0;
        movementDisabled = false;

        //Blinking player mesh while invulnerable
        while (invulnerableTime > blinkingTimer)
        {
            if (playerMesh.enabled)
            {
                playerMesh.enabled = false;
            }
            else
            {
                playerMesh.enabled = true;
            }


            yield return new WaitForSeconds(.2f);
        }


        playerMesh.enabled = true;
        isInvulnerable = false;

        yield break;
    }

    public void AddScore(int value)
    {
        score += value;
    }


}
