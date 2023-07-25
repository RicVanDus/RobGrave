using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.InputSystem.Users;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    [Header("Attributes")]
    public float moveSpeed = 10.0f;
    public float grip = 10.0f;
    public float rotationSpeed = 3.0f;
    public float digSpeedMultiplier = 3.0f;
    public float flashLightReach = 10f;
    public int currentLives = 3;
    public int maxLives = 3;
    public int score = 0;
    public float invulnerableTime = 3f;
    [NonSerialized] public int preScore;
    [SerializeField] private LayerMask _flashlightHits;

    [Header("Camera")]
    public Camera cam;

    private Vector2 moveDirection;
    private float hMovement;
    private float vMovement;
    [NonSerialized] public bool _interacting = false;
    [NonSerialized] public bool _using = false;
    
    private float scoreAddingTimer = 0f;
    private float scoreAddingTime = 3f;
    [HideInInspector] public bool goalAchieved = false;

    [Header("Meshes")]
    [SerializeField] private SkinnedMeshRenderer _playerMesh;
    [SerializeField] private MeshRenderer _playerCapMesh;
    private Material[] _playerMeshMaterials;

    private Material playerMeshMaterial;
    private Rigidbody rigidB;
    private Animator RGAnimator;

    [SerializeField] private Shader _ghostShader;
    private Shader _defaultShader;

    [SerializeField] private MeshRenderer _torchHand;
    [SerializeField] private MeshRenderer _torchHip;
    [SerializeField] private MeshRenderer _shovelHand;
    [SerializeField] private MeshRenderer _shovelBack;
    [SerializeField] private GameObject _flashLightCone;
    private Material _flashlightConeMat;
    private Color _defaultFlashlightColor;
    private Color _enemyFlashlightColor;
    private int _colorId;

    [NonSerialized] public bool movementDisabled = false;
    [NonSerialized] public bool playerInteracting = false;
    private bool isInvulnerable = false;
    private bool _canInteract;
    private bool _isCaught = false;
    private bool _isTeleporting = false;

    private float blinkingTimer;

    private float idleTimer;

    private float _flashlightHitDistance = 0f;

    private Color[] _ghostColors;

    public delegate void OnGettingCaught();
    public event OnGettingCaught GettingCaught;

    public delegate void OnRespawn();
    public event OnRespawn Respawned;
    
    public GameObject valuePopup;
    public delegate void OnChangingScore();
    public event OnChangingScore ChangingScore;

    public delegate void Interact();

    public RGInputs playerInputs;

    private InputAction move;
    private InputAction interact;
    private InputAction attack;
    private InputAction pause;

    private WaitForSeconds _wait01 = new(0.1f);
    private WaitForSeconds _wait02 = new(0.2f);
    

    public static PlayerController Instance { get; private set; }

    private void OnEnable()
    {
        move = playerInputs.Player.Move;
        move.Enable();

        attack = playerInputs.Player.Attack;
        attack.Enable();
        attack.performed += OnAttack;
        attack.canceled += OnAttack;

        interact = playerInputs.Player.Interact;
        interact.Enable();
        interact.performed += OnInteract;
        interact.canceled += OnInteract;

        pause = playerInputs.Player.Pause;
        pause.Enable();
        pause.performed += OnPause;

        Respawned += Respawn;   
        
        GameOverseer.Instance.Pause += WhenPaused;
        GameOverseer.Instance.Playing += WhenPlaying;
        GameOverseer.Instance.StartGame += DataFromGO;
        GameOverseer.Instance.Extract += DataToGO;

        // changing input device, make a state that can be checked for UI changes
        InputUser.onChange += (user, change, device) =>
        {
            if (change == InputUserChange.ControlSchemeChanged)
            {
                Debug.Log($"User {user} switched control scheme to {device}");
            }
        };
    }

    private void OnDisable()
    {
        move.Disable();
        attack.Disable();
        interact.Disable();
        pause.Disable();
        attack.performed -= OnAttack;
        attack.canceled -= OnAttack;
        interact.performed -= OnInteract;
        interact.canceled -= OnInteract;
        pause.performed -= OnPause;
        Respawned -= Respawn;        
        GameOverseer.Instance.Pause -= WhenPaused;
        GameOverseer.Instance.Playing -= WhenPlaying;
        GameOverseer.Instance.StartGame -= DataFromGO;
        GameOverseer.Instance.Extract -= DataToGO;
    }

    // Start is called before the first frame update
    void Awake()
    {
        _defaultShader = _playerCapMesh.material.shader;
         _playerMeshMaterials = _playerMesh.materials;
         
         _flashlightConeMat = _flashLightCone.GetComponent<Renderer>().material;
         _colorId = Shader.PropertyToID("_Color");
         _defaultFlashlightColor = _flashlightConeMat.GetColor(_colorId);
        
        if (Instance == null)
        {
            Instance = this;
        }

        playerInputs = new RGInputs();
        
        rigidB = GetComponent<Rigidbody>();

        RGAnimator = GetComponentInChildren<Animator>();

        // This needs to be fixed
        //playerMesh = transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();
        //playerMeshMaterial = playerMesh.GetComponent<Renderer>().material;

        gameObject.tag = "Player";
    }

    private void Start()
    {
        // starter values
        _torchHand.gameObject.SetActive(true);
        _torchHip.gameObject.SetActive(false);
        _shovelBack.gameObject.SetActive(true);
        _shovelHand.gameObject.SetActive(false);

        if (GameOverseer.Instance.currentLevel > 0) DataFromGO();

        _ghostColors[0] = EnemyManager.Instance.GhostType1;
        _ghostColors[1] = EnemyManager.Instance.GhostType2;
        _ghostColors[2] = EnemyManager.Instance.GhostType3;
    }

    private void Update()
    {
        moveDirection = move.ReadValue<Vector2>();

        hMovement = moveDirection.x;
        vMovement = moveDirection.y;

        CheckIdleTime(Time.deltaTime);

        blinkingTimer += Time.deltaTime;

        CheckAddedScore();

        CheckFlashlightHit();
        
        FlashlightConeVisual();
    }

    private void FixedUpdate()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        if (movementDisabled)
        {
            rigidB.velocity = new Vector3(0, 0, 0);
        }
        else
        {
            if (!(hMovement == 0 && vMovement == 0))
            {
                Vector3 newPosition = new Vector3(hMovement, 0f, vMovement);
                newPosition.Normalize();

                rigidB.velocity = (newPosition * moveSpeed);
            }
            else
            {
                rigidB.velocity = new Vector3(0, 0, 0);
            }

            PlayerRotation(hMovement, vMovement);
        }

        if (!_isTeleporting) RGAnimator.SetFloat("Speed", rigidB.velocity.magnitude);
    }

    private void PlayerRotation(float h, float v)
    {
        Vector3 direction = new Vector3(h * 10, 0f, v * 10);

        if (direction.magnitude == 0) { return; }
        direction = Quaternion.Euler(0, 0, 0) * direction;

        var rotation = Quaternion.LookRotation(direction);

        if (rigidB.velocity.magnitude > 0)
        {
            rigidB.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed));
        }
        else
        {
            rigidB.MoveRotation(Quaternion.RotateTowards(transform.rotation, transform.rotation, rotationSpeed));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        /*
        if (other.CompareTag("Enemy") && isInvulnerable == false)
        {
            Enemy nme = other.GetComponent<Enemy>();

            if (nme.ghostType == 3)
            {
                nme.GreedyGhostGotCaught();
            }
            else
            {
                // Move all this to a public function
                Ghosted(nme.ghostType);
                isInvulnerable = true;
                movementDisabled = true;
                _isCaught = true;
                currentLives -= 1;
                GettingCaught?.Invoke();

                if (currentLives <= 0)
                {
                    GameOverseer.Instance.SetGameState(GameState.GameOver);
                }
                else
                {
                    StartCoroutine(LosingLoot(nme.ghostType));
                }
            
                RGAnimator.SetBool("Caught", true);
            }
        } */

        if (other.CompareTag("Grave"))
        {
            _canInteract = true;
        }

        if (other.CompareTag("Exit"))
        {
            _canInteract = true;
        }
        
        if (other.CompareTag("Lamppost"))
        {
            _canInteract = true;
        }        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Grave"))
        {
            _canInteract = false;
        }

        if (other.CompareTag("Exit"))
        {
            _canInteract = false;
        }

        if (other.CompareTag("Lamppost"))
        {
            _canInteract = false;
        }
    }

    private void Respawning()
    {
        Respawned?.Invoke();
    }

    private void Respawn()
    {
        transform.SetPositionAndRotation(GameManager.Instance.PlayerSpawn.position, GameManager.Instance.PlayerSpawn.rotation);
        RGAnimator.SetBool("Floating", false);
        RGAnimator.SetBool("Caught", false);
        _isCaught = false;
        
        _playerCapMesh.material.shader = _defaultShader;

        if (_playerMeshMaterials.Length > 0)
        {
            for (int i=0; i < _playerMeshMaterials.Length; i++)
            {
                _playerMeshMaterials[i].shader = _defaultShader;
            }
        }
        StartCoroutine(InvulnerableTime());
    }

    private IEnumerator InvulnerableTime()
    {
        blinkingTimer = 0;
        movementDisabled = false;

        //Blinking player mesh while invulnerable
        while (invulnerableTime > blinkingTimer)
        {
            
            if (_playerMesh.enabled)
            {
                _playerMesh.enabled = false;
                _playerCapMesh.enabled = false;
            }
            else
            {
                _playerMesh.enabled = true;
                _playerCapMesh.enabled = true;
            }
            

            yield return _wait02;
        }

        _playerMesh.enabled = true;
        _playerCapMesh.enabled = true;
        _shovelBack.enabled = true;
        _shovelHand.enabled = true;
        _torchHand.enabled = true;
        _torchHip.enabled = true;
        isInvulnerable = false;
    }

    public void AddScore(int value)
    {
        preScore += value;
        scoreAddingTimer = 0f;
        Vector3 _spawnPos = new Vector3(transform.position.x, transform.position.y + 3f, transform.position.z);

        float _newScale = 0f;

        if (value < 0)
        {
            _newScale = 0.02f;
        }
        else if (value == 5)
        {
            _newScale = 0.01f;
        }
        else if (value == 10)
        {
            _newScale = 0.011f;
        }
        else if (value == 20)
        {
            _newScale = 0.012f;
        }
        else if (value == 40)
        {
            _newScale = 0.0135f;
        }
        else if (value == 100)
        {
            _newScale = 0.015f;
        }
        else if (value == 200)
        {
            _newScale = 0.018f;
        }

            GameObject _popupText = Instantiate(valuePopup, _spawnPos, Quaternion.identity);
        _popupText.transform.localScale = _newScale * Vector3.one;
        ValuePopup _thisPopup = _popupText.GetComponent<ValuePopup>();
        _thisPopup.PopUpScore(value, value>0);
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed && _canInteract)
        {
            _interacting = true;
        }
        else if (context.canceled)
        {
            _interacting = false;
            movementDisabled = false;
            IsDigging(false);
        }
    }


    private void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _using = true;
        }
        else if (context.canceled)
        {
            _using = false;
        }
    }


    private void CheckIdleTime(float dT)
    {
        if (moveDirection.magnitude == 0)
        {
            idleTimer += dT;
        }
        else
        {
            idleTimer = 0f;
        }

        RGAnimator.SetFloat("IdleTime", idleTimer);
    }

    private void CheckAddedScore()
    {
        if (preScore > 0)
        {
            scoreAddingTimer += Time.deltaTime;

            if (scoreAddingTimer >= scoreAddingTime)
            {
                StartCoroutine(AddingPreScoreToScore(preScore));
                preScore = 0;
            }
        }
        else if (preScore < 0)
        {
            StartCoroutine(AddingPreScoreToScore(preScore));
            preScore = 0;
        }
    }

    private IEnumerator AddingPreScoreToScore(int preScore)
    {
        float _amountToScore = preScore;
        _amountToScore = _amountToScore / 10;
        float _modAmount = _amountToScore % (preScore / 10);
        _amountToScore -= _modAmount;       

        int _counter = 0;

        if (preScore > 20)
        {
            if (_modAmount > 0.0f)
            {
                score += (int)(_modAmount * 10);
                _counter += (int)(_modAmount * 10);
                CheckIfGoalAchieved();
            }
            while (_counter < preScore)
            {                
                score += (int)_amountToScore;
                _counter += (int)_amountToScore;
                CheckIfGoalAchieved();

                yield return _wait01;
            }
        }
        else
        {
            score += preScore;
            CheckIfGoalAchieved();
        }
    }

    public void RotateToObject(GameObject obj)
    {
        rigidB.DORotate(obj.transform.rotation.eulerAngles, 0.3f, RotateMode.Fast);
    }

    public void IsDigging(bool digging)
    {
        bool _currentDig = RGAnimator.GetBool("Digging");
        
        if (digging && !_currentDig)
        {
            RGAnimator.SetBool("Digging", true);
            _torchHand.gameObject.SetActive(false);
            _torchHip.gameObject.SetActive(true);
            _shovelBack.gameObject.SetActive(false);
            _shovelHand.gameObject.SetActive(true);
            
        }
        else if (!digging && _currentDig)
        {
            RGAnimator.SetBool("Digging", false);
            _torchHand.gameObject.SetActive(true);
            _torchHip.gameObject.SetActive(false);
            _shovelBack.gameObject.SetActive(true);
            _shovelHand.gameObject.SetActive(false);
        }
    }

    public void IsHitting(bool hitting)
    {
        // SET STUFF FOR ANIMATION AND VISUALS
    }

    private void Ghosted(int GhostType)
    {
        Color _clr = Color.black;
        
        switch (GhostType)
        {
            case 0 :
                _clr = EnemyManager.Instance.GhostType1;    
                break;
            case 1 :
                _clr = EnemyManager.Instance.GhostType2;
                break;
            case 2 :
                _clr = EnemyManager.Instance.GhostType3;
                break;
        }
        
        _shovelBack.enabled = false;
        _torchHip.enabled = false;
        _shovelHand.enabled = false;
        _torchHand.enabled = false;
        
        // change shaders
        _playerCapMesh.material.shader = _ghostShader;
        _playerCapMesh.material.SetColor("_Color", _clr);
        
        if (_playerMeshMaterials.Length > 0)
        {
            for (int i=0; i < _playerMeshMaterials.Length; i++)
            {
                _playerMeshMaterials[i].shader = _ghostShader;
                _playerMeshMaterials[i].SetColor("_Color", _clr);
            }
        }
    }

    private IEnumerator LosingLoot(int GhostType)
    {
        int _times = GhostType + 1;
        float _radius = 40f;
        float _innerRadius = 20f;
        bool _approvedSpot = false;
        int _scoreToDrop = 0;
        int _amountOfDrops = 0;

        // recalculate _times & loot spawn amounts

        if (score / _times > 100f)
        {
            _scoreToDrop = Mathf.FloorToInt(score * 0.2f);
            _amountOfDrops = Random.Range(15, 25);
        }
        else if (score / _times == 0)
        {
            _times = 1;
        }
        else
        {
            _times = 1;
            _scoreToDrop = score;
            _amountOfDrops = Random.Range(15, 25);
        }
        
        // Doing it
        for (int i = 0; i < _times; i++)
        {
            Vector3 _newPosition = new Vector3();
            Vector3 _randomRadius = new Vector3();
            
            do
            {
                _randomRadius = Random.insideUnitSphere * _radius + (Vector3.one * _innerRadius);
                if (Vector3.Distance(transform.position, _randomRadius) > _innerRadius && Vector3.Distance(_randomRadius, GameManager.Instance.PlayerSpawn.position) > _innerRadius) 
                {
                    Debug.Log("Distance from player pos: "+ Vector3.Distance(transform.position, _randomRadius)+ "-- Spawn point ");
                    _approvedSpot = true;    
                }
            } while (_approvedSpot == false);

            _newPosition = transform.position + _randomRadius;
            
            _newPosition.y = 0f;

            NavMesh.SamplePosition(_newPosition, out NavMeshHit hit, _radius, NavMesh.AllAreas);

            yield return new WaitForSeconds(1f);
            RGAnimator.SetBool("Floating", true);
            

            Vector3 _newPos = new Vector3(hit.position.x, 5f, hit.position.z);

            transform.DORotate(new Vector3(0f, Random.Range(0f, 720f), 0f), 3f, RotateMode.FastBeyond360).SetEase(Ease.OutBounce);
            transform.DOMove(_newPos, 2f, false).SetEase(Ease.OutSine).OnComplete(() =>
            {
                LootManager.Instance.SpawnLoot(_scoreToDrop, _amountOfDrops, 0);
                AddScore(-_scoreToDrop);
                transform.DOMoveY(6.5f, 1f, false).SetEase(Ease.OutElastic);

            }).SetEase((Ease.Linear));
            
            _approvedSpot = false;
            
            yield return new WaitForSeconds(4f);
        }
        Invoke("Respawning", 1f);
    }

    
    // functions subscribed to gamestate
    private void OnPause(InputAction.CallbackContext context)
    {
        GameState _gameState = GameOverseer.Instance.gameState;
        
        // Not being able to pause when you are dead or caught
        if (context.performed && _gameState != GameState.Pause && currentLives > 0 && !_isCaught)
        {
            GameOverseer.Instance.SetGameState(GameState.Pause);
        }
        else if (context.performed && _gameState == GameState.Pause)
        {
            GameOverseer.Instance.SetGameState(GameState.Playing);
        }
    }

    private void CheckFlashlightHit()
    {
        bool seesEnemy = false;

        Vector3 drawFromPosition = new Vector3(0, 0.5f, 0);
        drawFromPosition += transform.position;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(drawFromPosition, transform.TransformDirection(Vector3.forward) * 100f, out hit, 100f, _flashlightHits))
        {
            Debug.DrawRay(drawFromPosition, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            
            //Debug.Log("Hitting: " + hit.collider);

            _flashlightHitDistance = hit.distance;
        }
        else
        {
            Debug.DrawRay(drawFromPosition, transform.TransformDirection(Vector3.forward) * 100f, Color.white);
            _flashlightConeMat.SetColor(_colorId, _defaultFlashlightColor);
        }
    }

    private void FlashlightConeVisual()
    {
        Vector3 oldscale = _flashLightCone.transform.localScale;
        Vector3 newscale = oldscale;
        newscale.z = Math.Clamp(_flashlightHitDistance * 1.3f, 0f, flashLightReach);
        _flashLightCone.transform.localScale = newscale;
    }

    public void FlashlightColor(int ghostType)
    {
        if (ghostType == 0)
        {
            _enemyFlashlightColor = _ghostColors[0];    
        }
        else if (ghostType == 1)
        {
            _enemyFlashlightColor = _ghostColors[1];
        }
        else if (ghostType == 2)
        {
            _enemyFlashlightColor = _ghostColors[2];
        }
        else
        {
            _enemyFlashlightColor = _defaultFlashlightColor;
        }
                
        _flashlightConeMat.SetColor(_colorId, _enemyFlashlightColor);
    }

    private void WhenPaused()
    {
        movementDisabled = true;
        RGAnimator.StopPlayback();
        move.Disable();
        interact.Disable();
        attack.Disable();
    }
    
    private void WhenPlaying()
    {
        movementDisabled = false;
        move.Enable();
        interact.Enable();
        attack.Enable();
    }
    
    private void CheckIfGoalAchieved()
    {
        if (score >= GameManager.Instance.thisLevel.valuablesRequired)
        {
            goalAchieved = true;
        }
        else
        {
            goalAchieved = false;
        }
        
        ChangingScore?.Invoke();
    }

    private void DataToGO()
    {
        GameOverseer.Instance.score = score;
        GameOverseer.Instance.maxLives = maxLives;
        GameOverseer.Instance.currentLives = currentLives;
    }
    
    private void DataFromGO()
    {
        score = GameOverseer.Instance.score;
        maxLives = GameOverseer.Instance.maxLives;
        currentLives = GameOverseer.Instance.currentLives;
    }

    public void TeleportTo(Transform newPos)
    {
        transform.position = newPos.position;
        transform.localRotation = newPos.rotation;
    }

    public void CryptTeleport(Crypt toCrypt)
    {
        movementDisabled = true;
        _isTeleporting = true;
        transform.position = toCrypt._moveTo.position;
        transform.rotation = toCrypt._moveTo.rotation;
        RGAnimator.SetFloat("Speed", 3.5f);

        transform.DOMove(toCrypt._spawnPoint.position, 1.3f).OnComplete(() =>
        {
            movementDisabled = false;
            _isTeleporting = false;
        });
    }
}