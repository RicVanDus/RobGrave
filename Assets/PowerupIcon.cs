using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerupIcon : MonoBehaviour
{
    [NonSerialized] public float _fill;
    [NonSerialized] public string _info;
    [NonSerialized] public Powerups _powerup;

    [SerializeField] private Image _iconFill;
    [SerializeField] private Image _iconImage;
    [SerializeField] private TextMeshProUGUI _infoText;

    void Start()
    {

    }

    void Update()
    {
        _iconFill.fillAmount = _fill;
    }

    public void SetPowerup(Powerups powerup)
    {
        _powerup = powerup;
        _infoText.text = _info;
        _iconImage.sprite = powerup.icon;

        if (powerup.type == PowerupType.green)
        {
            _iconFill.color = PowerupManager.Instance.greenColor;
        }
        else if (powerup.type == PowerupType.blue)
        {
            _iconFill.color = PowerupManager.Instance.blueColor;
        }
        else
        {
            _iconFill.color = PowerupManager.Instance.purpleColor;
        }
         
    }
}
