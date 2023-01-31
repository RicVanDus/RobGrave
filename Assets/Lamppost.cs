using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamppost : MonoBehaviour
{

    [SerializeField] private GameObject lamp1;
    [SerializeField] private GameObject lamp2;
    [SerializeField] private Light pointLight;

    private Material _lamp1Mat;
    private Material _lamp2Mat;

    // Start is called before the first frame update
    void Start()
    {
        _lamp1Mat = lamp1.GetComponent<Renderer>().material;
        _lamp2Mat = lamp2.GetComponent<Renderer>().material;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
