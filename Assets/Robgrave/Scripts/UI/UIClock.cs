using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIClock : MonoBehaviour
{

    private WaitForSeconds _wait1s = new(1f);

    [SerializeField] private GameObject _hours;
    [SerializeField] private GameObject _minutes;
    [SerializeField] private GameObject _seconds;
    [SerializeField] private Light _pointlight;

    private bool _running;

    void Start()
    {
        _running = true;
        StartCoroutine(MoveClock());
    }

    private IEnumerator MoveClock()
    {
        do
        {
            int h = GameManager.Instance.gameTimeHours;
            int m = GameManager.Instance.gameTimeMinutes;
            float s = GameManager.Instance.gameTimeSeconds;

            Vector3 sRotate = _seconds.transform.localEulerAngles;
            Vector3 mRotate = _minutes.transform.localEulerAngles;
            Vector3 hRotate = _hours.transform.localEulerAngles;
            
            sRotate.y = 6f * s;
            mRotate.y = 6f * m;
            hRotate.y = 30f * h + 0.5f * m;

            _seconds.transform.localEulerAngles = sRotate;
            _minutes.transform.localEulerAngles = mRotate;
            _hours.transform.localEulerAngles = hRotate;

            yield return _wait1s;
        } while (_running);
    }
}
