using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GraveBird : MonoBehaviour
{
    // needs to move to next target when current target is either dug/(being) defiled.
    // when crypt key grave is dug = flies out of frame
    // when grave 2 is looted then flies directly to grave 3
    
    private Rigidbody _rigidbody;
    [SerializeField] private float _speed = 0.1f;
    [SerializeField] private float _smoothing = 1f;
    private int _currentTarget = 0; // out of 3
    public Grave[] birdGraves;

    private bool _startingPosSet = false;
    private bool _birdIsDone = false;
    private bool _birdIsFlying = false;

    private Vector3 _target;
    private Vector3 _velocity;
    
    private WaitForSeconds _softTick = new (0.5f);

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (_birdIsFlying)
        {
            Flying();
        }
    }

    public void SetBirdStartingPosition()
    {
        transform.position = birdGraves[0].birdPos.position;
        transform.rotation = birdGraves[0].birdPos.rotation;

        StartCoroutine(CheckGraveStatus());
    }
    
    private void GoToTarget(Transform target)
    {
        _target = target.position;
        _birdIsFlying = true;
    }

    private void Flying()
    {
        transform.position =
            Vector3.SmoothDamp(transform.position, _target, ref _velocity, _smoothing, _speed, Time.deltaTime);

        if (Vector3.Distance(transform.position, _target) < 0.1f)
        {
            _birdIsFlying = false;
        }
        
        //rotate towards goal, flying animation started with speed influenced by _velocity.magnitude

    } 
    
    
    
    
    
    // coroutine checks for changes in current target & changes in last target. 
    // if changed check for next target state and then fly there
    private IEnumerator CheckGraveStatus()
    {
        do
        {
            if (_currentTarget < 3)
            {
                if (birdGraves[_currentTarget].graveIsDug || birdGraves[_currentTarget].defileProgress > 0f)
                {
                    _currentTarget++;
                    GoToTarget(birdGraves[_currentTarget].birdPos);
                }
            }
            else
            {
                // flying out of frame? Or into a tree (Random pos)?
                transform.gameObject.SetActive(false);
            }
            yield return _softTick;
        } while (!_birdIsDone);
    }
}