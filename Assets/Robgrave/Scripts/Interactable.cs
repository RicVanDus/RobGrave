using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    protected BoxCollider[] colliders = new BoxCollider[5];
    protected BoxCollider trigger;

    public bool PlayerCanInteract = false;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        // Only for checking if there's a trigger: we can get rid of this
        colliders = GetComponents<BoxCollider>();

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].isTrigger)
            {
                trigger = colliders[i];
                break;
            }
        }

        if (trigger == null)
        {
            Debug.LogError("No trigger found on Interactable GameObject: " + gameObject.name);
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //create function
        if (other.tag == "Player")
        {
            PlayerCanInteract = true;            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerCanInteract = false;            
        }
    }
}
