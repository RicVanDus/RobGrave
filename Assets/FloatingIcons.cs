using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

public class FloatingIcons : MonoBehaviour
{
    
    [SerializeField] 
    private RectTransform _testImage;
    
    private Camera _mainCam;

    [NonSerialized] 
    public List<GameObject> floatingIcons = new List<GameObject>();
    [NonSerialized]
    public List<GameObject> offScreenIndicators = new List<GameObject>();

    [SerializeField] 
    private GameObject _directionIndicator; 


    private Vector3 _playerPos;
    
    void Start()
    {
        _mainCam = PlayerController.Instance.cam;
    }

    void Update()
    {
        Vector3 _playerPos = PlayerController.Instance.transform.position;

        Vector3 playerPosScreen = _mainCam.WorldToScreenPoint(_playerPos);

        float posX = playerPosScreen.x - (Screen.width / 2f);
        float posY = playerPosScreen.y - (Screen.height / 2f);

        Vector3 newPos = new Vector3(posX, posY, 0f);
        
        _testImage.SetLocalPositionAndRotation(newPos, Quaternion.identity);
    }

    
    //updating Floating Icons
    private void UpdateFloatingIcons()
    {
        // first check if parentObject is still there, if not remove from list
        // animate?
    }

    // updating OffScreenIndicators
    private void UpdateOffScreenIndicators()
    {
        // first check if parentObject is still there, if not remove from list
        // update position
        // update direction of indicator
        
    }
    
    //Add to list and instantiate
    private void AddFloatingIcon(GameObject floatingIcon)
    {
        
    }
    
    //Add to list and instantiate this + dir.indicator
    private void AddOffScreenIndicator(GameObject floatingIcon)
    {
        
    }
    
}
