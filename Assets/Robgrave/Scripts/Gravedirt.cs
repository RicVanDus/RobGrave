using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravedirt : MonoBehaviour
{

    Material graveDirt;
    float _buldgeHeight;

    // Start is called before the first frame update
    void Start()
    {
        graveDirt = gameObject.GetComponent<Renderer>().material;
        _buldgeHeight = 30f;
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    public void DirtHeight(int height)
    {
        switch (height)
        {
            case 0:
                break;
            case 1:
                _buldgeHeight = 0f;
                break;
            case 2:
                _buldgeHeight = -20f;
                break;
            case 3:
                _buldgeHeight = -45f;
                break;
            case 4:
                _buldgeHeight = -70f;
                break;

        }


    }

}
