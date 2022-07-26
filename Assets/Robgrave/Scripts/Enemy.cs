using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent navAgent;
    private GameObject player;

    private Material myMaterial;

    [SerializeField] private GameObject _debugSphere;
    private GameObject _debugger;

    private bool debugVisuals = false;
    private LineRenderer pathLine;

    [Header("AI attributes")]
    [SerializeField] private int ghostType = 0;
    [SerializeField] private float searchAreaSize = 5f;
    public int EnemyId;

    private float _oldSearchAreaSize;
    private float _oldMoveSpeed;


    [SerializeField] private float moveSpeed = 5f;
    private float huntTimer;
    private float huntTime = 3.0f;

    private bool huntingPlayer;

    private float _distanceToPlayer;

    private float _visibility;

    public float Tester;

    private float _searchPrecision = 0.5f;


    private bool _searchingNewDestination = false;

    // Start is called before the first frame update
    void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        pathLine = GetComponent<LineRenderer>();
        

        myMaterial = transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().GetComponent<Renderer>().material;
    }

    private void Start()
    {
       _debugger = Instantiate<GameObject>(_debugSphere);

        if (GameManager.Instance.debugMode)
        {
            debugVisuals = GameManager.Instance.showAIpath;
        }

        SetGhostType(ghostType);

        _oldMoveSpeed = moveSpeed;
        _oldSearchAreaSize = searchAreaSize;
    }

    // Update is called once per frame
    void Update()
    {
        DistanceToPlayer();

        if (_distanceToPlayer > 7.0f)
        {
            myMaterial.SetFloat("_Visibility", 0f);
        }
        else
        {
            myMaterial.SetFloat("_Visibility", 1f);
        }

        if (_searchingNewDestination)
        {
            Vector3 newDestination = GetNewDestination();
            navAgent.SetDestination(newDestination);

            if (debugVisuals)
            {
                _debugger.transform.position = newDestination;
            }

            _searchingNewDestination = false;
        }

        if (navAgent.remainingDistance <= 0.1f)
        {
            _searchingNewDestination = true;
        }

        if (debugVisuals)
        {
            
            ShowDebugPath();
        }
        else
        {
            pathLine.enabled = false;
        }

        // Raycast forward to look for the player
        if (huntingPlayer)
        {
            huntTimer += Time.deltaTime;
            //Debug.Log("Ghost (" + gameObject.name + ") is hunting you for " + huntTimer + " seconds");

            if (huntTimer > huntTime)
            {
                searchAreaSize = _oldSearchAreaSize;
                huntingPlayer = false;
            }
        }
        else
        {
            if (LookForPlayer())
            {
                huntingPlayer = true;
                huntTimer = 0;
                searchAreaSize = 2f;

                Vector3 newDestination = GetNewDestination();
                navAgent.SetDestination(newDestination);
            }
        }
    }

     private void ShowDebugPath()
    {
        if (navAgent.hasPath)
        {
            pathLine.positionCount = navAgent.path.corners.Length;
            pathLine.SetPositions(navAgent.path.corners);
            pathLine.enabled = true;
        }
    }

    private void SetGhostType(int ghostType)
    {
        switch (ghostType)
        {
            case 0:
                huntTime = 10f;
                moveSpeed = 3.5f;
                myMaterial.SetColor("_Color", GameManager.Instance.GhostType1);
                break;

            case 1:
                huntTime= 20f;
                moveSpeed = 4.0f;
                myMaterial.SetColor("_Color", GameManager.Instance.GhostType2);
                break;

            case 2:
                huntTime = 30f;
                moveSpeed = 4.5f;
                myMaterial.SetColor("_Color", GameManager.Instance.GhostType3);
                break;

            case 3:
                huntTime = 30f;
                moveSpeed = 5.0f;
                myMaterial.SetColor("_Color", GameManager.Instance.GhostType4);
                break;

            default:
                Debug.LogError("No valid Ghost Type");
                break;
        }
    }

    private void DistanceToPlayer()
    {
        Vector3 playerPos = PlayerController.Instance.transform.position;
        Vector3 myPos = transform.position;


        _distanceToPlayer = Vector3.Distance(myPos, playerPos);
    }


    //Gets a random position on the Nav Mesh in a random search area between enemy and player
    private Vector3 GetNewDestination()
    {
        Vector3 _originRadius = Vector3.Lerp(transform.position, player.transform.position, _searchPrecision);

        Vector3 _newPosition = new Vector3();
        Vector3 _randomRadius = Random.insideUnitSphere * searchAreaSize;
        _newPosition = _originRadius + _randomRadius;
        _newPosition.y = 0f;

        NavMesh.SamplePosition(_newPosition, out NavMeshHit hit, searchAreaSize, NavMesh.AllAreas);

        return hit.position;
    }

    private void OnEnable()
    {
        PlayerController.Instance.GettingCaught += PlayerIsCaught;
        PlayerController.Instance.Respawned += PlayerRespawned;
    }

    private void OnDisable()
    {
        PlayerController.Instance.GettingCaught -= PlayerIsCaught;
        PlayerController.Instance.Respawned -= PlayerRespawned;
    }

    private void PlayerIsCaught()
    {
        navAgent.speed = 0;
     //   transform.SetPositionAndRotation(GameManager.Instance.EnemySpawn.position, GameManager.Instance.EnemySpawn.rotation);
    }

    private void PlayerRespawned()
    {
        navAgent.speed = _oldMoveSpeed;
    }

    private bool LookForPlayer()
    {
        if (_distanceToPlayer > 15.0f)
        {
            return false;
        }

        bool seesPlayer = false;

        Vector3 drawFromPosition = new Vector3(0, 0.5f, 0);
        drawFromPosition += transform.position;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(drawFromPosition, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(drawFromPosition, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);

            if (hit.collider.tag == "Player")
            {
                seesPlayer = true;
            }
        }
        else
        {
            Debug.DrawRay(drawFromPosition, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
        }

        return seesPlayer;
    }




    private bool RunTimer(float Timer, float TimeEnds, float DeltaTime)
    {
        if (Timer >= TimeEnds)
        {
            return true;
        }

        Timer += DeltaTime;


        return false;

    }

}
