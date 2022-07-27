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
        graves = GameObject.FindGameObjectsWithTag("Grave");

        Debug.Log("GRAVES: " + graves.Length);

        foreach (GameObject grave in graves)
        {
            Grave _grave = grave.GetComponent<Grave>();
            
            int rnd = Random.Range(0, 3);
            _grave.graveType = rnd;

        }
    }
}
