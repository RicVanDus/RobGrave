using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
    [SerializeField] private GameObject _powerupBg;

    [SerializeField] private PowerupBlock _powerupBlock1;
    [SerializeField] private PowerupBlock _powerupBlock2;
    [SerializeField] private PowerupBlock _powerupBlock3;

    [SerializeField] private Button _button;
    [SerializeField] private GameObject _wheel;
    [SerializeField] private GameObject _wheelTurn;
    [SerializeField] private AnimationCurve _spinCurve = new();

    [SerializeField] private Powerups[] _powerups;
    private List<Powerups> _powerupOptionsGreen = new();
    private List<Powerups> _powerupOptionsBlue = new();
    private List<Powerups> _powerupOptionsPurple = new();
    private PowerupUIChest _powerupUIChest;
    public List<PowerupBlock> powerupBlocks = new();
    
    private Vector3 _wheelDefaultPos;
    private Vector3 _wheelTurnDefaultRot;
    private Vector3 _wheelOffPos;
    private Vector3 _defBtnScale;
    
    private int _chestType;
    private Powerups _chosenPowerup;

    [Header("Colors")] [SerializeField] public Color greenColor;
    [SerializeField] public Color blueColor;
    [SerializeField] public Color purpleColor;
    [SerializeField] public Color greyColor;

    private int chosenIndex;
    private bool _wheelIsSpinning;
    
    private WaitForSeconds _wait1s = new (1f);


    /// <summary>
    /// POWERUP VARS
    /// </summary>
    ///

    private bool _energyDrinkActive = false;
    private float _energyDrinkTime = 0f;
    private float _energyDrinkTimer = 0f;
    private float _energyDrinkValue;
    
    
    
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

        powerupBlocks.Add(_powerupBlock1);
        powerupBlocks.Add(_powerupBlock2);
        powerupBlocks.Add(_powerupBlock3);

        _button.interactable = false;
        _wheelDefaultPos = _wheel.transform.localPosition;
        _wheelOffPos = _wheelDefaultPos;
        _wheelOffPos.x += 40f;

        _defBtnScale = _button.transform.localScale;
        _wheelTurnDefaultRot = _wheelTurn.transform.localEulerAngles;
        
        StartCoroutine(SoftTick());
    }

    private void Update()
    {
        if (_wheelIsSpinning)
        {
            // if its more than option.rotation && less than option.rotation + 360*fill then JIPPIE.
            for (int i = 0; i < powerupBlocks.Count; i++)
            {

                var wheelRot = 360f - _wheelTurn.transform.localEulerAngles.y;

                if (wheelRot > powerupBlocks[i]._wheelOption.transform.localEulerAngles.z
                    && wheelRot < powerupBlocks[i]._wheelOption.transform.localEulerAngles.z +
                    (360f * powerupBlocks[i]._wheelOption.fill))
                {
                    powerupBlocks[i].chosen = true;
                }
                else
                {
                    powerupBlocks[i].chosen = false;
                }
            }
        }
    }

    public void ChestOpen(int type)
    {
        _button.interactable = false;
        _button.transform.localScale = _defBtnScale;
        _wheel.transform.localPosition = _wheelOffPos;
        _wheelTurn.transform.localEulerAngles = _wheelTurnDefaultRot;
        
        _powerupUIChest.SetChest(type);
        _chestType = type;
        _uiAll.SetActive(true);
        Time.timeScale = 0f;

        Vector3 bgDefPos = _powerupBg.transform.localPosition;
        var bgNewPos = bgDefPos;
        bgNewPos.x = -55f;

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
        _powerupBg.transform.localPosition = bgNewPos;
        _powerupBg.transform.DOLocalMove(bgDefPos, 0.4f).SetEase(Ease.OutSine).SetUpdate(true);
        StartCoroutine(StartAnimation());
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
            }
            else if (rndNr > 90f && _powerupOptionsBlue.Count > 0)
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
            }
            else if (rndNr > 10f && _powerupOptionsBlue.Count > 0)
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
            }
            else if (rndNr > 10f && _powerupOptionsBlue.Count > 0)
            {
                int rndIndex = Random.Range(0, _powerupOptionsBlue.Count);
                chosenPowerup = _powerupOptionsBlue[rndIndex];
                _powerupOptionsBlue.Remove(chosenPowerup);
            }
            else if (_powerupOptionsPurple.Count > 0)
            {
                int rndIndex = Random.Range(0, _powerupOptionsPurple.Count);
                chosenPowerup = _powerupOptionsPurple[rndIndex];
                _powerupOptionsPurple.Remove(chosenPowerup);
            }
            else if (_powerupOptionsBlue.Count > 0)
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

        if (index == 0)
        {
            _powerupBlock1.SetPowerup(chosenPowerup);
        }
        else if (index == 1)
        {
            _powerupBlock2.SetPowerup(chosenPowerup);
        }
        else
        {
            _powerupBlock3.SetPowerup(chosenPowerup);
        }
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

        for (int i = 0; i < powerupBlocks.Count; i++)
        {
            if (powerupBlocks[i] == chosenPowerupBlock)
            {
                powerupBlocks[i].IncreaseChance();
            }
            else
            {
                powerupBlocks[i].DecreaseChance();
                powerupBlocks[i].HideButton();
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
    
    public void SpinWheel()
    {
        HideWheelButton();

        Vector3 rotateTo = new Vector3(0f, 0f, Random.Range(0f, 360f));
        // how many extra rotations before stopping
        rotateTo.z += 7 * 360f;

        // no more chance adding
        for (int i = 0; i < powerupBlocks.Count; i++)
        {
            powerupBlocks[i].HideButton();
        }

        _wheelIsSpinning = true;
        _wheelTurn.transform.DOLocalRotate(rotateTo, 4f, RotateMode.LocalAxisAdd).SetEase(_spinCurve).SetUpdate(true)
            .OnComplete(
                () =>
                {
                    _wheelIsSpinning = false;
                    StartCoroutine(WheelStopped());
                });
    }

    private IEnumerator WheelStopped()
    {
        yield return new WaitForSecondsRealtime(0.5f);

        for (int i = 0; i < powerupBlocks.Count; i++)
        {
            if (!powerupBlocks[i].chosen)
            {
                powerupBlocks[i].AnimateButtonDown();
            }
            else
            {
                _chosenPowerup = powerupBlocks[i]._powerup;
                powerupBlocks[i].chosen = false;
            }
        }

        yield return new WaitForSecondsRealtime(3f);

        ChestClose();
    }

    private void ChestClose()
    {
        Time.timeScale = 1f;
        _uiAll.SetActive(false);
        PowerupExecute(_chosenPowerup);
        _chosenPowerup = null;
    }

    private IEnumerator StartAnimation()
    {
        float pauseTime = 0.7f;
        bool setFocus = false;
        
        for (int i = 0; i < powerupBlocks.Count; i++)
        {
            yield return new WaitForSecondsRealtime(pauseTime);
            powerupBlocks[i].AnimateButtonUp();
        }
        
        _wheel.transform.DOLocalMove(_wheelDefaultPos, 0.7f).SetUpdate(true).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            _button.enabled = true;
        });
        
        yield return new WaitForSecondsRealtime(pauseTime);

        for (int i = 0; i < powerupBlocks.Count; i++)
        {
            powerupBlocks[i].EnableButton(true);
            if (!setFocus && powerupBlocks[i] != null)
            {
                powerupBlocks[i].button.Select();
                setFocus = true;
            }
        }

        _button.interactable = true;
    }

    public void HideWheelButton()
    {
        _button.interactable = false;
        _button.enabled = true;
        Vector3 toScale = _defBtnScale;
        toScale.y = 0f;
        _button.transform.DOScale(toScale, 0.7f).SetEase(Ease.InBounce).SetUpdate(true).OnComplete(() =>
        {
            _button.enabled = false;
        });
    }
    
    private void PowerupExecute(Powerups powerup)
    {
        /*
         * this will be a long dumb method, covering all the types of powerups
         */

        switch (powerup.item)
        {
            case "energydrink" :
                if (!_energyDrinkActive)
                {
                    _energyDrinkActive = true;
                    // add icon to the left and add it to the array
                }
                _energyDrinkTime += powerup.duration;
                _energyDrinkValue += powerup.value / 100f;
                PlayerController.Instance.moveSpeedMult += powerup.value / 100f;
                
                
                break;
            default :
                break;
        }
        
        UIMessages.Instance.CreateMessage(powerup.name, powerup.description, UIMessageType.Good, powerup.icon);
    }    

    // THIS IS ALL POWERUPS STUFF
    // All the timers, all the UI timer stuff. It's just a 1 second update, so not that precise, but noone cares. At least I don't.
    private IEnumerator SoftTick()
    {
        do
        {
            if (_energyDrinkActive)
            {
                _energyDrinkTimer += 1f;
                // fill: _energyDrinkTimer / _energyDrinkTime;

                if (_energyDrinkTimer >= _energyDrinkTime)
                {
                    _energyDrinkTime = 0f;
                    _energyDrinkTimer = 0f;
                    PlayerController.Instance.moveSpeedMult -= _energyDrinkValue;
                    _energyDrinkValue = 0f;                 

                    _energyDrinkActive = false;
                }
            }

            yield return _wait1s;
        } while (true);
    }

    private void AddIcon()
    {
        
    }
    
}