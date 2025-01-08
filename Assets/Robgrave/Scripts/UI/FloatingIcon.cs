using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

// link to gameobject to get position, animates accordingly within viewport, nothing more..


public class FloatingIcon : MonoBehaviour
{
    public GameObject Target;
    public Sprite ImageIcon;
    public float IconSize;
    
    private RectTransform _rectTransform;
    private Image _image;
    private GameObject _player;
    private Camera _mainCam;
    private bool bInit = false;

    private float _iconScreenPadding;
    void Start()
    {
        
    }
    
    void Update()
    {
        if (bInit)
        {
            UpdatePosition();
        }
    }
    
    public void Initialize()
    {
        _image = GetComponent<Image>();
        _rectTransform = GetComponent<RectTransform>();
        _player = PlayerController.Instance.gameObject;
        _mainCam = PlayerController.Instance.cam;
        _iconScreenPadding = GameManager.Instance.FloatingIcons.IconScreenPadding;
        
        _image.sprite = ImageIcon;
        _rectTransform.localScale = Vector3.one * IconSize;

        bInit = true;
    }

    private void UpdatePosition()
    {
        // also check when on screen again: delete from Icons list and destroy itself
        
        Vector3 playerPos = _player.transform.position;
        Vector3 targetPos = Target.transform.position;

        Vector3 playerPosScreen = _mainCam.WorldToViewportPoint(playerPos);
        Vector3 targetPosScreen = _mainCam.WorldToViewportPoint(targetPos);
        
        // clamp and add screenpadding
        targetPosScreen.x = Mathf.Clamp(targetPosScreen.x, _iconScreenPadding, 1f - _iconScreenPadding);
        targetPosScreen.y = Mathf.Clamp(targetPosScreen.y, _iconScreenPadding, 1f - _iconScreenPadding);

        float playerPosX = playerPosScreen.x * Screen.width - (Screen.width / 2f);
        float playerPosY = playerPosScreen.y * Screen.height - (Screen.height / 2f);
        
        float targetPosX = targetPosScreen.x * Screen.width - (Screen.width / 2f);
        float targetPosY = targetPosScreen.y * Screen.height - (Screen.height / 2f);
        
        

        Vector3 newImagePos = new Vector3(targetPosX, targetPosY, 0f);
        
        _rectTransform.SetLocalPositionAndRotation(newImagePos, Quaternion.identity);
    }

}
