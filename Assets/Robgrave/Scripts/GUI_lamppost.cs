using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_lamppost : MonoBehaviour
{

    public Lamppost _Lamppost;
    [HideInInspector] public bool PlayerIsInteracting;
    [HideInInspector] public float FillAmount = 0f;

    private bool startedRedBlinking = false;

    private Image _radialFill;
    private Image _circle1;
    private Image _circle2;
    private Image _circle3;

    private Color _filledColor = new Color(0.9f, 1f, 0f);
    private Color _defaultColor = new Color(0.70f, 0.75f, 0.3f);
    private Color _redColor = new Color(1f, 0f, 0f);

    [HideInInspector] public bool ShowGraphic = false;
    private bool IsVisible = false;

    [HideInInspector] public int currentLightStages = 0;

    private CanvasGroup _canvasGroup;
        // Start is called before the first frame update
    void Start()
    {
        _radialFill = transform.Find("RadialFill").GetComponent<Image>();
        _circle1 = transform.Find("RadialFill").Find("Circle").Find("Circle1").GetComponent<Image>();
        _circle2 = transform.Find("RadialFill").Find("Circle").Find("Circle2").GetComponent<Image>();
        _circle3 = transform.Find("RadialFill").Find("Circle").Find("Circle3").GetComponent<Image>();
        _canvasGroup = transform.GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {        
        if (ShowGraphic && !IsVisible)
        {
            _canvasGroup.alpha = 1;
            IsVisible = true;
        }
        else if (!ShowGraphic && IsVisible)
        {
            _canvasGroup.alpha = 0;
            IsVisible = false;
        }

        if (ShowGraphic)
        {
            UpdateUI();
        }
    }

    //Do all magic here.
    private void UpdateUI()
    {                
        if (PlayerIsInteracting)
        {
            _radialFill.color = _filledColor;
        }
        else
        {
            _radialFill.color = _redColor;
        }

        if (IsVisible)
        {
            if (currentLightStages == 1)
            {                
                _circle1.color = _filledColor;
                _circle2.color = _defaultColor;
                _circle3.color = _defaultColor;
            }
            else if (currentLightStages == 2)
            {
                _circle1.color = _filledColor;
                _circle2.color = _filledColor;
                _circle3.color = _defaultColor;
            }
            else if (currentLightStages == 3)
            {
                _circle1.color = _filledColor;
                _circle2.color = _filledColor;
                _circle3.color = _filledColor;
            }
            else
            {
                _circle1.color = _defaultColor;
                _circle2.color = _defaultColor;
                _circle3.color = _defaultColor;
            }
        }

        _radialFill.fillAmount = FillAmount;
    }
}
