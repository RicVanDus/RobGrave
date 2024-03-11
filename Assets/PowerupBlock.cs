using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerupBlock : MonoBehaviour
{
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private TMP_Text _valueAmount;
    [SerializeField] private Image _icon;
    [SerializeField] private GameObject _iconBg;
    [SerializeField] public PowerupWheelOption _wheelOption;
    [SerializeField] private Button _button;
    [SerializeField] private TMP_Text _buttonText;
    [SerializeField] private Light _spotLight;


    public int powerupBlockIndex;
    public Powerups _powerup;
    public float _chance;

    private Color _greenColor;
    private Color _blueColor;
    private Color _purpleColor;
    private Color _greyColor;
    private float _colorMult = 1f;
    
    void Start()
    {
        _chance = 100f / 3;

        if (powerupBlockIndex == 0)
        {
            _colorMult = 0.9f;
        }
        else if (powerupBlockIndex == 2)
        {
            _colorMult = 1.1f;
        }

        _greenColor = PowerupManager.Instance.greenColor * _colorMult;
        _blueColor = PowerupManager.Instance.blueColor * _colorMult;
        _purpleColor = PowerupManager.Instance.purpleColor * _colorMult;
        _greyColor = PowerupManager.Instance.greyColor * _colorMult;
    }

    public void SetPowerup(Powerups powerup)
    {

        if (powerup != null)
        {
            _powerup = powerup;
            Color clr = Color.yellow;

            switch (powerup.type)
            {
                case PowerupType.green :
                    clr = _greenColor;
                    break;
                case PowerupType.blue :
                    clr = _blueColor;
                    break;
                case PowerupType.purple :
                    clr = _purpleColor;
                    break;
                default:
                    clr = _greyColor;
                    break;
            }

            _spotLight.color = Color.yellow;

            _name.text = powerup.name;
            _name.color = clr;
            
            _description.text = powerup.description;
            _icon.sprite = powerup.icon;
            _button.interactable = true;
            _iconBg.GetComponent<Renderer>().material.SetColor("_Color", clr);

            string itemDescription = "";

            switch (powerup.item)
            {
                case "boots" :
                    // check current multiplier, then "+[current amount] -> +[new amount]% speed"
                    itemDescription = "runnnnnn";
                    break;
                case "shovel" :
                    itemDescription = "dig dig dig";
                    break;
                default :
                    itemDescription = "n/a";
                    break;
            }

            _valueAmount.text = itemDescription;            
        }
        else
        {
            Color clr = _greyColor;
            _button.interactable = false;
            
            _name.text = "";
            _description.text = "";
            _icon.sprite = null;
            _iconBg.GetComponent<Renderer>().material.SetColor("_Color", clr);
            _spotLight.enabled = false;
        }
    }

    public void SetChance(float value)
    {
        _chance = value;
    }
}
