using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightCone : MonoBehaviour
{
    
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            var nme = other.GetComponent<Enemy>();
            PlayerController.Instance.FlashlightColor(nme.ghostType);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerController.Instance.FlashlightColor(5);
    }
}
