using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupUIChest : MonoBehaviour
{
    [SerializeField] private Material _green;
    [SerializeField] private Material _blue;
    [SerializeField] private Material _purple;
    [SerializeField] private GameObject _lid;
    [SerializeField] private GameObject _base;
    
    public void SetChest(int type)
    {
        Material newMat;

        Renderer rendererBase = _base.GetComponent<Renderer>();
        Renderer rendererLid = _lid.GetComponent<Renderer>();
        
        // SET MATERIAL
        if (type == 0)
        {
            newMat = _green;
        } else if (type == 1)
        {
            newMat = _blue;
        }
        else
        {
            newMat = _purple;
        }

        Material[] matsBase = rendererBase.materials;
        Material[] matsLid = rendererLid.materials;

        matsBase[1] = newMat;
        matsLid[0] = newMat;

        rendererBase.materials = matsBase;
        rendererLid.materials = matsLid;

        /* play animation */
    }
}
