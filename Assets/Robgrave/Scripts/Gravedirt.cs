using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravedirt : MonoBehaviour
{
    private Material graveDirt;
    private float _buldgeHeight;
    private float _oldBuldgeHeight;
    private float _dirtChangeSpeed = 0.5f;
    private float _dirtTimer = 0f;
    private bool _changeDirt = false;
    private Color _dirtColorDefault = new Color(0.2830189f / 1.2f, 0.2144298f / 1.2f, 0.1559273f / 1.2f);
    private Color _dirtColorInteract = new Color(0.2830189f * 1.2f, 0.2144298f * 1.2f, 0.1559273f * 1.2f);


    // Start is called before the first frame update
    void Start()
    {
        graveDirt = gameObject.GetComponent<Renderer>().material;
        _buldgeHeight = 30f;
    }

    // Update is called once per frame
    void Update()
    {
        if (_changeDirt)
        {
            ChangeDirtHeight(Time.deltaTime);
        }
    }

    public void DirtHeight(int height)
    {
        switch (height)
        {
            case 0:
                break;
            case 1:
                _buldgeHeight = 5f;
                break;
            case 2:
                _buldgeHeight = -15f;
                break;
            case 3:
                _buldgeHeight = -35f;
                break;
            case 4:
                _buldgeHeight = -70f;
                break;
            case 5:
                _buldgeHeight = -90f;
                break;
            case 6:
                _buldgeHeight = -160f;
                break;

        }

        _oldBuldgeHeight = graveDirt.GetFloat("_Buldge_Height");
        _changeDirt = true;
    }

    private void ChangeDirtHeight(float deltaTime)
    {
        float newBuldgeHeight;

        _dirtTimer += deltaTime / _dirtChangeSpeed;
        newBuldgeHeight = Mathf.Lerp(_oldBuldgeHeight, _buldgeHeight, _dirtTimer);

        if (_dirtTimer >= 1f)
        {
            _changeDirt = false;
            _dirtTimer = 0f;
        }
        
        //Debug.Log(_changeDirt + " [ old: " + _oldBuldgeHeight + " new:  " + _buldgeHeight + " ] - " + speed + " - " + newBuldgeHeight);

        graveDirt.SetFloat("_Buldge_Height", newBuldgeHeight);
    }

    public void ChangeDirtColor(bool CanInteract)
    {
        if (CanInteract)
        {
            graveDirt.SetColor("_Color", _dirtColorDefault);
        }
        else
        {
            graveDirt.SetColor("_Color", _dirtColorDefault);
        }
    }
}
