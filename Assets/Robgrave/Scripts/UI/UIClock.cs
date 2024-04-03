using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIClock : MonoBehaviour
{

    private WaitForSeconds _wait1s = new(1f);

    [SerializeField] private GameObject _hours;
    [SerializeField] private GameObject _minutes;
    [SerializeField] private GameObject _seconds;
    [SerializeField] private Light _pointlight;
    [SerializeField] private Image _fillCircle;

    private bool _running;

    void Start()
    {
        _running = true;
        StartCoroutine(MoveClock());
    }

    private IEnumerator MoveClock()
    {
        float totalSeconds = 60f * 60f;
        float currentSeconds = 0f;
        float fill = 0f;

        var startM = GameManager.Instance.thisLevel.startMinutes;
        var startS = GameManager.Instance.thisLevel.startSeconds;

        float startSeconds = startS + 60f * startM;
        totalSeconds -= startSeconds;
        
        Color redColor = Color.red;
        Color yellowColor = Color.yellow;
        
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

            currentSeconds = s + 60f * m - startSeconds;
            fill = Mathf.Clamp(currentSeconds / totalSeconds, 0f, 1f);

            // 0:00 = RED
            if (h == 12)
            {
                fill = 1f;
            }
            
            _fillCircle.fillAmount = fill;
            _fillCircle.color = Color.Lerp(yellowColor, redColor, fill);

            yield return _wait1s;
        } while (_running);
    }
}
