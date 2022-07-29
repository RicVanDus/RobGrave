using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

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

    [Header("Player attributes")]
    public Transform PlayerSpawn;
    public GameObject[] graves;

    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        thisLevel = levels[currentLevel - 1];
    }

    private void Start()
    {
        // call function to assign graves
        AssignGraves();

        // spawn all enemies
        EnemyManager.Instance.CreateEnemyList();
        EnemyManager.Instance.SpawnAllEnemies();
    }

    // Update is called once per frame
    private void Update()
    {


    }


    // Assign more purple and blue graves if the score threshold calls for it (total nr of graves / threshold value)
    // Number of purple and blue graves that gets assigned to random graves. However: the graves closer to the north wall have a higher chance of becoming a blue/purple grave!
    // For instance: graves ID1 & ID2 have a 10% chance of becoming a purple grave
    // When purple is assigned, you assign blue, rest is filled with green


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
                        _gotcha = true;

                    }

                    indexDone[i] = _rnd;
                }

                

            } while (!_gotcha);
        }


    }
}
