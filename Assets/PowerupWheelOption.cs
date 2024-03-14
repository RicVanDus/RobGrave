using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

public class PowerupWheelOption : MonoBehaviour
{
    private Image wheelImg;
    [SerializeField] private GameObject _wheelIcon;
    private Sprite _wheelIconSprite;
    
    public float fill;
    public float rotationZ;
    private Vector3 _rotZVector3;
    private Vector3 _iconScale;

    public bool updatePosition = false;
    public int optionIndex;

    private void Awake()
    {
        wheelImg = GetComponent<Image>();
        _wheelIconSprite = _wheelIcon.GetComponent<Image>().sprite;
    }

    private void Start()
    {
        _iconScale = _wheelIcon.transform.localScale;

    }

    private void Update()
    {
        // check if we going to update the position (set by powerupblock)

        wheelImg.fillAmount = fill;
        PlaceIcon();
        RotateWheelOption();            
    }

    public void SetImage(Color clr, Sprite icon)
    {
        _wheelIcon.GetComponent<Image>().sprite = icon;
        wheelImg.color = clr;
        wheelImg.fillAmount = fill;
        PlaceIcon();
        RotateWheelOption();
    }

    private void PlaceIcon()
    {
        //ChatGPT
        float fillPercentage = fill /2;  // Change this to your actual fill percentage
        float angle = 2 * Mathf.PI * fillPercentage - Mathf.PI / 2;  // Calculate angle based on fill percentage, subtract Mathf.PI / 2 to start at the top

        float x = Mathf.Cos(angle) * 25f;  // Calculate x coordinate
        float y = Mathf.Sin(angle) * 25f;  // Calculate y coordinate

        Vector3 iconPos = new Vector3();
        iconPos = _wheelIcon.transform.localPosition;
        iconPos.x = x;
        iconPos.y = y;
        
        _wheelIcon.transform.localPosition = iconPos;

        var iconScaler = Mathf.Lerp(0.4f, 1.2f, fill);
        
        //scale icon
        var newIconScale = _iconScale * iconScaler;
        _wheelIcon.transform.localScale = newIconScale;
    }

    private void RotateWheelOption()
    {
        if (optionIndex == 1)
        {
            var rotZ = (PowerupManager.Instance.powerupBlocks[0]._chance / 100f) * 360f;
            rotationZ = rotZ;
        }
        else if (optionIndex == 2)
        {
            var rotZ = PowerupManager.Instance.powerupBlocks[0]._chance / 100f * 360f;
            rotZ += PowerupManager.Instance.powerupBlocks[1]._chance / 100f * 360f;
            rotationZ = rotZ;
        }
        
        _rotZVector3.z = rotationZ;
        transform.localEulerAngles = _rotZVector3;
        
        //counter rotate the icon
        Vector3 iconRot = new Vector3();
        iconRot = _rotZVector3;
        iconRot.x = -180f;

        _wheelIcon.transform.localEulerAngles = iconRot;
    }
}
