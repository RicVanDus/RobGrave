using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entrance : MonoBehaviour
{
    [SerializeField] private GameObject _gate1;
    [SerializeField] private GameObject _gate2;
    [SerializeField] private GameObject _blocker;
    [SerializeField] private Light _fireLight1;
    [SerializeField] private Light _fireLight2;
    [SerializeField] private ParticleSystem _fire1;
    [SerializeField] private ParticleSystem _fire2;

    private ParticleSystem.MainModule _fire1Main;
    private ParticleSystem.MainModule _fire2Main;

    private float _opening = 0f;


    private void Awake()
    {
        _fire1Main = _fire1.main;
        _fire2Main = _fire2.main;
    }

    void Start()
    {
        GateClosed();
    }

    void Update()
    {
        
    }

    private void GateOpen()
    {
        _fire1Main.startColor = Color.green;
        _fire2Main.startColor = Color.green;
        _fireLight1.color = Color.green;
        _fireLight2.color = Color.green;
    }

    // ever needing this?
    private void GateClosed()
    {
        _fire1Main.startColor = Color.red;
        _fire2Main.startColor = Color.red;
        _fireLight1.color = Color.red;
        _fireLight2.color = Color.red;
    }

    
}
