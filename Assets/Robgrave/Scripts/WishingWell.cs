using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WishingWell : MonoBehaviour
{
    [SerializeField] private GameObject _posIndicator;
    
    
    private int _giftBoxesSpawned = 0;

    private float _payingProgress = 0f;

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
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        
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