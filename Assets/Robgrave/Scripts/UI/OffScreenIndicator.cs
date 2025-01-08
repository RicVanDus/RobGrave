using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;


public class OffScreenIndicator : MonoBehaviour
{
    [NonSerialized] public GameObject _parentObj;
    public Sprite ImageIcon;
    public float IconSize = 1f;
    public GameObject IconTarget;

    private Image _imageComponent;
    private RectTransform _rectTransform;

    private Camera _mainCam;
    private WaitForSeconds _wait03 = new(0.3f);

    private bool bIndicatorSet = false;

    private float timer;
    private bool bChecking;    
    

    void Start()
    {
        _mainCam = PlayerController.Instance.cam;
        IconTarget = gameObject;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 3f && !bChecking)
        {
            StartCoroutine("CheckIfInFrame");
            bChecking = true;
        }
    }

    private IEnumerator CheckIfInFrame()
    {
        while (true)
        {
            // check if position is within viewport
            Vector3 objViewportPos = _mainCam.WorldToViewportPoint(transform.position);

            if (objViewportPos.x < 0f || objViewportPos.x > 1f || objViewportPos.y < 0f || objViewportPos.y > 1f)
            {
                if (!bIndicatorSet)
                {
                    GameManager.Instance.FloatingIcons.AddOffScreenIndicator(this);
                    bIndicatorSet = true;    
                }
            }
            yield return _wait03;
        }
    }
}
