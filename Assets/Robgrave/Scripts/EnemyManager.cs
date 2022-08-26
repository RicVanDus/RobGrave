using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { private set; get; }

    public List<GameObject> enemySpawnPoints;
    public List<Enemy> enemies;

    public Enemy enemy;

    [Header("Ghost attributes")]
    public Color GhostType1;
    public Color GhostType2;
    public Color GhostType3;
    public Color GhostType4;

    private void Awake()
    {
        if (Instance == null )
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateEnemyList()
    {
        LevelProperties _levelProps = GameManager.Instance.thisLevel;

        int _ghostTypes0 = _levelProps.ghostSpawnType0;
        int _ghostTypes1 = _levelProps.ghostSpawnType1;
        int _ghostTypes2 = _levelProps.ghostSpawnType2;
        int _enemyId = 0;

        for (int i = 0; i < _ghostTypes0; i++)
        {
            Enemy _newEnemy = new Enemy();
            _newEnemy.ghostType = 0;
            _newEnemy.EnemyId = _enemyId;

            enemies.Add(_newEnemy);
            _enemyId++;
        }

        for (int i = 0; i < _ghostTypes1; i++)
        {
            Enemy _newEnemy = new Enemy();
            _newEnemy.ghostType = 1;
            _newEnemy.EnemyId = _enemyId;

            enemies.Add(_newEnemy);
            _enemyId++;
        }


        for (int i = 0; i < _ghostTypes2; i++)
        {
            Enemy _newEnemy = new Enemy();
            _newEnemy.ghostType = 2;
            _newEnemy.EnemyId = _enemyId;

            enemies.Add(_newEnemy);
            _enemyId++;
        }
    }

    public void SpawnAllEnemies()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            int _enemyGhostType = enemies[i].ghostType;
            int _id = enemies[i].EnemyId;

            SpawnEnemy(_enemyGhostType, _id);
        }
    }

    public void DespawnAllEnemies()
    {

    }

    public void AddNewEnemy(int type)
    {
        Enemy _newEnemy = new Enemy();
        int _newId = enemies.Count;

        _newEnemy.ghostType = type;
        _newEnemy.EnemyId = _newId;

        enemies.Add(_newEnemy);

        SpawnEnemy(_newEnemy.ghostType, _newEnemy.EnemyId);
    }

    private void SpawnEnemy(int type, int id)
    {
        Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

        Enemy _newEnemy = Instantiate(enemy, SelectedSpawnPosition(), randomRotation);
        _newEnemy.name = "Ghost type" + type;
        _newEnemy.ghostType = type;
        _newEnemy.EnemyId= id;     

        //replace EnemyObject with spawned enemy
        enemies[id] = _newEnemy;

        Debug.Log("Enemy spawned!");
    }

    void DespawnEnemy(int id)
    {

    }

    private Vector3 SelectedSpawnPosition()
    {
        //float distanceToPlayer;
        //int closestDistanceIndex;

        //for (int i = 0; i < enemySpawnPoints.Count; i++)
        //{
        //distanceToPlayer = Vector3.Distance(enemySpawnPoints[i].transform.position, PlayerController.Instance.transform.position);


        //}

        int randomIndex;

        randomIndex = Random.Range(0, enemySpawnPoints.Count);

        return enemySpawnPoints[randomIndex].transform.position;
    }
}
