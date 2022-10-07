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

    public delegate void OnEnemyUpdate();
    public event OnEnemyUpdate EnemyUpdate;

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

    // SpawnAllEnemies - spawns all enemies from current level, adding them to the list. Id = 
    // DespawnAllEnemies - destroys all enemies, clears (list)
    // SpawnEnemy(Type) - Spawns enemy of Type
    // DespawnEnemy(id) - Destroys Enemy of id (list)
    // 

    public void SpawnAllEnemies()
    {
        LevelProperties _levelProps = GameManager.Instance.thisLevel;

        int _ghostTypes0 = _levelProps.ghostSpawnType0;
        int _ghostTypes1 = _levelProps.ghostSpawnType1;
        int _ghostTypes2 = _levelProps.ghostSpawnType2;

        for (int i = 0; i < _ghostTypes0; i++)
        {
            SpawnEnemy(0);
        }

        for (int i = 0; i < _ghostTypes1; i++)
        {
            SpawnEnemy(1);
        }


        for (int i = 0; i < _ghostTypes2; i++)
        {
            SpawnEnemy(2);
        }
    }

    public void DespawnAllEnemies()
    {

    }

    public void SpawnEnemy(int type)
    {
        Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

        Enemy _newEnemy = Instantiate(enemy, SelectedSpawnPosition(), randomRotation);
        _newEnemy.name = "Ghost type" + type;
        _newEnemy.ghostType = type;
        _newEnemy.EnemyId = enemies.Count;

        enemies.Add(_newEnemy);
        
        Debug.Log("Enemy spawned: [ID: " + _newEnemy.EnemyId + "]!");

        EnemyUpdate?.Invoke();
    }

    public void DespawnEnemy(int id)
    {
        // TEST THIS
        Enemy _enemyToDespawn = enemies[id];        
        enemies.Remove(_enemyToDespawn);
        Destroy(_enemyToDespawn);

        EnemyUpdate?.Invoke();
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
