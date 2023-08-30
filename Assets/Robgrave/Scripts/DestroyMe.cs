using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMe : MonoBehaviour
{
    public float destroyMeAfterSeconds;

    private float _timer;

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer > destroyMeAfterSeconds)
        {
            Destroy(gameObject);
        }
    }
}
