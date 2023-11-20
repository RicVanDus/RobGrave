using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftBox : MonoBehaviour
{
    public GiftBoxTemplate giftBoxData;

    private Material _material;
    public int lootId;
    private Rigidbody _rigidbody;

    private bool _physicsEnabled = true;
    private float physicsDisableTimer = 0f;
    private float physicsDisableTime = 3f;
    
    private bool _visible = false;
    
    // Start is called before the first frame update
    void Start()
    {
        _material = GetComponent<Renderer>().material;
        this.gameObject.GetComponent<MeshRenderer>().enabled = false;
        _rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_visible && transform.position.y < 3f && giftBoxData.giftType != GiftType.CryptKey)
        {
            this.gameObject.GetComponent<MeshRenderer>().enabled = true;
            _visible = true;
        }
        
        if (_visible) {
            physicsDisableTimer += Time.deltaTime;
            if (physicsDisableTimer > physicsDisableTime && _physicsEnabled)
            {
                _physicsEnabled = false;
                _rigidbody.freezeRotation = true;
                _rigidbody.useGravity = false;
                _rigidbody.isKinematic = true;
            }
        }
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
                if (giftBoxData.giftType == GiftType.CryptKey)
                {
                    GameManager.Instance.cryptKeyObtained?.Invoke();
                    Destroy(gameObject);
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
}
