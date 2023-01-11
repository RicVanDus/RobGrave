using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public class FX_Lightning : MonoBehaviour
{
    private Light _light;
    private BoxCollider _collider;

    private float _lightningTimer = 0f;
    private float  _lightningTime = 5f;
    

    private void Awake()
    { 
        _light = transform.GetComponent<Light>();
        _collider = transform.GetComponent<BoxCollider>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _collider.enabled = false; 
        _light.enabled = false;
        _lightningTime = GameManager.Instance.thisLevel.timeBetweenLightning;
    }

    // Update is called once per frame
    void Update()
    {
        _lightningTimer += Time.deltaTime;
        

        if (_lightningTimer >= _lightningTime)
        {
            StartCoroutine(ShowLightning(5));
            _lightningTimer = 0f;
        }
        
        //Debug.Log("Lightning Timer: " + _lightningTimer + " / " + _lightningTime);
    }



    private IEnumerator ShowLightning(int maxFreq)
    {
        int _freq = Random.Range(2, maxFreq);
        
        
        if (_freq > 0)
        {
            // lightning from random location around the graveyard
            _light.gameObject.transform.rotation = Quaternion.Euler(Random.Range(20f, 50f), Random.Range(0f, 360f), 0f );

            for (int i = 0; i < _freq; i++)
            {
                _collider.enabled = true;
                _collider.enabled = false;
                _collider.enabled = true;
                _light.enabled = true;
                _light.intensity = Random.Range(0.1f, 0.6f);
                
                yield return new WaitForSeconds(Random.Range(0.03f, 0.25f));

                _light.enabled = false;
                
                yield return new WaitForSeconds(Random.Range(0.1f, 0.45f));
            }

            _collider.enabled = false;

            _lightningTimer = Random.Range(0, _lightningTimer/3);
        }
    }
}
