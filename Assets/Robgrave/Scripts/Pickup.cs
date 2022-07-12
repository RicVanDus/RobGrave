using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public int id;
    public bool AutoPickup = true;
    public int value;
    public string objectName;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerController.Instance.AddScore(value);
        }
        LootManager.Instance.ClearLootSpot(id);
        Destroy(this.gameObject);
    }
}