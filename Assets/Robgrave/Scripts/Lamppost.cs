using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening.Core.Easing;
using UnityEngine;
using UnityEngine.UIElements;

public class Lamppost : MonoBehaviour
{

    [SerializeField] private GameObject _lamp1;
    [SerializeField] private GameObject _lamp2;
    [SerializeField] private GameObject _cone;
    [SerializeField] private Light _spotLight;
    [SerializeField] SphereCollider _collider;
    [SerializeField] private GameObject _UI;
    
    private Material _lamp1Mat;
    private Material _lamp2Mat;
    private GUI_lamppost _GUI_lamppost;
    private bool _guiVisible = false;

    private float _lightTimer;
    private float _maxLightTime = 2f;
    private bool _lightIsOn = true;
    private int _lightStage;
    private int _maxLightStages = 3;

    private bool _playerCanInteract = false;
    private bool _playerIsInteracting = false;
    private bool _lightIsflashing = false;

    // Start is called before the first frame update
    void Start()
    {
        _lamp1Mat = _lamp1.GetComponent<Renderer>().material;
        _lamp2Mat = _lamp2.GetComponent<Renderer>().material;
        _GUI_lamppost = _UI.GetComponent<GUI_lamppost>();
    }

    private void Update()
    {
        if (_lightStage > 0 && !_lightIsOn && !_lightIsflashing)
        {
            ToggleLight(true);
        }
        else if (_lightStage == 0 && _lightIsOn && !_lightIsflashing)
        {
            ToggleLight(false);
        } 
        
        if (_playerCanInteract)
        {
            if (PlayerController.Instance._interacting)
            {
                TimerChange(true);
            }
            else
            {
                TimerChange(false);
            }

            if (!_guiVisible)
            {
                _GUI_lamppost.ShowGraphic = true;
                _guiVisible = true;                
            }

        }
        else
        {
            {
                TimerChange(false);
            }
        }

        if (_lightStage == 1 && _lightTimer < (_maxLightTime/2) && !_playerIsInteracting)
        {
            if (!_lightIsflashing)
            {
                StartCoroutine(FlashingLight());
            }
        }
        else
        {
            if (_lightIsflashing)
            {
                StopCoroutine(FlashingLight());
                _lightIsflashing = false;
            }
        }

        if (!_playerCanInteract && _lightTimer == 0f)
        {
            if (_guiVisible)
            {
                _GUI_lamppost.ShowGraphic = false;
                _guiVisible = false;
            }
        }
        else
        {
            _GUI_lamppost.PlayerIsInteracting = _playerIsInteracting;
            _GUI_lamppost.currentLightStages = _lightStage;
            _GUI_lamppost.FillAmount = (_lightTimer % _maxLightTime) / _maxLightTime;
        }

    }

    private void ToggleLight(bool toggle)
    {
        _lightIsOn = toggle;
        _collider.enabled = toggle;
        _spotLight.enabled = toggle;
        _cone.SetActive(toggle);
    }

    private IEnumerator FlashingLight()
    {
        _lightIsflashing = true;
        do
        {
            ToggleLight(!_lightIsOn);

            yield return new WaitForSeconds(0.5f);
        } while (_lightIsflashing);
    }

    private void TimerChange(bool add)
    {
        if (add)
        {
            _playerIsInteracting = true;
            
            if (_lightStage < 3)
            {
                _lightTimer += Time.deltaTime;

                if (_lightStage < (int)(_lightTimer / _maxLightTime))
                {
                    _lightStage = (int)(_lightTimer / _maxLightTime);
                }

            }
            else
            {
                _lightTimer += Time.deltaTime;
                if (_lightTimer >= _maxLightTime * _maxLightStages)
                {
                    _lightTimer = _maxLightTime * _maxLightStages;
                }
            }
        }
        else
        {
            _playerIsInteracting = false;
            if (_lightStage > 0)
            {
                _lightTimer -= Time.deltaTime/4;
                if (_lightTimer > 0f)
                {
                    if (_lightStage > (int)Mathf.Ceil(_lightTimer / _maxLightTime))
                    {
                        _lightStage = (int)Mathf.Ceil(_lightTimer / _maxLightTime);
                    }
                }
                else
                {
                    _lightTimer = 0f;
                    _lightStage = 0;
                }
            }
            else
            {
                _lightTimer = 0f;
            }
        }
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerCanInteract = true;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerCanInteract = false;
        }
    }
}
