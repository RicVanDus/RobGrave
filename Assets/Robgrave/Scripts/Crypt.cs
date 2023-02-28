using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Crypt : MonoBehaviour
{
    public Crypt[] linkedCrypts;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Transform _moveTo;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
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
            PlayerController.Instance.TeleportTo(randomCrypt._spawnPoint);
        }
        else
        {
            Debug.Log("No linked crypts to teleport to!");
        }
        
    }
}
