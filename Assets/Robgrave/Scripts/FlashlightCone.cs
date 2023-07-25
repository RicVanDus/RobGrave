using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightCone : MonoBehaviour
{
    
    private void OnTriggerStay(Collider other)
    {
        var nme = other.GetComponent<Enemy>();
        nme.CaughtInLight();
        PlayerController.Instance.FlashlightColor(nme.ghostType);
        
        Debug.Log("Flashlight hitting: " + other);
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerController.Instance.FlashlightColor(5);
    }
}
