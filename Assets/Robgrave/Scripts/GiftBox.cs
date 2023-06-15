using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftBox : MonoBehaviour
{
    public GiftBoxTemplate giftBoxData;

    private Material _material;
    public int lootId;
    
    // Start is called before the first frame update
    void Start()
    {
        _material = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (giftBoxData.duration > 0f)
            {
                
            }
            else
            {
                giftBoxData.Apply();
                LootManager.Instance.ClearLootSpot(lootId);
                Destroy(gameObject);
            }
        }
    }
}
