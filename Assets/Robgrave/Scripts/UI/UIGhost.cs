using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

// do all ghost type shit here, check 

public class UIGhost : MonoBehaviour
{
    [SerializeField] private GameObject _mesh;
    [SerializeField] private GameObject _progress;
    [SerializeField] private Light _pointLight;
    
    private Material _ghostMat;
    private int _baseColorId;

    public int _ghostType = 999;
    public Enemy enemy;

    private readonly float _progressScaleMinY = 0f;
    private readonly float _progressScaleMaxY = 1.35f;
    private readonly float _progressPosMinY = 0.68f;
    private readonly float _progressPosMaxY = 0f;

    private bool _enemyIsSet = false;

    private WaitForSeconds _wait02 = new(0.2f);

    private void Awake()
    {
        _ghostMat = _mesh.GetComponent<Renderer>().materials[1];
        _baseColorId = Shader.PropertyToID("_BaseColor");
    }

    private void Start()
    {
        StartCoroutine(SoftTick());
    }

    private IEnumerator SoftTick()
    {
        do
        {
            if (ChangedGhostType())
            {
                ChangeGhostType();
            }
            
            UpdateProgressBar();

            yield return _wait02;
        } while (_enemyIsSet);
    }

    private bool ChangedGhostType()
    {
        if (enemy.ghostType == _ghostType)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void SetGhost(Enemy nme)
    {
        enemy = nme;
        _enemyIsSet = true;
    }

    private void ChangeGhostType()
    {
        // animate and change color
        _ghostType = enemy.ghostType;

        Vector3 defaultRotation = transform.eulerAngles;
        Vector3 newRotation = defaultRotation;
        newRotation.y += 180f;

        Sequence seq = DOTween.Sequence();

        seq.SetDelay(1f);
        seq.Delay();
        seq.Append(transform.DORotate(newRotation, 0.7f).SetEase(Ease.InBounce).OnComplete(() =>
        {
            newRotation.y += 180f;
            ChangeGhostColor();
            transform.DORotate(newRotation, 0.7f).SetEase(Ease.OutBounce);
        }));

        seq.Play();
    }

    private void ChangeGhostColor()
    {
        Color newColor = Color.black;
        float lightIntensity = 0.17f;

        switch (_ghostType)
        {
            case 0 :
                newColor = EnemyManager.Instance.GhostType1;
                break;
            case 1 :
                newColor = EnemyManager.Instance.GhostType2;
                lightIntensity = 0.34f;
                break;
            case 2 :
                newColor = EnemyManager.Instance.GhostType3;
                break;
            case 3 :
                newColor = EnemyManager.Instance.GhostType4;
                break;
        }
        
        _ghostMat.SetColor(_baseColorId, newColor);
        _pointLight.color = newColor;
        _pointLight.intensity = lightIntensity;
    }

    private void UpdateProgressBar()
    {
        Vector3 Pos = _progress.transform.localPosition;
        Vector3 Scale = _progress.transform.localScale;

        float nmeProgress = enemy.ghostEvolveProgress;

        float yScale = Mathf.Lerp(_progressScaleMinY, _progressScaleMaxY, nmeProgress);
        float yPos = Mathf.Lerp(_progressPosMinY, _progressPosMaxY, nmeProgress);

        Pos.y = yPos;
        Scale.y = yScale;

        _progress.transform.localPosition = Pos;
        _progress.transform.localScale = Scale;
    }
}