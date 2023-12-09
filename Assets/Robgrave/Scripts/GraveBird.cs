using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class GraveBird : MonoBehaviour
{
    // needs to move to next target when current target is either dug/(being) defiled.
    // when crypt key grave is dug = flies out of frame
    // when grave 2 is looted then flies directly to grave 3
    
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
    private Animator _anim;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
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
    
    // that's cute. BUT ITS WRONG
    private void GoToTarget(Transform target)
    {
        _target = target.position;
        _anim.SetBool("TakingOff", true);
        _birdIsFlying = true;

        Vector3 dir = transform.position - _target;
        Quaternion rot = Quaternion.LookRotation(dir);

        Vector3 newDir = rot.eulerAngles;
        
        newDir.x = transform.eulerAngles.x;
        newDir.z = transform.eulerAngles.z;
        
        transform.DORotate(newDir, 0.7f);
    }

    private void Flying()
    {
        Vector3 posDif;
        Vector3 _lastPosition;
        _lastPosition = transform.position;
        
        transform.position =
            Vector3.SmoothDamp(transform.position, _target, ref _velocity, _smoothing, _speed, Time.deltaTime);

        if (Vector3.Distance(transform.position, _target) < 0.1f)
        {
            _birdIsFlying = false;
            _anim.SetBool("TakingOff", false);
        }

        //calculate velocity?
        posDif = _lastPosition - transform.position;
        var velocity = posDif.magnitude;
        
        _anim.SetFloat("Velocity", velocity);
        Debug.Log("SPEED bird: " + velocity);
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