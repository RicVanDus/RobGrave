using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public int id;
    public bool AutoPickup = true;
    public int value;
    public string objectName;
    [Header("Light")]
    [SerializeField] private GameObject _pointLight;
    [SerializeField] private float _lightHeight;
    [SerializeField] private GameObject _pickupFX;
    
    private Vector3 _lightPos;
    private Rigidbody _rigidbody;
    private bool _physicsEnabled = true;
    private float physicsDisableTimer = 0f;
    private float physicsDisableTime = 2f;

    private bool _noLight;
    private bool _visible = false;

    void Start()
    {
        this.gameObject.GetComponent<MeshRenderer>().enabled = false;
        _rigidbody = gameObject.GetComponent<Rigidbody>();

        if (_pointLight == null)
        {
            _noLight = true;
        }
    }

    void Update()
    {
        if (!_visible && transform.position.y < 3f)
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
        
        _lightPos = new Vector3(0f, _lightHeight, 0f);

        if (!_noLight)
        {
            PositionPointLight();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerController.Instance.AddScore(value);
            LootManager.Instance.ClearLootSpot(id);
            Instantiate(_pickupFX, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
            
        }
        else if (other.tag == "Enemy")
        {
            Enemy _enemy = other.GetComponent<Enemy>();
            _enemy.PickUpValuable(value);
            LootManager.Instance.ClearLootSpot(id);
            Instantiate(_pickupFX, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    private void PositionPointLight()
    {
        _pointLight.transform.position = transform.position + _lightPos;
    }
}