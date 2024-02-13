using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireLight : MonoBehaviour
{
    private Light _pointLight;
    private WaitForSeconds _wait01 = new (0.1f);
    private bool _flicker = true;
    private float _baseIntensity;

    void Awake()
    {
        _pointLight = GetComponent<Light>();
    }
    
    void Start()
    {
        _baseIntensity = _pointLight.intensity;
        StartCoroutine(FlickeringLight());
    }

    private IEnumerator FlickeringLight()
    {
        float randomMult = 1f;
        float newIntensity = 0f;
        
        do
        {
            randomMult = Random.Range(0.5f, 2f);

            newIntensity = _baseIntensity * randomMult;

            _pointLight.intensity = newIntensity;
            
            yield return _wait01;
        } while (_flicker);
    }
}