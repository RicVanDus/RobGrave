using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;


public class OffScreenIndicator : MonoBehaviour
{
    [NonSerialized] public GameObject _parentObj;
    public Sprite IconImage;
    public float Size = 1f;
    public bool bIsAnOffScreenIndicator;

    private Image _imageComponent;
    private RectTransform _rectTransform;

    private Camera _mainCam;
    private WaitForSeconds _wait03 = new(0.3f);

    void Start()
    {
        _mainCam = PlayerController.Instance.cam;
        StartCoroutine("CheckIfInFrame");
    }

    void Update()
    {

    }

    public void Initialize()
    {
        _imageComponent = GetComponent<Image>();
        _rectTransform = GetComponent<RectTransform>();

        _imageComponent.sprite = IconImage;

        _rectTransform.localScale = Vector3.one * Size;
    }

    private IEnumerator CheckIfInFrame()
    {
        while (true)
        {

            // check if position is within viewport
            Vector3 objViewportPos = _mainCam.WorldToViewportPoint(transform.position);


            if (objViewportPos.x < 0f || objViewportPos.x > 1f || objViewportPos.y < 0f || objViewportPos.y > 1f)
            {
                Debug.Log("OUT OF FRAME!!!!!!!!");
            }
            yield return _wait03;
        }
    }
}
