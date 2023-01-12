using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.Experimental.GlobalIllumination;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent navAgent;
    private GameObject player;

    private Material myMaterial;

    [SerializeField] private GameObject _debugSphere;
    private GameObject _debugger;

    private bool debugVisuals = false;
    private LineRenderer pathLine;
    public Light ghostLight; 
    private float ghostLightIntensity = 1.5f;

    [Header("AI attributes")]
    [SerializeField] public int ghostType = 0;
    [SerializeField] private float searchAreaSize = 5f;
    private int currentGhostType;
    public int EnemyId;

    public int score = 0;

    private float _oldSearchAreaSize;
    private float _oldMoveSpeed;

    [HideInInspector] public float ghostEvolveProgress;
    private float _ghostEvolveScore;

    [SerializeField] private float moveSpeed = 5f;
    private float huntTimer;
    private float huntTime = 3.0f;
    private bool huntingPlayer;

    private float _distanceToPlayer;

    private float _visibility = 1f;
    private bool _visible = true;

    private float _searchPrecision = 0.5f;

    private bool _searchingNewDestination = false;
    private bool _reachedDestination = false;

    private float _lookAroundTimer = 0f;
    private float _lookAroundTime = 0f;
    private bool _setLookAround = false;

    private float _lightTriggerTimer = 0f;
    private readonly float _lightTriggerTime = 2f;
    private bool _lightTriggered = false;

    
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
        GhostMovement();

        // Why was this needed?
        /*
        if (currentGhostType != ghostType)
        {
            SetGhostType(ghostType);
        } */
        
        myMaterial.SetFloat("_Visibility", _visibility);
        
        
        // ** LIGHTTRIGGERED ** 
        if (_lightTriggered)
        {
            _lightTriggerTimer += Time.deltaTime;
            if (_lightTriggerTimer > _lightTriggerTime)
            {
                _lightTriggered = false;
            }
        }
    }

    private void GhostMovement()
    {
        // When lookaround: rotate in a set speed
        // maybe not make the ghosts stop when player is too far away


        if (_searchingNewDestination)
        {
            _reachedDestination = false;
            _setLookAround = false;
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
            _reachedDestination = true;
        }

        if (_reachedDestination)
        {
            if (huntingPlayer || _distanceToPlayer > 20f)
            {
                _searchingNewDestination = true;
            }

            if (!_setLookAround)
            {
                _lookAroundTime = (Random.Range(1f, 6f) - ghostType);

                if (_lookAroundTime < 2f)
                {
                    _searchingNewDestination = true;
                }
                _setLookAround = true;
            }
            else
            {
                _lookAroundTimer += Time.deltaTime;

                // rotate lerpje?
                //transform.localRotation


                if (_lookAroundTimer > _lookAroundTime)
                {
                    _searchingNewDestination = true;
                    _lookAroundTimer = 0f;
                }
            }
        }

        // DEBUG VISUALS
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

    private void SetGhostType(int _ghostType)
    {
        ghostType = _ghostType;

        switch (_ghostType)
        {
            case 0:
                huntTime = 10f;
                moveSpeed = 3f;
                myMaterial.SetColor("_Color", EnemyManager.Instance.GhostType1);
                ghostLight.color = EnemyManager.Instance.GhostType1;
                break;

            case 1:
                huntTime= 20f;
                moveSpeed = 3.5f;
                myMaterial.SetColor("_Color", EnemyManager.Instance.GhostType2);
                ghostLight.color = EnemyManager.Instance.GhostType2;
                break;

            case 2:
                huntTime = 30f;
                moveSpeed = 4f;
                myMaterial.SetColor("_Color", EnemyManager.Instance.GhostType3);
                ghostLight.color = EnemyManager.Instance.GhostType3;
                break;

            case 3:
                huntTime = 30f;
                moveSpeed = 4.5f;
                myMaterial.SetColor("_Color", EnemyManager.Instance.GhostType4);
                ghostLight.color = EnemyManager.Instance.GhostType4;
                break;

            default:
                Debug.LogError("No valid Ghost Type");
                break;
        }

        EnemyManager.Instance.UpdateUI();
        navAgent.speed = moveSpeed;
    }
    
    private void DistanceToPlayer()
    {
        Vector3 playerPos = PlayerController.Instance.transform.position;
        Vector3 myPos = transform.position;
        
        _distanceToPlayer = Vector3.Distance(myPos, playerPos);

        if (_lightTriggered && _visible == false)
        {
            ShowEnemy(true);
        }
        else
        {
            if (_distanceToPlayer > 8f && _visible)
            {
                ShowEnemy(false);
            }
            else if (_distanceToPlayer < 6f && _visible == false)
            {
                ShowEnemy(true);
            }
        }
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

            if (hit.collider.CompareTag("Player"))   
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


    public void PickUpValuable(int _value)
    {
        score += _value;

        switch (ghostType)
        {
            case 0 :
                _ghostEvolveScore = 250f;
                break;
            case 1 :
                _ghostEvolveScore = 400f;
                break;
            case 2 :
                _ghostEvolveScore = 500f;
                break;
        }
        

        //Debug.Log(this.name + " - score: " + score);

        if (score >= 250 && ghostType == 0)
        {
            SetGhostType(1);
            score = 0;
            _ghostEvolveScore = 400f;
        }
        else if (score >= 400 && ghostType == 1)
        {
            SetGhostType(2);
            score = 0;
            _ghostEvolveScore = 500f;
        }
        else if (score >= 500 && ghostType == 2)
        {
            SetGhostType(3);
            score = 0;
            _ghostEvolveScore = 500f;
        }
        else if (score >= 500 && ghostType == 3)
        {
            SetGhostType(4);
            score = 0;
        }

        ghostEvolveProgress = score / _ghostEvolveScore;

        EnemyManager.Instance.UpdateUI();
    }

    private void ShowEnemy(bool show)
    {
        if (show)
        {
            DOTween.To(()=> _visibility, x=> _visibility = x, 1f, 0.7f);
            ghostLight.DOIntensity(ghostLightIntensity, 1f);
            _visible = true;
        }
        else
        {
            DOTween.To(()=> _visibility, x=> _visibility = x, 0f, 0.7f);
            ghostLight.DOIntensity(0f, 1f);
            _visible = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("LightCollider"))
        {
            _lightTriggered = true;
            _lightTriggerTimer = 0f;
        }
    }


    public void CaughtPlayer()
    {
        //rotates to player, begins sucking up animation (set Vector3 pos of player)
        
    }

    private IEnumerator SuckedUp()
    {
        // adds to the mask of being sucked up, on complete trigger Player ghost shader (or just time it in the Player script)
        
        yield break;
    }
    
}
