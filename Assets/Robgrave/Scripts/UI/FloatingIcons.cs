using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

// HOLDS A LIST OF FLOATING ICONS / OFFSCREENINDICATORS
// Can toggle visibility
// Variables like screen padding etc.

public class FloatingIcons : MonoBehaviour
{
    [SerializeField] 
    private Camera _mainCam;

    [NonSerialized] 
    public List<GameObject> floatingIcons = new List<GameObject>();
    [NonSerialized]
    public List<GameObject> offScreenIndicators = new List<GameObject>();

    [SerializeField] 
    private GameObject _directionIndicatorPrefab;

    [SerializeField]
    private GameObject _floatingIconPrefab;

    private Vector3 _playerPos;

    public float IconScreenPadding = 0.02f;
    
    void Start()
    {
        _mainCam = PlayerController.Instance.cam;
    }

    void Update()
    {
        
    }
    
    //Add to list and instantiate
    public void AddFloatingIcon(FloatingIcon floatingIcon)
    {
        
    }
    
    //Add to list and instantiate this + dir.indicator
    public void AddOffScreenIndicator(OffScreenIndicator offScreenIndicator)
    {
        GameObject newOffscreenIndicator = Instantiate(_floatingIconPrefab, gameObject.transform);

        FloatingIcon newFloatingIcon = newOffscreenIndicator.GetComponent<FloatingIcon>();
        newFloatingIcon.ImageIcon = offScreenIndicator.ImageIcon;
        newFloatingIcon.IconSize = offScreenIndicator.IconSize;
        newFloatingIcon.Target = offScreenIndicator.gameObject;
        
        newFloatingIcon.Initialize();
        
        offScreenIndicators.Add(newOffscreenIndicator);
    }
    
    
}
