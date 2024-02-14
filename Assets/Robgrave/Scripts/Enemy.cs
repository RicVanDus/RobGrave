using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;
using Vector4 = UnityEngine.Vector4;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent _navAgent;
    private GameObject _player;

    private Material _myMaterial;

    [SerializeField] private GameObject _debugSphere;
    private GameObject _debugger;

    private bool _debugVisuals = false;
    private LineRenderer _pathLine;
    public Light ghostLight; 
    private readonly float _ghostLightIntensity = 1.5f;

    [Header("AI attributes")]
    [SerializeField] public int ghostType = 0;
    [SerializeField] private float searchAreaSize = 5f;
    private int _currentGhostType;
    public int EnemyId;

    public int score = 0;

    private float _oldSearchAreaSize;
    private float _oldMoveSpeed;

    [HideInInspector] public float ghostEvolveProgress;
    private float _ghostEvolveScore;

    [SerializeField] private float moveSpeed = 5f;
    private float _huntTimer;
    private float _huntTime = 3.0f;
    private bool _huntingPlayer;

    private float _distanceToPlayer;

    private float _visibility = 1f;
    private bool _visible = true;

    private readonly float _searchPrecision = 0.5f;

    private bool _searchingNewDestination = false;
    private bool _reachedDestination = false;

    private float _lookAroundTimer = 0f;
    private float _lookAroundTime = 0f;
    private bool _setLookAround = false;

    private float _lightTriggerTimer = 0f;
    private readonly float _lightTriggerTime = 2f;
    private bool _lightTriggered = false;

    private float _greedyGhostTimer = 0f;
    private readonly float _greedyGhostTime = 60f;
    private bool _blinkingGhost = false;

    private WaitForSeconds _wait05 = new(0.5f);

    private int _suckUpProgressId;
    private int _suckUpPosId;
    
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

    private void Awake()
    {
        _suckUpPosId = Shader.PropertyToID("SuckingUpPosition");
        _suckUpProgressId = Shader.PropertyToID("SuckingUpProgress");
    }

    private void Start()
    {
       //_debugger = Instantiate<GameObject>(_debugSphere);
       _player = GameObject.FindGameObjectWithTag("Player");
       _navAgent = GetComponent<NavMeshAgent>();
        
       _pathLine = GetComponent<LineRenderer>();
       _myMaterial = transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().GetComponent<Renderer>().material;       

        if (GameManager.Instance.debugMode)
        {
            _debugVisuals = GameManager.Instance.showAIpath;
        }

        SetGhostType(ghostType);

        _oldMoveSpeed = moveSpeed;
        _oldSearchAreaSize = searchAreaSize;
    }

    void Update()
    {
        DistanceToPlayer();
        GhostMovement();

        _myMaterial.SetFloat("_Visibility", _visibility);

        if (_visibility > 0f)
        {
            transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
        else
        {
            transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
        
        // ** LIGHTTRIGGERED ** 
        if (_lightTriggered)
        {
            if (_lightTriggerTimer < 0.5f)
            {
                _myMaterial.SetFloat("_Base_Transparency", 0.7f);
            }
            else
            {
                _myMaterial.SetFloat("_Base_Transparency", 0.2f);
            }
            _lightTriggerTimer += Time.deltaTime;
            if (_lightTriggerTimer > _lightTriggerTime)
            {
                _lightTriggered = false;
            }
        }

        if (ghostType == 3)
        {
            _greedyGhostTimer += Time.deltaTime;

            if (_greedyGhostTimer > (_greedyGhostTime * 0.9) && !_blinkingGhost)
            {
                StartCoroutine(Blinking());
            }

            if (_greedyGhostTimer > _greedyGhostTime)
            {
                GreedyGhostOver();
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
            _navAgent.SetDestination(newDestination);

            if (_debugVisuals)
            {
                _debugger.transform.position = newDestination;
            }

            _searchingNewDestination = false;
        }

        if (_navAgent.remainingDistance <= 0.1f)
        {
            _reachedDestination = true;
        }

        if (_reachedDestination)
        {
            if (_huntingPlayer || _distanceToPlayer > 20f || ghostType == 3)
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

                if (_lookAroundTimer > _lookAroundTime)
                {
                    _searchingNewDestination = true;
                    _lookAroundTimer = 0f;
                }
            }
        }

        // DEBUG VISUALS
        if (_debugVisuals)
        {
            ShowDebugPath();
        }
        else
        {
            _pathLine.enabled = false;
        }

        // Raycast forward to look for the player
        if (_huntingPlayer)
        {
            _huntTimer += Time.deltaTime;
            //Debug.Log("Ghost (" + gameObject.name + ") is hunting you for " + huntTimer + " seconds");

            if (_huntTimer > _huntTime)
            {
                searchAreaSize = _oldSearchAreaSize;
                _huntingPlayer = false;
            }
        }
        else
        {
            if (LookForPlayer())
            {
                _huntingPlayer = true;                
                _huntTimer = 0;
                searchAreaSize = 2f;

                Vector3 newDestination = GetNewDestination();
                _navAgent.SetDestination(newDestination);
            }
        }
    }
    
     private void ShowDebugPath()
    {
        if (_navAgent.hasPath)
        {
            _pathLine.positionCount = _navAgent.path.corners.Length;
            _pathLine.SetPositions(_navAgent.path.corners);
            _pathLine.enabled = true;
        }
    }

    private void SetGhostType(int _ghostType)
    {
        ghostType = _ghostType;

        switch (_ghostType)
        {
            case 0:
                _huntTime = 10f;
                moveSpeed = 3f;
                _myMaterial.SetColor("_Color", EnemyManager.Instance.GhostType1);
                ghostLight.color = EnemyManager.Instance.GhostType1;
                break;

            case 1:
                _huntTime= 20f;
                moveSpeed = 3.5f;
                _myMaterial.SetColor("_Color", EnemyManager.Instance.GhostType2);
                ghostLight.color = EnemyManager.Instance.GhostType2;
                break;

            case 2:
                _huntTime = 30f;
                moveSpeed = 4f;
                _myMaterial.SetColor("_Color", EnemyManager.Instance.GhostType3);
                ghostLight.color = EnemyManager.Instance.GhostType3;
                break;

            case 3:
                _huntTime = 30f;
                moveSpeed = 4.5f;
                _myMaterial.SetColor("_Color", EnemyManager.Instance.GhostType4);
                ghostLight.color = EnemyManager.Instance.GhostType4;
                _greedyGhostTimer = 0f;
                break;

            default:
                Debug.LogError("No valid Ghost Type");
                break;
        }

        _navAgent.speed = moveSpeed;
    }
    
    private void DistanceToPlayer()
    {
        Vector3 playerPos = PlayerController.Instance.transform.position;
        Vector3 myPos = transform.position;
        
        _distanceToPlayer = Vector3.Distance(myPos, playerPos);
        
        
        //This is wrong.
        // it should check for lighttriggered
        
        if (_lightTriggered || ghostType == 3)
        {
            if (!_visible)
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
        Vector3 _originRadius = Vector3.Lerp(transform.position, _player.transform.position, _searchPrecision);

        Vector3 _newPosition = new Vector3();
        Vector3 _randomRadius = Random.insideUnitSphere * searchAreaSize;
        _newPosition = _originRadius + _randomRadius;
        _newPosition.y = 0f;

        float searchAreaSize2 = searchAreaSize;
        
        if (ghostType == 3)
        {
            _newPosition = Vector3.zero;
            searchAreaSize2 = 100f;
        }
        
        NavMesh.SamplePosition(_newPosition, out NavMeshHit hit, searchAreaSize, NavMesh.AllAreas);    
        
        return hit.position;
    }

    private void PlayerIsCaught()
    {
        _navAgent.speed = 0;
     //   transform.SetPositionAndRotation(GameManager.Instance.EnemySpawn.position, GameManager.Instance.EnemySpawn.rotation);
    }

    private void PlayerRespawned()
    {
        _navAgent.speed = _oldMoveSpeed;
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
        if (Physics.Raycast(drawFromPosition, transform.TransformDirection(Vector3.forward), out hit, 100f))
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

        ghostEvolveProgress = score / _ghostEvolveScore;

        //EnemyManager.Instance.UpdateUI();
    }

    private void ShowEnemy(bool show)
    {
        if (show)
        {
            DOTween.To(()=> _visibility, x=> _visibility = x, 1f, 0.2f);
            ghostLight.DOIntensity(_ghostLightIntensity, 0.5f);
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
            CaughtInLight();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (ghostType == 3)
            {
                GreedyGhostGotCaught();
            }
            else
            {
                PlayerController.Instance.EnemyHitsPlayer(ghostType);
            }
        }
    }

    public void CaughtInLight()
    {
        _lightTriggered = true;
        _lightTriggerTimer = 0f;
    }

    public void CaughtPlayer()
    {
        //rotates to player, begins sucking up animation (set Vector3 pos of player)
        Vector3 dir = transform.position - PlayerController.Instance.transform.position;
        Quaternion rot = Quaternion.LookRotation(dir);
        
        Vector3 newDir = rot.eulerAngles;
        newDir.y += 180f;

        transform.DORotate(newDir, 0.3f, RotateMode.Fast);
        StartCoroutine(SuckedUp(PlayerController.Instance.transform.position));
    }

    private IEnumerator SuckedUp(Vector3 pos)
    {
        // adds to the mask of being sucked up, on complete trigger Player ghost shader (or just time it in the Player script)
        bool suckDone = false;
        float speed = 10f;
        float suckUpProgress = 0f;
        Vector4 posv4 = new Vector4(pos.x, pos.y, pos.z, 0f);
        
        _myMaterial.SetVector(_suckUpPosId, posv4);

        do
        {
            suckUpProgress += Time.deltaTime * speed;

            _myMaterial.SetFloat(_suckUpProgressId, suckUpProgress);
            yield return null;

            if (suckUpProgress > 1f)
            {
                suckDone = true;
                Respawn();
            }
        } while (!suckDone);
    }

    private void Respawn()
    {
        // respawns Enemy at one of the respawn points
        // Maybe add some effect? -poof

        var spawnPoints = EnemyManager.Instance.enemySpawnPoints;
        int rndIndex = Random.Range(0, spawnPoints.Count);

        transform.position = spawnPoints[rndIndex].transform.position;
    }

    public void GreedyGhostGotCaught()
    {
        ResetGhost();
        LootManager.Instance.SpawnLoot(Random.Range(1500, 2000), Random.Range(10,20), 0);
    }

    private void ResetGhost()
    {
        Respawn();
        SetGhostType(0);
    }

    private void GreedyGhostOver()
    {
        _blinkingGhost = false;
        StopCoroutine(Blinking());
        SetGhostType(2);
    }

    private IEnumerator Blinking()
    {
        while (ghostType == 3)
        {
            if (_myMaterial.GetColor("_Color") == EnemyManager.Instance.GhostType4)
            {
                _myMaterial.SetColor("_Color", EnemyManager.Instance.GhostType3);
            }
            else
            {
                _myMaterial.SetColor("_Color", EnemyManager.Instance.GhostType4);
            }

            yield return _wait05;
        }
    }
}
