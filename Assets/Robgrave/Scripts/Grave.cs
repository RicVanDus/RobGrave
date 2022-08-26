using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grave : Interactable
{
    public int graveType = 0;
    private int maxDepth;
    private int currentDepth = 0;
    private float diggingProgress = 0f;
    private int defiledDepth = 0;
    private float diggingtTime = 3.0f;
    private float defileTime = 5.0f;
    private float defileProgress = 0f;
    public int valuables;

    [SerializeField] private Text _depthNumber;

    Gravedirt _gravedirt;

    public bool playerIsDigging = false;
    private bool graveIsDug = false;
    private bool graveTouched = false;

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
        if (PlayerController.Instance._interacting)
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
            graveTouched = true;

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
        LootManager.Instance.SpawnLoot(valuables, spawnNr);
    }

    private void DefiledGrave()
    {
        int _defDepth = currentDepth - defiledDepth;

        if (_defDepth > 0 && currentDepth != maxDepth)
        {
            defileProgress += Time.deltaTime;
            Debug.Log(" Defiling grave! ");

            if (defileProgress >= defileTime)
            {
                int _rnd1 = Random.Range(0, 5);
                int _rnd2 = Random.Range(0, 5);

                if (_rnd1 == _rnd2)
                {
                    Debug.Log("DEFILEMENT: You are haunted!");
                    EnemyManager.Instance.AddNewEnemy(graveType);
                }
                else
                {
                    Debug.Log("DEFILEMENT: You are lucky...");
                }

                defiledDepth++;
                defileProgress = 0f;
            }
        }
    }
}


/*   TODO: Make functionality when diggingProces reaches 0 and is touched. It will tick down each segment and each segment has a 10% chance of spawning the ghost  */