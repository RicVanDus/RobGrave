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
}
