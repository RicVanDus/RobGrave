using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UIHeartBlock : MonoBehaviour
{
    
    [SerializeField] private GameObject _heartFill;
    
    public bool _isOn;

    private void Awake()
    {
        _isOn = false;
        _heartFill.SetActive(false);
    }

    public void TurnOn()
    {
        if (_isOn) return;
        
        Sequence seq = DOTween.Sequence();
        
        Vector3 defaultRotation = transform.eulerAngles;
        Vector3 newRotation = defaultRotation;
        newRotation.y += 180f;
        
        seq.SetDelay(1f);
        seq.Delay();
        seq.Append(transform.DORotate(newRotation, 0.7f).SetEase(Ease.InBounce).OnComplete(() =>
        {
            _heartFill.SetActive(true);
            newRotation.y += 180f;
            _isOn = true;
            transform.DORotate(newRotation, 0.7f).SetEase(Ease.OutBounce);
        }));

        seq.Play();
    }
    
    public void TurnOff()
    {
        if (!_isOn) return;
        
        Vector3 defaultRotation = transform.eulerAngles;
        Vector3 newRotation = defaultRotation;
        newRotation.y += 180f;
        
        Sequence seq = DOTween.Sequence();
        
        seq.SetDelay(1f);
        seq.Delay();
        seq.Append(transform.DORotate(newRotation, 0.7f).SetEase(Ease.InBounce).OnComplete(() =>
        {
            _heartFill.SetActive(false);
            newRotation.y += 180f;
            _isOn = false;
            transform.DORotate(newRotation, 0.7f).SetEase(Ease.OutBounce);
        }));

        seq.Play();
    }
}
