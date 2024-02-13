using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

// TODO: make a traffic light - green is open and safe, yellow is enemy nearby at the exit, red is closed
// Set a random crypt every time you teleport
// Make every crypt look at enemy distance and set safe bool


public class Crypt : MonoBehaviour
{
    public Crypt[] linkedCrypts;
    [SerializeField] public Transform _spawnPoint;
    [SerializeField] public Transform _moveTo;
    [SerializeField] private GameObject _blocker;
    [SerializeField] private GameObject _lock;
    [SerializeField] private GameObject _lightcone;
    [SerializeField] private GameObject _doorR;
    [SerializeField] private GameObject _doorL;
    [SerializeField] private ParticleSystem _fire1;
    [SerializeField] private ParticleSystem _fire2;
    [SerializeField] private Light _fire1Light;
    [SerializeField] private Light _fire2Light;

    private float _safeDistance = 12f;
    private bool _checkEnemiesDistance = false;
    private bool _settingLight = true;
    private WaitForSeconds _waitNME = new(0.5f);
    private WaitForSeconds _wait01 = new(0.1f);
    

    private ParticleSystem.MainModule _fire1Main;
    private ParticleSystem.MainModule _fire2Main;

    private Crypt _linkedCrypt;
    

    public bool safe = false; 

    private void OnEnable()
    {
        GameManager.Instance.cryptKeyObtained += OpenCrypt;
    }

    private void OnDisable()
    {
        GameManager.Instance.cryptKeyObtained -= OpenCrypt;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !PlayerController.Instance.movementDisabled)
        {
            TeleportPlayer();
        }
    }

    private void Start()
    {
        _fire1Main = _fire1.main;
        _fire2Main = _fire2.main;
        _lightcone.SetActive(false);
        SetTeleportCrypt();
        CloseCrypt();
    }

    private void TeleportPlayer()
    {
        if (_linkedCrypt != null)
        {
            PlayerController.Instance.CryptTeleport(_linkedCrypt);
            SetTeleportCrypt();
        }
    }

    private void SetTeleportCrypt()
    {
        if (linkedCrypts.Length > 0)
        {
            int randomIndex = Random.Range(0, linkedCrypts.Length);

            _linkedCrypt = linkedCrypts[randomIndex];
        }
        else
        {
            Debug.Log("No linked crypts to teleport to!");
        }
    }

    private void OpenCrypt()
    {
        _checkEnemiesDistance = true;
        StartCoroutine(CheckEnemyDistance());
        StartCoroutine(SetLightColor());
        
        _blocker.SetActive(false);
        _lock.SetActive(false);
        _doorL.transform.localEulerAngles = new Vector3(0f,-130f,0f);
        _doorR.transform.localEulerAngles = new Vector3(0f,-20f,0f);
        _lightcone.SetActive(true);
    }

    private void CloseCrypt()
    {
        _checkEnemiesDistance = false;
        _fire1Main.startColor = Color.red;
        _fire2Main.startColor = Color.red;
        _fire1Light.color = Color.red;
        _fire2Light.color = Color.red;
        StopCoroutine(CheckEnemyDistance());
        StopCoroutine(SetLightColor());
    }

    private IEnumerator CheckEnemyDistance()
    {
        do
        {
            safe = true;
            var enemies = EnemyManager.Instance.enemies;
            
            for (int i = 0; i < enemies.Count; i++)
            {
                float dist = Vector3.Distance(_spawnPoint.position, enemies[i].transform.position);
                Debug.Log("Enemy" + i +  " -> " + enemies[i].gameObject + " Distance: " + dist);

                if (dist < _safeDistance)
                {
                    safe = false;  
                    break;
                }
            }

            yield return _waitNME;
        } while (_checkEnemiesDistance);
    }

    private IEnumerator SetLightColor()
    {
        do
        {
            if (_linkedCrypt != null)
            {
                if (_linkedCrypt.safe)
                {
                    _fire1Main.startColor = Color.green;
                    _fire2Main.startColor = Color.green;
                    _fire1Light.color = Color.green;
                    _fire2Light.color = Color.green;
                }
                else
                {
                    _fire1Main.startColor = Color.yellow;
                    _fire2Main.startColor = Color.yellow;
                    _fire1Light.color = Color.yellow;
                    _fire2Light.color = Color.yellow;
                }
            }
            
            yield return _wait01;
        } while (_settingLight);


    }
}
