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
    
    [Header("Ghost attributes")]
    public Transform EnemySpawn;
    public Color GhostType1;
    public Color GhostType2;
    public Color GhostType3;
    public Color GhostType4;

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

    }

    // Update is called once per frame
    private void Update()
    {


    }

    private void AssignGraves()
    {
        graves = GameObject.FindGameObjectsWithTag("Grave");

        Debug.Log("GRAVES: " + graves.Length);

        foreach (GameObject grave in graves)
        {
            Grave _grave = grave.GetComponent<Grave>();

            int rnd = Random.Range(0, 3);
            _grave.graveType = rnd;

            //Debug.Log(_grave.valuables);
        }
    }



}
