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
    private float _maxLightTime = 20f;
    private bool _lightIsOn = false;

    private bool _playerCanInteract = false;
    private bool _playerIsInteracting = false;

    // Start is called before the first frame update
    void Start()
    {
        _lamp1Mat = _lamp1.GetComponent<Renderer>().material;
        _lamp2Mat = _lamp2.GetComponent<Renderer>().material;
    }

    private void Update()
    {
        
        if (_playerCanInteract && PlayerController.Instance._interacting)
        {
            _lightTimer = Mathf.Clamp(_lightTimer + Time.deltaTime, 0f, _maxLightTime);
            StopCoroutine(FlashingLight());
            if (!_lightIsOn) ToggleLight(true);
            _playerIsInteracting = true;
        }
        else
        {
            _lightTimer = Mathf.Clamp(_lightTimer - Time.deltaTime, 0f, _maxLightTime);
            _playerIsInteracting = false;
        }

        if (_lightTimer < (_maxLightTime * 0.2f) && _lightTimer > 0f && !_playerIsInteracting)
        {
            StartCoroutine(FlashingLight());
        }
        else if (_lightTimer == 0f)
        {
            ToggleLight(false);
            StopCoroutine(FlashingLight());
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
        do
        {
            ToggleLight(!_lightIsOn);

            yield return new WaitForSeconds(0.5f);
        } while (_lightTimer != 0f);
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
