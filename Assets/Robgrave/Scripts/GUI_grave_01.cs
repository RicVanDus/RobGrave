using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GUI_grave_01 : MonoBehaviour
{

    public Grave _grave;
    public bool PlayerCanInteract;
    public bool GraveDefiling;

    private bool startedRedBlinking = false;

    private Image _radialFill;
    private Image _circle1;
    private Image _circle2;
    private Image _circle3;

    private Color _defaultColor = new Color(0.28f, 0.76f, 0.35f);
    private Color _filledColor = new Color(0.9f, 1f, 0f);
    private Color _noColor = new Color(0.75f, 0.75f, 0.75f);
    private Color _redColor = new Color(1f, 0f, 0f);

    private bool _isVisible = true;

    private Vector3 _localPos;
    private Quaternion _localRot;
    private Vector3 _newPos;

    // Start is called before the first frame update
    void Start()
    {
        _radialFill = transform.Find("RadialFill").GetComponent<Image>();
        _circle1 = transform.Find("RadialFill").Find("Base").Find("Circle").Find("Circle1").GetComponent<Image>();
        _circle2 = transform.Find("RadialFill").Find("Base").Find("Circle").Find("Circle2").GetComponent<Image>();
        _circle3 = transform.Find("RadialFill").Find("Base").Find("Circle").Find("Circle3").GetComponent<Image>();

        _localPos = transform.localPosition;
        _localRot = transform.localRotation;
        _newPos = _localPos;
        _newPos.y -= 1f;
    }

    // Update is called once per frame
    void Update()
    {        
        if (_grave != null)
        {
            PlayerCanInteract = _grave.PlayerCanInteract;
            GraveDefiling = _grave.graveIsDifiling;

            if (PlayerCanInteract || GraveDefiling || _grave.diggingProgress > 0f)
            {
                UpdateUI();

                if (!_isVisible)
                {
                    CanvasGroup _canvasGroup = transform.GetComponent<CanvasGroup>();
                    _canvasGroup.alpha = 1;
                    
                    _isVisible = true;
                }

            }
            else
            {
                if (_isVisible)
                {
                    CanvasGroup _canvasGroup = transform.GetComponent<CanvasGroup>();
                    _canvasGroup.alpha = 0; 
                    _isVisible = false;
                }

            }
        }        
    }

    //Do all magic here.
    private void UpdateUI()
    {                
        if (_grave.playerIsDigging)
        {
            _radialFill.color = _filledColor;
        }
        else
        {
            _radialFill.color = _noColor;
        }

        if (!GraveDefiling)
        {
            if (_grave.currentDepth == 1)
            {                
                _circle1.color = _filledColor;
                _circle2.color = _defaultColor;
                _circle3.color = _defaultColor;
            }
            else if (_grave.currentDepth == 2)
            {
                _circle1.color = _filledColor;
                _circle2.color = _filledColor;
                _circle3.color = _defaultColor;
            }
            else if (_grave.currentDepth == 3)
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


        if (GraveDefiling)
        {
            _radialFill.color = _redColor;
            _radialFill.fillAmount = 1 - (_grave.defileProgress / _grave.defileTime);
            _radialFill.fillClockwise = true;
            if (startedRedBlinking == false)
            {
                StartCoroutine(BlinkingRedCircle());
            }            
        }
        else
        {
            _radialFill.fillAmount = _grave.diggingProgress / _grave.diggingtTime;
            _radialFill.fillClockwise = true;
            startedRedBlinking = false;
            if (startedRedBlinking)
            {
                StopCoroutine(BlinkingRedCircle());                
            }                        
        }

        if (_grave.defiledDepth == 1)
        {
            _circle1.color = _noColor;
        }
        else if (_grave.defiledDepth == 2)
        {
            _circle1.color = _noColor;
            _circle2.color = _noColor;
        }

    }


    public void SetUIPosition()
    {
        Vector3 _newPos;
        Quaternion _newRot;

        if (_grave != null)
        {
            _newPos = new Vector3(0f, 0.1f, -0.5f);
            
            if (Mathf.Round(_grave.transform.eulerAngles.y) == 0)
            {
                _newRot = Quaternion.Euler(0f, 0f, 0f); 
            }
            else if (Mathf.Round(_grave.transform.eulerAngles.y) == 180)
            {
                _newRot = Quaternion.Euler(0f, 180f, 0f);
            }
            else if (Mathf.Round(_grave.transform.eulerAngles.y) == 90)
            {
                _newRot = Quaternion.Euler(0f, -90f, 0f);
            }
            else
            {
                _newRot = Quaternion.Euler(0f, 90f, 0f);
            }

            transform.localRotation = _newRot;
            transform.localPosition = _newPos;
        }
    }

    private IEnumerator BlinkingRedCircle()
    {
        Image _circle = _circle1;

        startedRedBlinking = true;

        while (GraveDefiling)
        {
            if (_grave.defiledDepth == 0)
            {
                _circle = _circle1;
            }
            else if (_grave.defiledDepth == 1)
            {
                _circle = _circle2;
            }
            else if (_grave.defiledDepth == 2)
            {
                _circle = _circle3;
            }

            if (_circle.color == _redColor)
            {
                _circle.color = _filledColor;
            }
            else
            {
                _circle.color = _redColor;
            }

            yield return new WaitForSeconds(0.5f);
        }                
        
    }

}
