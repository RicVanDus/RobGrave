using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Sequence = DG.Tweening.Sequence;

public class PowerupBlock : MonoBehaviour
{
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private TMP_Text _valueAmount;
    [SerializeField] private Image _icon;
    [SerializeField] private GameObject _iconBg;
    [SerializeField] public PowerupWheelOption _wheelOption;
    [SerializeField] public Button button;
    [SerializeField] private TMP_Text _buttonText;
    [SerializeField] private TMP_Text _buttonPrice;
    [SerializeField] private Light _spotLight;
    [SerializeField] private Button _spinWheelBtn;
    [SerializeField] private GameObject _blockMaterial;
    
    public int powerupBlockIndex;
    public Powerups _powerup;
    public float _chance;
    public float _newChance;

    private Color _greenColor;
    private Color _blueColor;
    private Color _purpleColor;
    private Color _greyColor;
    private Vector4 _colorMult = Vector4.one;
    private Material _blockMat;

    private Vector3 _defaultBtnScale;

    private Vector3 _defaultPos;
    private Vector3 _downPos;

    private bool btnDisabled = false;

    private float _wheelFill;
    private float _updateDur = 0.6f;
    public bool wheelSpinning;

    private bool isHighlighted = false;
    public bool chosen;

    private int _clicked = 0;
    private int _price = 0;
    
    void Start()
    {
        if (powerupBlockIndex == 0)
            _colorMult = Vector4.one * 0.8f;
        else if (powerupBlockIndex == 2)
            _colorMult = Vector4.one * 1.2f;

        _greenColor = PowerupManager.Instance.greenColor * _colorMult;
        _blueColor = PowerupManager.Instance.blueColor * _colorMult;
        _purpleColor = PowerupManager.Instance.purpleColor * _colorMult;
        _greyColor = PowerupManager.Instance.greyColor * _colorMult;
        
        //start out of frame
        _defaultPos = transform.localPosition;
        _downPos = _defaultPos;
        _downPos.y -= 40f;
        _defaultBtnScale = button.transform.localScale;

        _blockMat = _blockMaterial.gameObject.GetComponent<Renderer>().material;
        SetPrice();
    }

    private void Update()
    {
        _wheelOption.fill = _chance / 100f;

        if (chosen)
            HighLightBlock(true);
        else
            HighLightBlock(false);
    }
    
    public void SetPowerup(Powerups powerup)
    {
        _wheelOption.fill = _chance / 100f;
        _chance = 100f / 3;
        _price = 0;
        _clicked = 0;
        transform.localPosition = _downPos;
        HighLightBlock(false);
        
        button.interactable = false;
        button.enabled = true;
        btnDisabled = true;
        SetPrice();
        
        //chosen = false;
        
        if (powerup != null)
        {
            _powerup = powerup;
            Color clr = Color.yellow;

            button.transform.localScale = _defaultBtnScale;

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
                case "cherries" :
                    itemDescription = "regain 1 heart";
                    break;
                case "lightningstrike" :
                    itemDescription = "ghosts are visible\n(+" + powerup.duration + "s)";
                    break;
                case "fullmoon" :
                    itemDescription = "ghosts are slower\n(+" + powerup.duration + "s)";
                    break;                
                case "goldbars" :
                    itemDescription = "$" + powerup.value;
                    break; 
                case "moneystack" :
                    itemDescription = "+" + powerup.value + "% valuables\n(+" + powerup.duration + "s)";
                    break; 
                case "energydrink" :
                    itemDescription = "+" + powerup.value + "% speed boost\n(+" + powerup.duration + "s)";
                    break;
                default :
                    itemDescription = "n/a";
                    break;
            }

            var valueString = powerup.value.ToString();
            var durationString = powerup.duration.ToString();
            var technicalReplace1 = powerup.technical.Replace("#v", valueString);
            var technicalReplace2 = technicalReplace1.Replace("#d", durationString).ToString();

            itemDescription = technicalReplace2;
                
            _valueAmount.text = itemDescription;
            _wheelOption.SetImage(clr, powerup.icon);
            _wheelOption.optionIndex = powerupBlockIndex;
        }
        else
        {
            Color clr = _greyColor;
            button.interactable = false;
            button.enabled = false;
            btnDisabled = true;
            
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
            _clicked++;
            PlayerController.Instance.AddPreScoreToScoreDirectly(-_price);
            PowerupManager.Instance.PowerupChanceSet(powerupBlockIndex);
            Vector3 toScale = _defaultBtnScale;
            toScale *= 0.75f;
        
            button.interactable = false;
            btnDisabled = true;

            if (_clicked < 4)
            {
                button.transform.DOScale(toScale, 0.2f).SetUpdate(true).OnComplete(() =>
                {
                    Sequence seq = DOTween.Sequence().SetUpdate(true);
                
                    seq.SetDelay(0.3f);
                    seq.Delay();

                    seq.Append(button.transform.DOScale(_defaultBtnScale, 0.2f)).SetUpdate(true).OnComplete(() =>
                    {
                        _price = CalculatePrice();
                        SetPrice();

                        if (_price > PlayerController.Instance.score || _clicked > 4)
                        {
                            
                            _spinWheelBtn.Select();
                        }
                        else
                        {
                            Debug.Log("DUM DUM DUM");
                            button.interactable = true;
                            button.Select();
                            btnDisabled = false;
                        }
                    });
                
                    seq.SetDelay(0.3f);
                    seq.Delay();
                
                    seq.Play();
                });
            }
        }
        else
        {
            _spinWheelBtn.Select();
        }
    }

    public void IncreaseChance()
    {
        if (_chance == 100f / 3)
        {
            _wheelOption.updatePosition = true;
            DOTween.To(()=> _chance, x=> _chance = x, 50f, _updateDur).SetUpdate(true).OnComplete(() =>
            {
                _wheelOption.updatePosition = false;
            });
        }
        else
        {
            _wheelOption.updatePosition = true;
            DOTween.To(()=> _chance, x=> _chance = x, _chance+10f, _updateDur).SetUpdate(true).OnComplete(() =>
            {
                _wheelOption.updatePosition = false;
            });
            //maybe remove this part?
            if (_chance > 70f)
            {
                button.interactable = false;
                Vector3 toScale = _defaultBtnScale;
                toScale.y = 0f;
                button.transform.DOScale(toScale, 0.7f).SetEase(Ease.InBounce).SetUpdate(true).OnComplete(() =>
                {
                    button.enabled = false;
                    _spinWheelBtn.Select();
                });                
            }
        }
    }

    public void DecreaseChance()
    {
        if (_chance == 100f / 3)
        {
            _wheelOption.updatePosition = true;
            DOTween.To(()=> _chance, x=> _chance = x, 25f, _updateDur).SetUpdate(true).OnComplete(() =>
            {
                _wheelOption.updatePosition = false;
            });
        }
        else
        {
            _wheelOption.updatePosition = true;
            DOTween.To(()=> _chance, x=> _chance = x, _chance-5f, _updateDur).SetUpdate(true).OnComplete(() =>
            {
                _wheelOption.updatePosition = false;
            });
        }
    }

    public void HideButton()
    {
        
        if (!btnDisabled)
        {
            button.interactable = false;
            button.enabled = true;
            btnDisabled = true;
            Vector3 toScale = _defaultBtnScale;
            toScale.y = 0f;
            button.transform.DOScale(toScale, 0.7f).SetEase(Ease.InBounce).SetUpdate(true).OnComplete(() =>
            {
                button.enabled = false;
            });
        }
    }

    public void EnableButton(bool toggle)
    {
        button.interactable = toggle;
        btnDisabled = !toggle;
    }

    public void AnimateButtonUp()
    {
        transform.DOLocalMove(_defaultPos, 0.6f).SetEase(Ease.OutBounce).SetUpdate(true);
    }

    public void AnimateButtonDown()
    {
        transform.DOLocalMove(_downPos, 0.6f).SetEase(Ease.InBounce).SetUpdate(true);
    }
    
    public void HighLightBlock(bool toggle)
    {
       if (!isHighlighted && toggle)
       { 
           _blockMat.SetColor("_EmissionColor", Color.yellow * 0.02f);
            isHighlighted = true;
       }
       else if (isHighlighted && !toggle)
       {
            _blockMat.SetColor("_EmissionColor", Color.black);
            isHighlighted = false;
       }
    }

    private int CalculatePrice()
    {
        // depends on level valuables req, rarity and how many times pressed
        // level req: 1000 / 40 - 30 - 20 -(*1 * hitamount)
        // green: 25 
        // blue 50
        // purple: 75

        float sub = 20f;

        if (_powerup.type == PowerupType.green)
        {
            sub = 40f;
        }
        else if (_powerup.type == PowerupType.blue)
        {
            sub = 30f;
        }

        sub -= 1 * _clicked;

        var price = (int)(GameOverseer.Instance.thisLevel.valuablesRequired / sub);

        return price;
    }

    private void SetPrice()
    {
        _buttonPrice.text = "$" + _price;
    }
}