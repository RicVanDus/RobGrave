using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScore : MonoBehaviour
{
    [SerializeField] private GameObject _nrRotator1;
    [SerializeField] private GameObject _nrRotator2;
    [SerializeField] private GameObject _nrRotator3;
    [SerializeField] private GameObject _nrRotator4;
    [SerializeField] private GameObject _nrRotator5;
    
    
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    
    public void UpdateScore(int scoreAdded)
    {
        if (scoreAdded > 0)
        {
            
        }
        else if (scoreAdded < 0)
        {
            
        }
        else
        {
            return;
        }
    }
}
