using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{
    public static PowerupManager Instance;
    
    /*
     * needs an array with powerups
     * needs another array with available powerups
     * method to check the powerup to be viable for selection
     * method to activate the powerup (probably big bullshit code to accomodate for all possible thingies)
     */

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}
