using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraveBird : MonoBehaviour
{
    // needs to move to next target when current target is either dug/(being) defiled.
    // when crypt key grave is dug = flies out of frame
    // when grave 2 is looted then flies directly to grave 3
    
    private Rigidbody _rigidbody;
    [SerializeField] private float _speed;
    private int _currentTarget = 0; // out of 3
    public Grave[] birdGraves;

    private bool _startingPosSet = false;
    private bool _birdIsDone = false;
    private bool _birdIsFlying = false;

    private Vector3 _target;
    
    private WaitForSeconds _softTick = new (0.5f);

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (_birdIsFlying)
        {
            
        }
    }

    public void SetBirdStartingPosition()
    {
        transform.position = birdGraves[0].birdPos.position;
        transform.rotation = birdGraves[0].birdPos.rotation;
    }
    
    private void GoToTarget(Transform target)
    {
        _target = target.position;
        _birdIsFlying = true;
    }

    private void Flying()
    {
        // flap flap flap
    } 
    
    // coroutine checks for changes in current target & changes in last target. 
    // if changed check for next target state and then fly there
    private IEnumerator CheckGraveStatus()
    {
        do
        {
            if (birdGraves[_currentTarget].graveIsDug || birdGraves[_currentTarget].defileProgress > 0f)
            {
                
            }
            
        } while (!_birdIsDone);

        yield return _softTick;
    }
    
}