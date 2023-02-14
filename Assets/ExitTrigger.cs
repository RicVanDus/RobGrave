using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitTrigger : MonoBehaviour
{
    public GameObject UI;
    public Image radialImg;
    public Image exitSign;

    private bool _playerOnExit = false;

    private float _holdTimer = 0f;
    private float _holdTime = 3f;
    
    
    // Start is called before the first frame update
    void Start()
    {
        PlayerController.Instance.ChangingScore += SetExitSignColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerOnExit && PlayerController.Instance.goalAchieved && PlayerController.Instance._interacting)
        {
            PlayerInteracting();
        }
        else
        {
            _holdTimer -= Time.deltaTime;
            _holdTimer = Math.Clamp(_holdTimer, 0f, _holdTime);
            radialImg.fillAmount = _holdTimer / _holdTime;
        }
    }

    private void Awake()
    {
        //PlayerController.Instance.ChangingScore += SetExitSignColor;
        //Debug.Log(PlayerController.Instance.goalAchieved);
    }

    private void OnDestroy()
    {
        PlayerController.Instance.ChangingScore -= SetExitSignColor;
    }

    private void PlayerInteracting()
    {
        _holdTimer += Time.deltaTime;

        radialImg.fillAmount = _holdTimer / _holdTime;
        
        if (_holdTimer > _holdTime)
        {
            PlayerExit();
        }
    }
    
    
    private void PlayerExit()
    {
        GameManager.Instance.PlayerExtract();
    }

    private void ShowUI(bool show)
    {
        SetExitSignColor();
        if (show)
        {
            UI.SetActive(true);
        }
        else
        {
            UI.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShowUI(true);
            _playerOnExit = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShowUI(false);
            _playerOnExit = false;
        }
    }

    private void SetExitSignColor()
    {
        if (PlayerController.Instance.goalAchieved)
        {
            exitSign.color = new Color(0f, 1f, 0f, 0.5f);
        }
        else
        {
            exitSign.color = new Color(1f,0f,0f,0.5f);
        }
        
    }
}
