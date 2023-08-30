using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Crypt : MonoBehaviour
{
    public Crypt[] linkedCrypts;
    [SerializeField] public Transform _spawnPoint;
    [SerializeField] public Transform _moveTo;
    [SerializeField] private GameObject _blocker;
    [SerializeField] private GameObject _lock;


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

    private void TeleportPlayer()
    {
        if (linkedCrypts.Length > 0)
        {
            int randomIndex = Random.Range(0, linkedCrypts.Length);

            var randomCrypt = linkedCrypts[randomIndex];
            PlayerController.Instance.CryptTeleport(randomCrypt);
        }
        else
        {
            Debug.Log("No linked crypts to teleport to!");
        }
        
    }

    private void OpenCrypt()
    {
        _blocker.SetActive(false);
        _lock.SetActive(false);
    }
}
