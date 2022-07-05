using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LootManager : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject debugObject;

    static public int _rows = 117;
    static public int _columns = 69;
    static private int _gridAmount = _rows * _columns;

    public float lootSpawnOuterRadius = 10f;
    public float lootSpawnInnerRadius = 0f;

    public float debugTime = 5f;
    private float debugTimer = 0f;

    //public LootPosition[] lootPositions = new LootPosition[_gridAmount];
    public List<LootPosition> lootPositions = new List<LootPosition>();

    public List<LootPosition> selectedPositions = new List<LootPosition>();

    
    private void Awake()
    {
        CreateLootGrid();
        
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        debugTimer += Time.deltaTime;

        if (debugTimer > debugTime)
        {
            // DEBUG FUNCTION
            Debug.Log("DEBUG: Running function");

            CreateSpawnPoints(6, lootSpawnInnerRadius, lootSpawnOuterRadius);

            Debug.Log(selectedPositions.Count);
            SpawnDebugObjects();

            debugTimer = 0f;
        }
    }


    private void CreateLootGrid()
    {
        float _zBase = -33f;
        float _y = 0.2f;
        float _xBase = -58f;
        int id = 0;

        //Columns
        for (int c = 0; c < _columns; c++)
        {
            var _z = _zBase + c;
            

            for (int r = 0; r < _rows; r++)
            {
                var _x = _xBase + r;
                var _lootVec = new Vector3(_x, _y, _z);

                if (CheckNavMesh(_lootVec))
                {
                    var _newLootPos = new LootPosition(_lootVec, true, id);

                    lootPositions.Add(_newLootPos);

                    id++;
                }
                
            }

        }

    }

    
    public void CreateSpawnPoints(int _amount, float _innerRadius, float _outerRadius)
    {
        selectedPositions.Clear();

        Vector3 playerPos = PlayerController.Instance.transform.position;

        // loop through spawn points and add everything within the radius to the selectpoints list, then randomly remove the amount of indexes you don't need

        for (int i = 0; i < lootPositions.Count; i++)
        {

            //Debug.Log(CheckDistance(_outerRadius, _innerRadius, playerPos, lootPositions[i].GridPosition));

            if (CheckDistance(_outerRadius, _innerRadius, playerPos, lootPositions[i].GridPosition))
            {
                var _newLootPos = new LootPosition();

                _newLootPos.Id = lootPositions[i].Id;
                _newLootPos.GridPosition = lootPositions[i].GridPosition;
                _newLootPos.Empty = lootPositions[i].Empty;

                selectedPositions.Add(_newLootPos);
            }
        }
    }

    private void SpawnDebugObjects()
    {
        for (int i = 0; i < selectedPositions.Count; i++)
        {
            var SpawnObject = Instantiate(debugObject, selectedPositions[i].GridPosition, debugObject.transform.rotation);
            SpawnObject.name = "lootPos-" + i;

        }
    }


    // Check if position is on navmesh
    private bool CheckNavMesh(Vector3 targetDestination)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetDestination, out hit, 0.7f, NavMesh.AllAreas))
        {
            return true;
        }
        return false;

    }

    // Check distance between 2 positions
    private bool CheckDistance(float maxDistance, float minDistance, Vector3 v1, Vector3 v2)
    {
        float _distance = Vector3.Distance(v1, v2);

        if (_distance < maxDistance && _distance > minDistance)
        {
            return true;
        }

        return false;
    }
}
