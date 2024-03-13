using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
    [SerializeField] private Button _spinWheelBtn;


    public int powerupBlockIndex;
    public Powerups _powerup;
    public float _chance;

    private Color _greenColor;
    private Color _blueColor;
    private Color _purpleColor;
    private Color _greyColor;
    private Vector4 _colorMult = Vector4.one;

    private Vector3 _defaultBtnScale;

    private bool btnDisabled = false;
    
    void Start()
    {
        _chance = 100f / 3;
        _defaultBtnScale = _button.transform.localScale;
        _button.interactable = false;

        if (powerupBlockIndex == 0)
        {
            _colorMult = Vector4.one * 0.8f;
        }
        else if (powerupBlockIndex == 2)
        {
            _colorMult = Vector4.one * 1.2f;
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

            _button.interactable = true;
            _button.enabled = true;
            _button.transform.localScale = _defaultBtnScale;
            btnDisabled = false;

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
            
            _spotLight.color = clr;
            
            _name.text = powerup.name;
            _name.color = clr;
            
            _description.text = powerup.description;
            _icon.sprite = powerup.icon;
            _button.interactable = true;
            _iconBg.GetComponent<Renderer>().material.SetColor("_BaseColor", clr);
            _iconBg.GetComponent<Renderer>().material.SetColor("_EmissionColor", clr);
            
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
            _wheelOption.SetImage(clr, powerup.icon);
            _wheelOption.fill = _chance / 100f;
            _wheelOption.optionIndex = powerupBlockIndex;
            SetWheelOptionRotation();
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
            _wheelOption.SetImage(clr, _icon.sprite);
        }
    }
    
    public void SetChance()
    {
        if (!btnDisabled)
        {
            PowerupManager.Instance.PowerupChanceSet(powerupBlockIndex);
            Vector3 toScale = _defaultBtnScale;
            toScale *= 0.75f;
        
            _button.interactable = false;
            btnDisabled = true;

            _button.transform.DOScale(toScale, 0.2f).SetUpdate(true).OnComplete(() =>
            {
                _button.transform.DOScale(_defaultBtnScale, 0.4f).SetUpdate(true).OnComplete(() =>
                {
                    _button.interactable = true;
                    _button.Select();
                    btnDisabled = false;
                });
            });            
        }
    }

    public void IncreaseChance()
    {
        if (_chance == 100f / 3)
        {
            _chance = 50f;
        }
        else
        {
            _chance += 10f;
            if (_chance > 70f)
            {
                _button.interactable = false;
                Vector3 toScale = _defaultBtnScale;
                toScale.y = 0f;
                _button.transform.DOScale(toScale, 0.7f).SetEase(Ease.InBounce).SetUpdate(true).OnComplete(() =>
                {
                    _button.enabled = false;
                    _spinWheelBtn.Select();
                });                
            }
        }
        _wheelOption.fill = _chance / 100f;
    }

    public void DecreaseChance()
    {
        if (_chance == 100f / 3)
        {
            _chance = 25f;
        }
        else
        {
            _chance -= 5f;
        }
        _wheelOption.fill = _chance / 100f;
    }

    public void HideButton()
    {
        if (!btnDisabled)
        {
            _button.interactable = false;
            _button.enabled = true;
            btnDisabled = true;
            Vector3 toScale = _defaultBtnScale;
            toScale.y = 0f;
            _button.transform.DOScale(toScale, 0.7f).SetEase(Ease.InBounce).SetUpdate(true).OnComplete(() =>
            {
                _button.enabled = false;
            });
        }
    }

    private void SetWheelOptionRotation()
    {
        if (powerupBlockIndex == 1)
        {
            
        }
        else if (powerupBlockIndex == 2)
        {
            
        }
        
    }
}
