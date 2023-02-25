using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Debug")]
    [FormerlySerializedAs("Debug mode")] [SerializeField] public bool debugMode;
    [SerializeField] public bool showAIpath;

    [Header("Level attributes")]
    public int currentLevel;
    public LevelProperties[] levels;
    public LevelProperties thisLevel;
    
    [Header("Gravestones")]
    public Color graveType0Color;
    public Color graveType1Color;
    public Color graveType2Color;
    public Color graveDefaultColor;
    public Color graveWarningColor;
    public GameObject[] graveStones;
    
    [Header("UI")]
    public GameObject GraveUI;
    public GameObject GraveUI2;
    public GameObject GraveUI3;
    public GameObject ButtonPrompt;

    [Header("Misc objects")] 
    public GameObject steppingStone;
    public Transform envStones;

    [Header("Player attributes")]
    public Transform PlayerSpawn;
    public GameObject[] graves;
    public GameObject exitTrigger;
    
    private float _gameTimeSeconds;
    private int _gameTimeMinutes;
    private int _gameTimeHours;
    private bool _gameStarted;
    public string gameTime;
    private bool _witchingHour = false;
    
    private void OnEnable()
    {
        GameOverseer.Instance.StartGame += StartingGame;
    }

    private void OnDisable()
    {
        GameOverseer.Instance.StartGame -= StartingGame;
    }

    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        thisLevel = levels[GameOverseer.Instance.currentLevel];

        _gameTimeHours = thisLevel.startHours;
        _gameTimeMinutes = thisLevel.startMinutes;
        _gameTimeSeconds = thisLevel.startSeconds;
    }

    private void Update()
    {
        if (_gameStarted)
        {
            GameTime();
        }
    }


    // Assign more purple and blue graves if the score threshold calls for it (total nr of graves / threshold value)
    // Number of purple and blue graves that gets assigned to random graves. However: the graves closer to the north wall have a higher chance of becoming a blue/purple grave!
    // For instance: graves ID1 & ID2 have a 10% chance of becoming a purple grave
    // When purple is assigned, you assign blue, rest is filled with green
    // -- assign random grave-mesh, with random rotation on Y (0 or 180) and slight random rotation on Z
    
    private void AssignGraves()
    {
        int _scoreReq = thisLevel.valuablesRequired;

        graves = GameObject.FindGameObjectsWithTag("Grave");

        int[] indexDone = new int[graves.Length];
        int[] gravesType1 = new int[thisLevel.graveType1];
        int[] gravesType2 = new int[thisLevel.graveType2];

        for (int i = 0; i < gravesType2.Length; i++)
        {
            bool _gotcha = false;

            do
            {
                int _rnd = Random.Range(0, graves.Length);
                bool _check = true;

                for (int c = 0; c < indexDone.Length; c++)
                {
                    if (indexDone[c] == _rnd)
                    {
                        _check = false;
                    }
                }

                if (_check)
                {
                    Grave _grave = graves[_rnd].GetComponent<Grave>();

                    float _disToSpawn = Vector3.Distance(_grave.transform.position, PlayerSpawn.transform.position);
                    int _chance = (int)_disToSpawn;
                    int _random = Random.Range(1, 100);

                    if (_chance > _random)
                    {
                        _grave.graveType = 2;
                        _grave.SetGraveType(2);
                        _gotcha = true;
                    }

                    indexDone[i] = _rnd;
                }
            } while (!_gotcha);
        }

        for (int i = 0; i < gravesType1.Length; i++)
        {
            bool _gotcha = false;

            do
            {
                int _rnd = Random.Range(0, graves.Length);
                bool _check = true;

                for (int c = 0; c < indexDone.Length; c++)
                {
                    if (indexDone[c] == _rnd)
                    {
                        _check = false;
                    }
                }

                if (_check)
                {
                    Grave _grave = graves[_rnd].GetComponent<Grave>();

                    float _disToSpawn = Vector3.Distance(_grave.transform.position, PlayerSpawn.transform.position);
                    int _chance = (int)_disToSpawn;
                    int _random = Random.Range(1, 100);

                    if (_chance > _random)
                    {
                        _grave.graveType = 1;
                        _grave.SetGraveType(1);
                        
                        _gotcha = true;
                    }

                    indexDone[i] = _rnd;
                }                
            } while (!_gotcha);
        }

        // assign meshes to all graves, with the right color.

        for (int i = 0; i < graves.Length; i++)
        {
            Grave _grave = graves[i].GetComponent<Grave>();
            GameObject _graveStone = graves[i].transform.Find("gravestone").gameObject;            


            //Pick a random mesh
            GameObject _randomGraveStone = graveStones[Random.Range(0, graveStones.Length)];
            Mesh _randomMesh = _randomGraveStone.GetComponent<MeshFilter>().sharedMesh;
            Material _randomMeshMaterial = _randomGraveStone.GetComponent<MeshRenderer>().sharedMaterial;
            Color _graveColor = graveType0Color;

            Vector3 _gravePos = new Vector3(_graveStone.transform.position.x, -0.5f, _graveStone.transform.position.z);
            Vector3 _graveSize = new Vector3(1.3f, 1.3f, 1.3f);
            Vector3 _graveRotation = new Vector3(Random.Range(-3.0f, 3.0f), 0f, Random.Range(-6.0f, 6.0f));

            _graveStone.GetComponent<MeshFilter>().mesh = _randomMesh;
            _graveStone.GetComponent<MeshRenderer>().material = _randomMeshMaterial;
            _graveStone.transform.position = _gravePos;
            _graveStone.transform.localScale = _graveSize;
            _graveStone.transform.Rotate(_graveRotation);
            
            switch (_grave.graveType)
            {
                case 0:
                    _graveColor = graveType0Color;
                    break;
                case 1:
                    _graveColor = graveType1Color;
                    break;
                case 2:
                    _graveColor = graveType2Color;
                    break;
            }

            _graveStone.GetComponent<MeshRenderer>().material.SetColor("_Color", _graveColor);


            // Add the GraveUI & buttonPrompt

            float _graveUIScale = 0.6f;
            
            if (_grave.graveType == 0)
            {
                GameObject _graveUI = Instantiate(GraveUI, _grave.transform);
                _graveUI.transform.localScale *= _graveUIScale;

                GUI_grave_01 _UI = _graveUI.GetComponent<GUI_grave_01>();

                _UI._grave = _grave;
                _UI.SetUIPosition();
             
            }
            else if (_grave.graveType == 1)
            {
                GameObject _graveUI = Instantiate(GraveUI2, _grave.transform);
                _graveUI.transform.localScale *= _graveUIScale;

                GUI_grave_02 _UI = _graveUI.GetComponent<GUI_grave_02>();

                _UI._grave = _grave;
                _UI.SetUIPosition();
            }
            else if (_grave.graveType == 2)
            {
                GameObject _graveUI = Instantiate(GraveUI3, _grave.transform);
                _graveUI.transform.localScale *= _graveUIScale;

                GUI_grave_03 _UI = _graveUI.GetComponent<GUI_grave_03>();

                _UI._grave = _grave;
                _UI.SetUIPosition();
            }
            
        }
    }

    private void SpawnSteppingStones()
    {
        var lootGrid = LootManager.Instance.lootPositions;

        float _y = -0.72f;
        

        if (lootGrid.Count > 0)
        {
            for (int i = 0; i < lootGrid.Count; i++)
            {
                Vector3 _stonePos = new Vector3(lootGrid[i].GridPosition.x, _y, lootGrid[i].GridPosition.z);
                
                //random rotations
                int _rnd = Random.Range(0, 3);
                float _xRot = 90 * _rnd;
                _rnd = Random.Range(0, 3);
                float _yRot = 90 * _rnd;
                _rnd = Random.Range(0, 3);
                float _zRot = 90 * _rnd;

                Quaternion _stoneRot = Quaternion.Euler(_xRot, _yRot, _zRot);

                Instantiate(steppingStone, _stonePos, _stoneRot, envStones);
            }
        }
    }
    
    public void StartingGame()
    {
        AssignGraves();
        SpawnSteppingStones();
        EnemyManager.Instance.SpawnAllEnemies();
        
        GameOverseer.Instance.SetGameState(GameState.Playing);
        _gameStarted = true;
    }

    private void GameTime()
    {
        _gameTimeSeconds += Time.deltaTime;

        if (_gameTimeSeconds >= 60f)
        {
            _gameTimeMinutes++;
            _gameTimeSeconds = 0f;
        }

        if (_gameTimeMinutes >= 60)
        {
            _gameTimeHours++;
            _gameTimeMinutes = 0;
        }

        if (_gameTimeHours == 0 && _witchingHour == false) _witchingHour = true;

        gameTime = _gameTimeHours + ":" + _gameTimeMinutes + "'" + _gameTimeSeconds;
    }

    public void PlayerExtract()
    {
        EnemyManager.Instance.DespawnAllEnemies();
        GameOverseer.Instance.currentLevel++;
        PlayerController.Instance.score -= thisLevel.valuablesRequired;
        GameOverseer.Instance.SetGameState(GameState.Extract);
    }
}
