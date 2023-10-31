using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteAlways]
public class FixRotation : MonoBehaviour
{
    
    [SerializeField] private int _degrees;
    
    public void SnapRotation()
    {
        if (_degrees == 0) return;

        var children = transform.GetComponentsInChildren<Transform>();
        
        foreach (Transform child in children)
        {
            float _rotAmountX = child.eulerAngles.x / _degrees;
            float _rotAmountY = child.eulerAngles.y / _degrees;
            float _rotAmountZ = child.eulerAngles.z / _degrees;
            

            _rotAmountX = MathF.Round(_rotAmountX) * _degrees;
            _rotAmountY = MathF.Round(_rotAmountY) * _degrees;
            _rotAmountZ = MathF.Round(_rotAmountZ) * _degrees;

            Vector3 _newRotation = new Vector3(_rotAmountX, _rotAmountY, _rotAmountZ);

            child.eulerAngles = _newRotation;
        }
    }
}

[ExecuteAlways]

[CustomEditor(typeof(FixRotation))]
public class FixRotationEditor : Editor
{

    
    
    override public void  OnInspectorGUI ()
    {
        FixRotation fixRotation = (FixRotation)target;
        if(GUILayout.Button("Snap Rotation"))
        {
            fixRotation.SnapRotation();
        }
        DrawDefaultInspector();
    }
}

