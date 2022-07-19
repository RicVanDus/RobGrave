using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grave : Interactable
{
    public int graveType = 0;
    private int maxDepth;
    private int currentDepth;
    private float diggingProgress = 0f;
    private float diggingtTime = 3.0f;
    public int valuables;

    [SerializeField] private Text _depthNumber;

    Gravedirt _gravedirt;

    public bool playerIsDigging = false;
    private bool graveIsDug = false;


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

        PlayerIsDiggingOrNot();
    }

    private void CheckPlayerInteract()
    {
        if (PlayerController.Instance.interact1)
        {
            playerIsDigging = true;
        }
        else
        {
            playerIsDigging = false;
        }
    }

    private void SetDepthText()
    {
        _depthNumber.text = currentDepth + " / " + maxDepth;
    }

    private void SetGraveType(int graveType)
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
            diggingProgress += (Time.deltaTime * PlayerController.Instance.digSpeedMultiplier);

            if (diggingProgress >= diggingtTime)
            {
                currentDepth += 1;
                diggingProgress = 0f;
                SetDepthText();
                _gravedirt.DirtHeight(currentDepth);
                if (currentDepth >= maxDepth)
                {
                    graveIsDug = true;
                    SpawnLoot();
                }
            }

        }
        else
        {
            diggingProgress -= Time.deltaTime;
        }
        diggingProgress = Mathf.Clamp(diggingProgress, 0.0f, diggingtTime);
    }


    private void SpawnLoot()
    {
        int spawnNr = Random.Range(17, 26);
        LootManager.Instance.SpawnLoot(valuables, spawnNr);
    }
}
