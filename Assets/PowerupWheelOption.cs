using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerupWheelOption : MonoBehaviour
{
    private Image wheelImg;
    [SerializeField] private Image _wheelIcon;

    private void Awake()
    {
        wheelImg = GetComponent<Image>();
    }



    public void SetImage(Color clr, Sprite icon)
    {
        _wheelIcon.sprite = icon;
        wheelImg.color = clr;
    }

}
