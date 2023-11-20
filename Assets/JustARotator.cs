using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustARotator : MonoBehaviour
{

    [SerializeField] private float _rotateSpeed;
    [SerializeField] private AxisSelector _axis;
    
    void Update()
    {
        Vector3 _currentRotation = transform.eulerAngles;

        if (_axis == AxisSelector.x)
        {
            _currentRotation.x += _rotateSpeed * Time.deltaTime;
        }
        else if (_axis == AxisSelector.y)
        {
            _currentRotation.y += _rotateSpeed * Time.deltaTime;
        }
        else
        {
            _currentRotation.z += _rotateSpeed * Time.deltaTime;
        }

        transform.eulerAngles = _currentRotation;
    }
}

public enum AxisSelector
{
    x, y, z
}