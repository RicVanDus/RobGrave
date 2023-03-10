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
    
    private Material _lamp1Mat;
    private Material _lamp2Mat;

    private float _lightTimer;
    private float _maxLightTime = 2f;
    private bool _lightIsOn = true;
    private int _lightStage;

    private bool _playerCanInteract = false;
    private bool _playerIsInteracting = false;
    private bool _lightIsflashing = false;

    // Start is called before the first frame update
    void Start()
    {
        _lamp1Mat = _lamp1.GetComponent<Renderer>().material;
        _lamp2Mat = _lamp2.GetComponent<Renderer>().material;
    }

    private void Update()
    {
        Debug.Log("LANTAARN - Stage[" + _lightStage + "] - Timer: " + _lightTimer);

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
        }
        else
        {
            {
                TimerChange(false);
            }
        }

        if (_lightStage == 1 && !_playerIsInteracting)
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
                if (_lightTimer >= _maxLightTime)
                {
                    _lightStage++;
                    if (_lightStage != 3)
                    {
                        _lightTimer = 0f;
                    }
                    else
                    {
                        _lightTimer = _maxLightTime;
                    }
                }
            }
            else
            {
                _lightTimer += Time.deltaTime;
                if (_lightTimer >= _maxLightTime)
                {
                    _lightTimer = _maxLightTime;
                }
            }
        }
        else
        {
            _playerIsInteracting = false;
            if (_lightStage > 0)
            {
                _lightTimer -= Time.deltaTime/4;
                if (_lightTimer <= 0f)
                {
                    _lightStage--;
                    if (_lightStage > 0)
                    {
                        _lightTimer = _maxLightTime;
                    }
                    else
                    {
                        _lightTimer = 0f;
                    }
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
