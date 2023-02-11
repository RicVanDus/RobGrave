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
    private Vector3 _lightPos;

    private bool _noLight;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<MeshRenderer>().enabled = false;

        if (_pointLight == null)
        {
            _noLight = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.transform.position.y < 2f)
        {
            this.gameObject.GetComponent<MeshRenderer>().enabled = true;
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
            Destroy(this.gameObject);
        }
        else if (other.tag == "Enemy")
        {
            Enemy _enemy = other.GetComponent<Enemy>();
            _enemy.PickUpValuable(value);
            LootManager.Instance.ClearLootSpot(id);
            Destroy(this.gameObject);
        }
    }

    private void PositionPointLight()
    {
        _pointLight.transform.position = transform.position + _lightPos;
    }
}