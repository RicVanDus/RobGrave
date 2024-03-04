using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{
    public static PowerupManager Instance;

    [SerializeField] private GameObject _uiBase;
    [SerializeField] private GameObject _uiAll;
    [SerializeField] private GameObject _powerupBlock;
    [SerializeField] private GameObject _chest;

    [SerializeField] private Powerups[] _powerups;
    private Powerups[] _availPowerups;
    private Powerups[] _powerupOptions;
    private PowerupUIChest _powerupUIChest;

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

    private void Start()
    {
        _powerupUIChest = _chest.GetComponent<PowerupUIChest>();
        _availPowerups = _powerups;
    }

    public void ChestOpen(int type)
    {
        _powerupUIChest.SetChest(type);
        _uiAll.SetActive(true);
        Time.timeScale = 0f;
        
        if (type == 0)
        {
            
        } else if (type == 1)
        {
        }
        else
        {
            
        }
        
        
        /*
         * Sets the 3 powerups.
         * Sets the chest type.
         * Starts animations 
         */
    }

    public void ChestClose()
    {
        Time.timeScale = 1f;
        _uiAll.SetActive(false);
    }
    
    private void PowerupValidation(Powerups powerup)
    {
        /*
         * If blue then we check if its 1 night (or when its value is given: goldbars)
         * If green we don't have to check
         * If purple we check the current amount of the powerup
         *
         * whenever the result is false we remove it from the available array
         */

    }

    private void PowerupExecute(Powerups powerup)
    {
        /*
         * this will be a long dumb method, covering all the types of powerups
         */


    }

    private void CreatePowerupOption(Powerups powerup, int index)
    {
        /*
         * Creating a powerup from the prefab and then add that to the array
         */
    }

    private void PowerupChanceSet(Powerups powerup)
    {

    }

    /*
     * methods needed:
     * - animations
     * - wheel of fortune chance set
     * - wheel of fortune chance update
     * - spinning the wheel
     * */
}