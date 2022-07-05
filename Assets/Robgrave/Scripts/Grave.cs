using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grave : Interactable
{
    private int graveType = 0;
    private int maxDepth;
    private int currentDepth;
    private float diggingProgress = 0f;
    private float diggingtTime = 3.0f;

    [SerializeField] private Text _depthNumber;
    

    public bool playerIsDigging = false;
    private bool graveIsDug = false;


    private void Awake()
    {
        
    }

    protected override void Start()
    {
        SetGraveType(graveType);
        SetDepthText();
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
                break;

            case 1:
                maxDepth = 4;
                break;

            case 2:
                maxDepth = 5;
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
                if (currentDepth >= maxDepth)
                {
                    graveIsDug = true;
                }
            }

        }
        else
        {
            diggingProgress -= Time.deltaTime;
        }

        diggingProgress = Mathf.Clamp(diggingProgress, 0.0f, diggingtTime);

    }
}
