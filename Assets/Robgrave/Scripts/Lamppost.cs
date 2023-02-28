using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamppost : MonoBehaviour
{

    [SerializeField] private GameObject _lamp1;
    [SerializeField] private GameObject _lamp2;
    [SerializeField] private Light _spotLight;
    
    private Material _lamp1Mat;
    private Material _lamp2Mat;
    private SphereCollider _collider;

    private float _lightTimer;
    private float _maxLightTime = 20f;

    // Start is called before the first frame update
    void Start()
    {
        _lamp1Mat = _lamp1.GetComponent<Renderer>().material;
        _lamp2Mat = _lamp2.GetComponent<Renderer>().material;
        _collider = GetComponent<SphereCollider>();
    }

    private void Update()
    {
        _lightTimer = Mathf.Clamp(_lightTimer - Time.deltaTime, 0f, _maxLightTime);
    }

    private void ToggleLight(bool on)
    {
        
    }
    
    
}
