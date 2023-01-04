using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Grave : Interactable
{
    public int graveType = 0;
    public int maxDepth;
    public int currentDepth = 0;
    public float diggingProgress = 0f;
    public int defiledDepth = 0;
    public float diggingtTime = 3.0f;
    public float defileTime = 5.0f;
    public float defileProgress = 0f;
    public int valuables;

    [SerializeField] private Text _depthNumber;

    Gravedirt _gravedirt;    

    public bool playerIsDigging = false;
    public bool graveIsDifiling = false;
    private bool graveIsDug = false;
    private bool graveTouched = false;
    private bool graveDefiled = false;
    private bool _rotatedPlayer = false;



    private void Awake()
    {
        
    }

    protected override void Start()
    {
        SetGraveType(graveType);
        SetDepthText();

        _gravedirt = GetComponentInChildren<Gravedirt>();
    }

    protected override void Update()
    {
        if (PlayerCanInteract && !graveIsDug)
        {
            CheckPlayerInteract();
        }
        else
        {
            playerIsDigging = false;
        }

        _gravedirt.ChangeDirtColor(PlayerCanInteract);

        PlayerIsDiggingOrNot();
    }

    private void CheckPlayerInteract()
    {
        if (PlayerController.Instance._interacting)
        {
            playerIsDigging = true;
            if (_rotatedPlayer == false)
            {
                RotatePlayer();    
            }
            
        }
        else
        {
            playerIsDigging = false;
            _rotatedPlayer = false;
        }
    }

    private void SetDepthText()
    {
        _depthNumber.text = currentDepth + " / " + maxDepth;
    }

    public void SetGraveType(int graveType)
    {
        switch (graveType)
        {
            case 0:
                maxDepth = 3;
                valuables = Random.Range(125, 250);
                break;

            case 1:
                maxDepth = 4;
                valuables = Random.Range(350, 500);
                break;

            case 2:
                maxDepth = 5;
                valuables = Random.Range(600, 1000);
                break;
        }
    }

    private void PlayerIsDiggingOrNot()
    {
        if (playerIsDigging)
        {
            PlayerController.Instance.movementDisabled = true;
            PlayerController.Instance.IsDigging(true);
            
            diggingProgress += (Time.deltaTime * PlayerController.Instance.digSpeedMultiplier);
            if (defileProgress > 0f)
            {
                defileProgress -= Time.deltaTime;
            }            
            graveTouched = true;
            graveIsDifiling = false;

            if (diggingProgress >= diggingtTime)
            {
                currentDepth += 1;
                diggingProgress = 0f;
                SetDepthText();
                _gravedirt.DirtHeight(currentDepth);
                if (currentDepth >= maxDepth)
                {
                    PlayerController.Instance.IsDigging(false);
                    graveIsDug = true;
                    GameObject _graveStone = transform.Find("gravestone").gameObject;
                    Color _defaultColor = GameManager.Instance.graveDefaultColor;
                    _graveStone.GetComponent<MeshRenderer>().material.SetColor("_Color", _defaultColor);

                    SpawnLoot();
                }
            }

        }
        else
        {
            diggingProgress -= Time.deltaTime;
            if (diggingProgress <= 0f && graveTouched)
            {
                DefiledGrave();
            }
        }
        diggingProgress = Mathf.Clamp(diggingProgress, 0.0f, diggingtTime);
    }


    private void SpawnLoot()
    {
        int spawnNr = Random.Range(17, 26);

        if (defiledDepth > 0)
        {
            valuables = (int)(valuables - (valuables * (0.2f * defiledDepth)));
            spawnNr = spawnNr - defiledDepth;
        }
        LootManager.Instance.SpawnLoot(valuables, spawnNr);
    }

    private void DefiledGrave()
    {
        int _defDepth = currentDepth - defiledDepth;
        graveIsDifiling = false;

        if (_defDepth > 0 && currentDepth != maxDepth && graveDefiled == false)
        {
            graveIsDifiling = true;
            defileProgress += Time.deltaTime;

            if (defileProgress >= defileTime)
            {
                int _rnd1 = Random.Range(0, 5);
                int _rnd2 = Random.Range(0, 5);

                if (_rnd1 == _rnd2)
                {
                    graveDefiled = true;
                    Debug.Log("DEFILEMENT: You are haunted!");
                    EnemyManager.Instance.SpawnEnemy(graveType);
                    
                }
                else
                {
                    Debug.Log("DEFILEMENT: You are lucky...");
                }

                defiledDepth++;
                defileProgress = 0f;
                graveIsDifiling = false;
            }
        }
    }

    private void RotatePlayer()
    {
        _rotatedPlayer = true;
        
        PlayerController.Instance.RotateToGrave(this.GameObject());
    }    
}


