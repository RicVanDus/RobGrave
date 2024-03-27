using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class CamControl : MonoBehaviour
{
    
    public Transform target;
    public float camSpeed = 10.0f;
    
    
    [SerializeField] private Transform CameraBounds;

    private Vector2 CamBoundsX;
    private Vector2 CamBoundsY;

    private bool _isShaking = false;

    private Camera _cam;
    private float _defaultFov;

    private void Start()
    {
        _cam = GetComponentInChildren<Camera>();
        _defaultFov = _cam.fieldOfView;
        
        CamBoundsX.x = (CameraBounds.transform.localScale.x / 2) + CameraBounds.transform.position.x;
        CamBoundsX.y = (CameraBounds.transform.localScale.x / 2 * -1) + CameraBounds.transform.position.x;

        CamBoundsY.x = (CameraBounds.transform.localScale.z / 2) + CameraBounds.transform.position.z;
        CamBoundsY.y = (CameraBounds.transform.localScale.z / 2 * -1) + CameraBounds.transform.position.z;
    }

    void Update()
    {
        if (!_isShaking)
        {
            Vector3 newPosition = Vector3.Lerp(transform.position, target.position, Time.deltaTime * camSpeed);

            newPosition.x = Mathf.Clamp(newPosition.x, CamBoundsX.y, CamBoundsX.x);
            newPosition.z = Mathf.Clamp(newPosition.z, CamBoundsY.y, CamBoundsY.x);
        
            transform.position = newPosition;    
        }
    }

    public void CamShake(float duration, float strength)
    {
        if (!_isShaking)
        {
            _isShaking = true;
            StartCoroutine(ShakingCamera(duration, strength));
        }
    }

    private IEnumerator ShakingCamera(float duration, float strength)
    {
        float shakeTime = 0f;
        Vector3 newPos = new Vector3();
        Vector3 oldPos = transform.position;
        
        do
        {
            transform.position = oldPos;
            
            shakeTime += Time.deltaTime;

            float shakeRange = strength * (1 - (shakeTime / duration));

            newPos.z = Random.Range(-shakeRange, shakeRange);
            newPos.x = Random.Range(-shakeRange, shakeRange);

            transform.position += newPos;
            
            yield return null;
        } while (shakeTime < duration);

        _isShaking = false;
    }

    public void CameraZoom(float fov, float duration)
    {
        _cam.DOFieldOfView(fov, duration).SetEase(Ease.InSine);
    }

    public void ResetCameraZoom(float duration)
    {
        _cam.DOFieldOfView(_defaultFov, duration).SetEase(Ease.OutSine);
    }
}
