using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WishingWell : MonoBehaviour
{
    [SerializeField] private GameObject _posIndicator;
    
    
    private int _giftBoxesSpawned = 0;

    private float _payingProgress = 0f;
    private float _payTime = 3f;
    private float _cooldownTime = 30f;
    private float _cooldownTimer = 0f;

    private bool _onCooldown = false;
    
    private bool _playerPays = false;
    private bool _playerCanInteract = false;
    private bool _wellIsDry = false;

    private WaitForSeconds _wait03 = new(0.3f); 
    
    // needs to check if giftbox is picked up before giving the option to pay for the next one 

    private void Start()
    {
        StartCoroutine(PosIndicator());
    }

    private void Update()
    {
        if (_playerCanInteract)
        {
            if (PlayerController.Instance._interacting && !_onCooldown)
            {
                _payingProgress += Time.deltaTime;
                Debug.Log(_payingProgress);
                if (_payingProgress > _payTime)
                {
                    _giftBoxesSpawned++;
                    LootManager.Instance.SpawnGiftBox(_giftBoxesSpawned+1, true, 0);
                    
                    if (_giftBoxesSpawned > 4)
                    {
                        _wellIsDry = true;
                    }

                    _payingProgress = 0f;
                    _onCooldown = true;
                }
            }
        }
        else
        {
            _payingProgress -= Time.deltaTime;
            _payingProgress = Mathf.Clamp(_payingProgress, 0f, 10f);
        }

        if (_onCooldown)
        {
            _cooldownTimer += Time.deltaTime;
            if (_cooldownTimer > _cooldownTime)
            {
                _onCooldown = false;
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

    private IEnumerator PosIndicator()
    {
        float minDistance = 10f;

        do
        {
            if (!_playerCanInteract && !PlayerController.Instance._canInteract && !_wellIsDry)
            {
                if (Vector3.Distance(transform.position, PlayerController.Instance.transform.position) < minDistance)
                {
                    _posIndicator.SetActive(true);
                }
                else
                {
                    _posIndicator.SetActive(false);
                }
            }
            else
            {
                _posIndicator.SetActive(false);
            }

            yield return _wait03;
        } while (true);
    }
}