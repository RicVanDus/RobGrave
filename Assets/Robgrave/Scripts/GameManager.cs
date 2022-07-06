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
    }

    // Update is called once per frame
    private void Update()
    {


    }

}
