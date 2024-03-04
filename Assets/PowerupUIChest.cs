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
        /*
        if (type == 0)
        {
            _lid.GetComponent<Renderer>().material = _green;
            _base.GetComponent<Renderer>().material = _green;
        } else if (type == 1)
        {
            _lid.GetComponent<Renderer>().material = _blue;
            _base.GetComponent<Renderer>().material = _blue;            
        }
        else
        {
            _lid.GetComponent<Renderer>().material = _purple;
            _base.GetComponent<Renderer>().material = _purple;            
        } */
    }
    
}
