using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGhost : MonoBehaviour
{
    [SerializeField] private GameObject _mesh;
    [SerializeField] private GameObject _progress;
    [SerializeField] private Light _pointLight;
    
    private Material _ghostMat;
    private int _baseColorId;

    private readonly float _progressScaleMinY = 0.1f;
    private readonly float _progressScaleMaxY = 1.35f;
    private readonly float _progressPosMinY = 0.68f;
    private readonly float _progressPosMaxY = 1f;

    private void Awake()
    {
        _ghostMat = _mesh.GetComponent<Renderer>().materials[1];
        _baseColorId = Shader.PropertyToID("_BaseColor");
    }
    
    //create methods for making progress bar fill and create method for changing into new form (when rotated backwards: change color of light and mat)
    // 
    
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
