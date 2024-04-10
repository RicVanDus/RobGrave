using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

// Off screen indicator that shows up whenever object moves out of view
//
// Custom icon to show. Size variable. Cursor pointing at object. Padding variable. 

public class OffScreenIndicator : MonoBehaviour
{
    [SerializeField] private GameObject _indicator;
    [SerializeField] private Transform _target;
    [SerializeField] private GameObject _cursorCanvas;
    [SerializeField] private GameObject _imageCanvas;
    [SerializeField] private Image _image;
    
    [SerializeField] private Sprite _icon;
    [SerializeField] private float _screenPadding;
    [SerializeField] private bool _cursor;

    private Camera _uiCam;
    private bool _iconVisible;
    private Vector3 _iconRot;

    private void Awake()
    {
        _iconRot = new Vector3(90f, 0f, 0f);
        _cursorCanvas.SetActive(false);
    }

    void Start()
    {
        _uiCam = LootManager.Instance.UICam;
        _image.sprite = _icon;
        IconVisible(false);
        _cursorCanvas.transform.eulerAngles = _iconRot;
    }

    void FixedUpdate()
    {
        // checks item pos in viewport
        var viewportPos = _uiCam.WorldToViewportPoint(_target.position);

        if (ClampVector3(viewportPos) != viewportPos)
        {
            if (!_iconVisible)
                IconVisible(true);
            
            Vector3 iconPos = viewportPos;
            float iconPadding = 0.05f;
        
            iconPos.x = Mathf.Clamp(iconPos.x, 0f + _screenPadding, 1f - _screenPadding);
            iconPos.y = Mathf.Clamp(iconPos.y, 0f + _screenPadding * _uiCam.aspect/2, 1f - _screenPadding * _uiCam.aspect/2);

            Vector3 iconWorldPos =  _uiCam.ViewportToWorldPoint(iconPos);
            
            transform.position = iconWorldPos;

            if (_cursor)
            {
                // Rotate towards the target
                var rotDir = transform.position - _target.position;
                
                // Calculate the angle in degrees
                float angle = Mathf.Atan2(rotDir.y, rotDir.x) * Mathf.Rad2Deg;
                
                // Set the rotation of the canvas
                _cursorCanvas.transform.localEulerAngles = new Vector3(0f, 0f, angle+90f);
            }
        }
        else
        {
            if (_iconVisible)
                IconVisible(false);
        }
    }

    private Vector3 ClampVector3(Vector3 screenPos)
    {
        bool outsideScreen;
        Vector3 screenPosClamped = new Vector3(
            Mathf.Clamp(screenPos.x, 0f, 1f),
            Mathf.Clamp(screenPos.y, 0f, 1f),
            screenPos.z);

        return screenPosClamped;
    }
    
    private void IconVisible(bool toggle)
    {
        _indicator.SetActive(toggle);
        _iconVisible = toggle;
        
        if (_cursor)
            _cursorCanvas.SetActive(true);
    }    
}
