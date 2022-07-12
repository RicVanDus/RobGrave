using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LootManager : MonoBehaviour
{
    public static LootManager Instance;


    public GameObject debugObject;

    static public int _rows = 117;
    static public int _columns = 69;
    static private int _gridAmount = _rows * _columns;

    public float lootSpawnOuterRadius = 10f;
    public float lootSpawnInnerRadius = 0f;

    public bool Debug_Lootmanager = false;


    public float debugTime = 10f;
    private float debugTimer = 0f;

    //public LootPosition[] lootPositions = new LootPosition[_gridAmount];
    public List<LootPosition> lootPositions = new List<LootPosition>();

    public List<LootPosition> selectedPositions = new List<LootPosition>();

    [SerializeField]
    public List<ValuableTemplate> valuables = new List<ValuableTemplate>();







    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

    }

    void Start()
    {
        CreateLootGrid();
    }

    // Update is called once per frame
    void Update()
    {
        debugTimer += Time.deltaTime;

        if (debugTimer > debugTime)
        {
            // *** DEBUG FUNCTION ***
            //Debug.Log("DEBUG: Running LootManager function");

            //CreateSpawnPoints(6, lootSpawnInnerRadius, lootSpawnOuterRadius);
            //Debug.Log(selectedPositions.Count);
            //SpawnDebugObjects();

            //SpawnLoot(322, 20);

            debugTimer = 0f;
        }
    }


    // Function that spawns loot around the player position
    // the amount is the loot value, it will create a loot pool that has a minimum and max amount of drops. If it doesn't fit the slots it needs to extend its radius and fill the SelectionList once again.
    // It randomly chooses an index in the Selection List and then set the bool of the LootPosition List to the correct state (by saved ID)
    // Selection List needs to save ID 
    //
    //
    //  
    // CreateSpawnPoints - check for bool, add to radius if not big enough, assign loot to random index (spawn in and remove from list?), clear list
    // 
    //
    //
    //
    //
    //
    //
    public void SpawnLoot(int value, int spawns)
    {
        ValuableTemplate randomValuable = new ValuableTemplate();

        float _newInnerRadius = lootSpawnInnerRadius;
        float _newOuterRadius = lootSpawnOuterRadius;
        bool _createdSpawnPoints = false;


        CreateSpawnPoints(spawns, _newInnerRadius, _newOuterRadius);

        do
        {
            if (selectedPositions.Count >= spawns)
            {
                _createdSpawnPoints = true;
            }
            else
            {
                _newOuterRadius += 2f;
                CreateSpawnPoints(spawns, _newInnerRadius, _newOuterRadius);
            }
        } while (_createdSpawnPoints == false);


        // **** DEBUG ****
        if (Debug_Lootmanager)
        {
            Debug.Log(spawns + " / " + selectedPositions.Count + " Radius: " + _newInnerRadius + "/" + _newOuterRadius);
            //SpawnDebugObjects();
        }

        for (int i = 0; i < spawns; i++)
        {
            float avg = value / (spawns-i);

            if (avg > 20f && value > 375)
            {
                randomValuable = GetRandomValuable(5);
            }
            else if (avg > 15f && value > 175)
            {
                randomValuable = GetRandomValuable(4);
            }
            else if (avg > 12f && value > 75)
            {
                randomValuable = GetRandomValuable(3);
            }
            else if (avg > 10f && value > 35)
            {
                randomValuable = GetRandomValuable(2);
            }
            else if (avg > 5f && value > 15)
            {
                randomValuable = GetRandomValuable(1);
            }
            else if (value > 5)
            {
                randomValuable = GetRandomValuable(0);
            }
            else
            {
                Debug.Log("STOP");
                randomValuable = null;
            }

            if (randomValuable != null)
            {
                // add loot to new List and Spawn Object
                value -= randomValuable.value;

                int _randomIndex = Random.Range(1, selectedPositions.Count);
                int _id = selectedPositions[_randomIndex].Id;

                LootPosition _newLP = lootPositions[_id];
                _newLP.Empty = false;
                lootPositions[_id] = _newLP;

                // Here comes the loot
                SpawnLootMesh(randomValuable, selectedPositions[_randomIndex].GridPosition);

                selectedPositions.RemoveAt(_randomIndex);

                if (Debug_Lootmanager)
                {
                    Debug.Log(randomValuable.name + " - value left: " + value + " - AVG: " + avg);
                }
            }
        }


        // loop through new list of spawned objects, assign to 


    }

    private ValuableTemplate GetRandomValuable(int n)
    {
        int randomIndex = 0;

        switch (n)
        {
            case 0:
                randomIndex = Random.Range(0, 0);
                break;
            case 1:
                randomIndex = Random.Range(0, 2);
                break;
            case 2:
                randomIndex = Random.Range(0, 3);
                break;
            case 3:
                randomIndex = Random.Range(1, 4);
                break;
            case 4:
                randomIndex = Random.Range(2, 5);
                break;
            case 5:
                randomIndex = Random.Range(3, 6);
                break;

        }

        if (Debug_Lootmanager)
        {
            Debug.Log(n + " - " + randomIndex);
        }
        

        return valuables[randomIndex];

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

        // loop through spawn points and add everything within the radius to the selectpoints list

        for (int i = 0; i < lootPositions.Count; i++)
        {
            if (CheckDistance(_outerRadius, _innerRadius, playerPos, lootPositions[i].GridPosition) && lootPositions[i].Empty)
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

    private void SpawnLootMesh(ValuableTemplate valuable, Vector3 position)
    {
        var SpawnObject = Instantiate(valuable.mesh, position, valuable.mesh.transform.rotation);
        SpawnObject.name = valuable.name;
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
