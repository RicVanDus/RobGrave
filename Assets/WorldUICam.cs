using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldUICam : MonoBehaviour
{
    [SerializeField] private Camera _mainCam;

    private Camera _uicam;

    private void Awake()
    {
        _uicam = GetComponent<Camera>();
    }

    void Update()
    {
        _uicam.focalLength = _mainCam.focalLength;
    }
}
