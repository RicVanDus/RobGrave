using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class GiftBox : MonoBehaviour
{
    public GiftBoxTemplate giftBoxData;

    [SerializeField] private GameObject _chestMesh;

    private Material _material;
    public int lootId;
    private Rigidbody _rigidbody;

    private bool _physicsEnabled = true;
    private float physicsDisableTimer = 0f;
    private float physicsDisableTime = 2f;
    
    private bool _visible = false;
    public int boxType = 0;

    private Camera _uiCam;

    private bool _iconVisible;
    private Vector3 _iconRot;

    private void Awake()
    {
        _iconRot = new Vector3(90f, 0f, 0f);
    }

    void Start()
    {
        _material = GetComponent<Renderer>().material;
        this.gameObject.GetComponent<MeshRenderer>().enabled = false;
        _rigidbody = gameObject.GetComponent<Rigidbody>();
        _chestMesh.SetActive(false);
        _uiCam = LootManager.Instance.UICam;
    }

    void Update()
    {
        if (!_visible && transform.position.y < 2f)
        {
            this.gameObject.GetComponent<MeshRenderer>().enabled = true;
            _visible = true;
            _chestMesh.SetActive(true);
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
            if (giftBoxData.giftType == GiftType.CryptKey)
            {
                GameManager.Instance.cryptKeyObtained?.Invoke();
                UIMessages.Instance.CreateMessage("The crypt is open!", "A shortcut to another place", UIMessageType.Good ,MessageIcon.CryptKey);
                Destroy(gameObject);
            }
            else
            {
                LootManager.Instance.ClearLootSpot(lootId);
                Destroy(gameObject);
                PowerupManager.Instance.ChestOpen(boxType);
            }
        }
    }
}
