using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WishingWell : MonoBehaviour
{

    private int _giftBoxesSpawned = 0;

    private float _payingProgress = 0f;

    private bool _playerPays = false;
    
    // needs to check if giftbox is picked up before giving the option to pay for the next one 
    
    private void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}

