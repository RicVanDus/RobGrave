using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PowerupManager : MonoBehaviour
{
    public static PowerupManager Instance;

    [SerializeField] private GameObject _uiBase;
    [SerializeField] private GameObject _uiAll;
    [SerializeField] private GameObject _powerupBlock;
    [SerializeField] private GameObject _chest;

    [SerializeField] private PowerupBlock _powerupBlock1;
    [SerializeField] private PowerupBlock _powerupBlock2;
    [SerializeField] private PowerupBlock _powerupBlock3;
    
    [SerializeField] private Button _button;

    [SerializeField] private Powerups[] _powerups;
    private List<Powerups> _powerupOptionsGreen = new();
    private List<Powerups> _powerupOptionsBlue = new();
    private List<Powerups> _powerupOptionsPurple = new();
    private PowerupUIChest _powerupUIChest;

    private int _chestType;
    private Powerups _chosenPowerup;

    [Header("Colors")] 
    [SerializeField] public Color greenColor;
    [SerializeField] public Color blueColor;
    [SerializeField] public Color purpleColor;
    [SerializeField] public Color greyColor;

    private int chosenIndex;

    
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
        _uiAll.SetActive(false);
    }

    public void ChestOpen(int type)
    {
        _powerupUIChest.SetChest(type);
        _chestType = type;
        _uiAll.SetActive(true);
        Time.timeScale = 0f;

        chosenIndex = 3;
       
        // Make 3 arrays with the powerup rarities and check the blue/purple ones
        _powerupOptionsGreen.Clear();
        _powerupOptionsBlue.Clear();
        _powerupOptionsPurple.Clear();
        
        /* Check for valid options [ TODO ] */

        for (int i = 0; i < _powerups.Length; i++)
        {
            if (_powerups[i].type == PowerupType.green)
            {
                _powerupOptionsGreen.Add(_powerups[i]);
            }
            else if (_powerups[i].type == PowerupType.blue)
            {
                _powerupOptionsBlue.Add(_powerups[i]);
            }
            else if (_powerups[i].type == PowerupType.purple)
            {
                _powerupOptionsPurple.Add(_powerups[i]);
            }
        }
        
        /*
         * depending on type, it looks for a certain rarity.
         * Then checks if that option is available. If not, it will go to the rarity type below.
         * Random index, setting the option and remove item from array.
         */
        CreatePowerupOption(0);
        CreatePowerupOption(1);
        CreatePowerupOption(2);
        
        /*
         * Sets the 3 powerups.
         * Starts animations 
         */
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
    
    private void CreatePowerupOption(int index)
    {
        /*
         * Separate powerupoption script attached to optionBlock?
         * Sets the chance (instead of in the powerup itself, which is weird)
         * Makes up the option array
         * Wheel distribution is taken from that array and its chances
         */

        float rndNr = Random.Range(0f, 100f);
        Powerups chosenPowerup = null;

        if (_chestType == 0)
        {
            if (rndNr > 98f && _powerupOptionsPurple.Count > 0)
            {
                int rndIndex = Random.Range(0, _powerupOptionsPurple.Count);
                chosenPowerup = _powerupOptionsPurple[rndIndex];
                _powerupOptionsPurple.Remove(chosenPowerup);
            } else if (rndNr > 90f && _powerupOptionsBlue.Count > 0)
            {
                int rndIndex = Random.Range(0, _powerupOptionsBlue.Count);
                chosenPowerup = _powerupOptionsBlue[rndIndex];
                _powerupOptionsBlue.Remove(chosenPowerup);
            }
            else if (_powerupOptionsGreen.Count > 0)
            {
                int rndIndex = Random.Range(0, _powerupOptionsGreen.Count);
                chosenPowerup = _powerupOptionsGreen[rndIndex];
                _powerupOptionsGreen.Remove(chosenPowerup);
            }
        } 
        else if (_chestType == 1)
        {
            if (rndNr > 95f && _powerupOptionsPurple.Count > 0)
            {
                int rndIndex = Random.Range(0, _powerupOptionsPurple.Count);
                chosenPowerup = _powerupOptionsPurple[rndIndex];
                _powerupOptionsPurple.Remove(chosenPowerup);
            } else if (rndNr > 10f && _powerupOptionsBlue.Count > 0)
            {
                int rndIndex = Random.Range(0, _powerupOptionsBlue.Count);
                chosenPowerup = _powerupOptionsBlue[rndIndex];
                _powerupOptionsBlue.Remove(chosenPowerup);
            }
            else
            {
                int rndIndex = Random.Range(0, _powerupOptionsGreen.Count);
                chosenPowerup = _powerupOptionsGreen[rndIndex];
                _powerupOptionsGreen.Remove(chosenPowerup);
            }
        }
        else
        {
            if (rndNr > 95f && _powerupOptionsGreen.Count > 0)
            {
                int rndIndex = Random.Range(0, _powerupOptionsGreen.Count);
                chosenPowerup = _powerupOptionsGreen[rndIndex];
                _powerupOptionsGreen.Remove(chosenPowerup);
            } else if (rndNr > 10f && _powerupOptionsBlue.Count > 0)
            {
                int rndIndex = Random.Range(0, _powerupOptionsBlue.Count);
                chosenPowerup = _powerupOptionsBlue[rndIndex];
                _powerupOptionsBlue.Remove(chosenPowerup);
            }
            else
            {
                int rndIndex = Random.Range(0, _powerupOptionsPurple.Count);
                chosenPowerup = _powerupOptionsPurple[rndIndex];
                _powerupOptionsPurple.Remove(chosenPowerup);
            }
        }

        if (index == 0)
        {
            _powerupBlock1.SetPowerup(chosenPowerup);
        } else if (index == 1)
        {
            _powerupBlock2.SetPowerup(chosenPowerup);
        }
        else
        {
            _powerupBlock3.SetPowerup(chosenPowerup);
        }
    }

    private void PowerupExecute(Powerups powerup)
    {
        /*
         * this will be a long dumb method, covering all the types of powerups
         */
    }

    public void PowerupChanceSet(int blockIndex)
    {
        /*
         * increasing one of the powerupOptions, decreasing the chance of the others by the same amount, rotate graphics accordingly
         * after 2 indexes have been adjusted, disable the button on index 3
         * don't adjust the values negatively of the chosen 2 indices
         * first time adjusting is to 50%, after that it's +10% up to 90%
         * 25% - 25% / 60% - 20 - 20 / 70 - 15 - 15 / 80 - 10 - 10
         */

        chosenIndex = blockIndex;

        List<PowerupBlock> _powerupBlocks = new();
        _powerupBlocks.Add(_powerupBlock1);
        _powerupBlocks.Add(_powerupBlock2);
        _powerupBlocks.Add(_powerupBlock3);

        PowerupBlock chosenPowerupBlock = null;  

        if (chosenIndex == 0)
        {
            chosenPowerupBlock = _powerupBlock1;
        }
        else if (chosenIndex == 1)
        {
            chosenPowerupBlock = _powerupBlock2;
        }
        else if (chosenIndex == 2)
        {
            chosenPowerupBlock = _powerupBlock3;
        }

        for (int i = 0; i < _powerupBlocks.Count; i++)
        {
            if (_powerupBlocks[i] == chosenPowerupBlock)
            {
                _powerupBlocks[i].IncreaseChance();
            }
            else
            {
                _powerupBlocks[i].DecreaseChance();
                _powerupBlocks[i].HideButton();
            }
        }
        
        
    }

    /*
     * methods needed:
     * - animations
     * - wheel of fortune chance set
     * - wheel of fortune chance update
     * - spinning the wheel
     * */
    
    public void ChestClose()
    {
        Time.timeScale = 1f;
        _uiAll.SetActive(false);
        PowerupExecute(_chosenPowerup);
        _chosenPowerup = null;
    }
    
    
}