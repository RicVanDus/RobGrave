using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValuablePickUpFX : MonoBehaviour
{
    private ParticleSystem _particles;
    private float _timer;

    private void Awake()
    {
        _particles = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer > 2f)
        {
            Destroy(gameObject);
        }
    }
}
