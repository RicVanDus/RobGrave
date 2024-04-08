using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core.Easing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class Lamppost : MonoBehaviour
{
    [SerializeField] private GameObject _lamp1;
    [SerializeField] private GameObject _lamp2;
    [SerializeField] private GameObject _lampbase;
    [SerializeField] private GameObject _cone;
    [SerializeField] private Light _spotLight;
    [SerializeField] private Light _softSpotLight;
    [SerializeField] SphereCollider _collider;
    [SerializeField] private GameObject _UI;
    [SerializeField] private GameObject _posIndicator;

    [SerializeField] private Color _highlightColor;
    
    private Material _lamp1Mat;
    private Material _lamp2Mat;
    private Material _lamp1BulbMat;
    private Material _lamp2BulbMat;
    private Material _lampbaseMat;
    private Material _lamp1Glass;
    private Material _lamp2Glass;
    private GUI_lamppost _GUI_lamppost;
    
    private bool _guiVisible = false;
    private bool _isHighlighted = false;
    
    private Vector3 _guiDefaultScale;

    private float _lightTimer;
    private float _maxLightTime = 2f;
    private bool _lightIsOn = true;
    private bool _IdleLightIsOn = false;
    private int _lightStage;
    private int _maxLightStages = 3;

    private bool _playerCanInteract = false;
    private bool _playerIsInteracting = false;
    private bool _lightIsflashing = false;

    private float _baseConeSize;
    private float _baseSpotlightSize;
    private float _baseTriggerSize;
    private bool _playerInteracting = false;

    private int _emissionColorId;

    private Color _bulbColor = new Color(0.75f, 0.7f, 0.17f);

    private WaitForSeconds _wait01 = new (0.1f);
    private WaitForSeconds _wait03 = new (0.3f);

    private Material[] _wiggleMats = new Material[7];
    private int _wiggleDeformId;
    private int _deformDirId;
    private bool _bWiggling;
    
    // Starts a coroutine: sets the dir.vector, loops through a deforming value, which gets halfed each cycle (back/forth)

    private void Awake()
    {
        _emissionColorId = Shader.PropertyToID("_NewEmissionColor");
        _wiggleDeformId = Shader.PropertyToID("_WiggleDeform");
        _deformDirId = Shader.PropertyToID("_DeformDirection");
    }

    void Start()
    {
        _lamp1Mat = _lamp1.GetComponent<Renderer>().materials[0];
        _lamp2Mat = _lamp2.GetComponent<Renderer>().materials[0];
        _lamp1Glass = _lamp1.GetComponent<Renderer>().materials[1];
        _lamp2Glass = _lamp2.GetComponent<Renderer>().materials[1];
        _lamp1BulbMat = _lamp1.GetComponent<Renderer>().materials[2];
        _lamp2BulbMat = _lamp2.GetComponent<Renderer>().materials[2];
        _lampbaseMat = _lampbase.GetComponent<Renderer>().materials[0];
        _GUI_lamppost = _UI.GetComponent<GUI_lamppost>();
        _guiDefaultScale = _UI.transform.localScale;
        
        // filling wiggle array
        _wiggleMats[0] = _lamp1Mat;
        _wiggleMats[1] = _lamp2Mat;
        _wiggleMats[2] = _lamp1Glass;
        _wiggleMats[3] = _lamp2Glass;
        _wiggleMats[4] = _lamp1BulbMat;
        _wiggleMats[5] = _lamp2BulbMat;
        _wiggleMats[6] = _lampbaseMat;

        _baseConeSize = _cone.transform.localScale.x / 5;
        _baseTriggerSize = _collider.radius / 5;
        _baseSpotlightSize = _spotLight.range / 5;
        
        _posIndicator.SetActive(false);
        
        ToggleHighlight(false);
        StartCoroutine(PosIndicator());
    }

    private void Update()
    {
        if (_lightStage > 0 && !_lightIsOn && !_lightIsflashing)
        {
            ToggleLight(true);
        }
        else if (_lightStage == 0 && _lightIsOn && !_lightIsflashing)
        {
            ToggleLight(false);
        } 
        
        if (_playerCanInteract)
        {
            if (PlayerController.Instance._interacting)
            {
                TimerChange(true);
                if (!_playerInteracting)
                {
                    _playerInteracting = true;
                    PlayerController.Instance.targetLamppost = this;
                    PlayerController.Instance.RotateToObject(this.gameObject);
                    PlayerController.Instance.movementDisabled = true;
                    PlayerController.Instance.IsHitting(true);
                }
            }
            else
            {
                if (_playerInteracting)
                {
                    _playerInteracting = false;
                    PlayerController.Instance.movementDisabled = false;  
                    PlayerController.Instance.targetLamppost = null;
                    PlayerController.Instance.IsHitting(false);
                }
                TimerChange(false);
            }

            if (!_guiVisible)
            {
                SetGUIVisible(true);           
            }

            if (!_isHighlighted)
            {
                ToggleHighlight(true);
            }

        }
        else
        {
            TimerChange(false);
            
            if (_isHighlighted)
            {
                ToggleHighlight(false);
            }
        }

        if (_lightStage == 1 && _lightTimer < (_maxLightTime/2) && !_playerIsInteracting)
        {
            if (!_lightIsflashing)
            {
                StartCoroutine(FlashingLight());
            }
        }
        else
        {
            if (_lightIsflashing)
            {
                StopCoroutine(FlashingLight());
                _lightIsflashing = false;
            }
        }

        if (!_playerCanInteract && _lightTimer == 0f)
        {
            if (_guiVisible)
            {
                SetGUIVisible(false);
            }
        }
        else
        {
            _GUI_lamppost.PlayerIsInteracting = _playerIsInteracting;
            _GUI_lamppost.currentLightStages = _lightStage;
            _GUI_lamppost.FillAmount = (_lightTimer % _maxLightTime) / _maxLightTime;
        }
    }

    private void ToggleLight(bool toggle)
    {
        _lightIsOn = toggle;
        _collider.enabled = toggle;
        _spotLight.enabled = toggle;
        _cone.SetActive(toggle);

        if (toggle)
        {
            _lamp1BulbMat.SetColor(_emissionColorId, _bulbColor);
            _lamp2BulbMat.SetColor(_emissionColorId, _bulbColor);
            StopCoroutine(IdleState());
        }
        else
        {
            _lamp1BulbMat.SetColor(_emissionColorId, Color.black);
            _lamp2BulbMat.SetColor(_emissionColorId, Color.black);
            StartCoroutine(IdleState());
        }
    }

    private void ToggleIdleLight(bool toggle)
    {
        _IdleLightIsOn = toggle;
        _softSpotLight.enabled = toggle;

        if (toggle)
        {
            _lamp1BulbMat.SetColor(_emissionColorId, _bulbColor);
            _lamp2BulbMat.SetColor(_emissionColorId, _bulbColor);
        }
        else
        {
            _lamp1BulbMat.SetColor(_emissionColorId, Color.black);
            _lamp2BulbMat.SetColor(_emissionColorId, Color.black);   
        }
    }
    
    
    private IEnumerator FlashingLight()
    {
        _lightIsflashing = true;
        do
        {
            ToggleLight(!_lightIsOn);

            yield return new WaitForSeconds(0.5f);
        } while (_lightIsflashing);
    }

    private void TimerChange(bool add)
    {
        if (add)
        {
            _playerIsInteracting = true;
            
            if (_lightStage < 3)
            {
                _lightTimer += Time.deltaTime * PlayerController.Instance.hitSpeedMult;

                if (_lightStage < (int)(_lightTimer / _maxLightTime))
                {
                    _lightStage = (int)(_lightTimer / _maxLightTime);
                    SetLightSize();
                    //StartWiggle(0.7f);
                }

            }
            else
            {
                _lightTimer += Time.deltaTime * PlayerController.Instance.hitSpeedMult;;
                if (_lightTimer >= _maxLightTime * _maxLightStages)
                {
                    _lightTimer = _maxLightTime * _maxLightStages;
                }
            }
        }
        else
        {
            _playerIsInteracting = false;
            if (_lightStage > 0)
            {
                _lightTimer -= Time.deltaTime/3;
                if (_lightTimer > 0f)
                {
                    if (_lightStage > (int)Mathf.Ceil(_lightTimer / _maxLightTime))
                    {
                        _lightStage = (int)Mathf.Ceil(_lightTimer / _maxLightTime);
                        SetLightSize();
                    }
                }
                else
                {
                    _lightTimer = 0f;
                    _lightStage = 0;
                }
            }
            else
            {
                _lightTimer = 0f;
            }
        }
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerCanInteract = true;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerCanInteract = false;
        }
    }

    private void SetGUIVisible(bool visible)
    {
        if (visible)
        {
            _GUI_lamppost.ShowGraphic = true;
            _UI.transform.localScale = Vector3.zero;

            _UI.transform.DOScale(_guiDefaultScale, 0.5f).SetEase(Ease.OutBounce);
            _guiVisible = true;
        }
        else
        {
            _GUI_lamppost.ShowGraphic = false;
            _guiVisible = false;
        }
    }

    private void SetLightSize()
    {
        int size = _lightStage + 2;
        float triggerSize = size;
        float spotlightSize = _baseSpotlightSize * size;
        float coneSize = _baseConeSize * size;

        _collider.radius = triggerSize;
        _cone.transform.localScale = new Vector3(coneSize, coneSize, 5f);
        _spotLight.range = spotlightSize;
    }

    private void ToggleHighlight(bool toggle)
    {
        if (toggle)
        {
            _lamp1Mat.SetColor(_emissionColorId, _highlightColor);
            _lamp2Mat.SetColor(_emissionColorId, _highlightColor);
            _lampbaseMat.SetColor(_emissionColorId, _highlightColor);
            _isHighlighted = true;
        }
        else
        {
            _lamp1Mat.SetColor(_emissionColorId, Color.black);
            _lamp2Mat.SetColor(_emissionColorId, Color.black);
            _lampbaseMat.SetColor(_emissionColorId, Color.black);
            _isHighlighted = false;            
        }
    }


    private IEnumerator IdleState()
    {
        do
        {
            
            int RandomChance = Random.Range(1, 10);
            
            if (_IdleLightIsOn)
            {
                ToggleIdleLight(false);
            }
            else
            {
                if (RandomChance > 8)
                {
                    ToggleIdleLight(true);     
                }
            }

            yield return _wait01;
        } while (!_lightIsOn);
    }

    public void StartWiggle(float power)
    {
        _bWiggling = true;
        StartCoroutine(Wiggle(power));
    }
    
    private IEnumerator Wiggle(float power)
    {
        // set direction
        Vector3 dir = transform.position - PlayerController.Instance.transform.position;
        dir = dir.normalized;
        Vector4 dirv4 = new Vector4(dir.x, 0f, dir.z, 0f);
        
        float wiggleAmount = 0f;
        float limit = power;
        float speed = 8f;
        float t = 0.5f;
        bool bForward = true;
        
        // loop through all mats to set dir
        for (int i = 0; i < _wiggleMats.Length; i++)
        {
            _wiggleMats[i].SetVector(_deformDirId, dirv4);
        }
        
        Debug.Log("Dir:" + dirv4);
        
        //Doesn't build up
        do
        {
            t += Time.deltaTime * speed;

            wiggleAmount = Mathf.Lerp(-power, power, t);
            
            for (int n = 0; n < _wiggleMats.Length; n++)
            {
                _wiggleMats[n].SetFloat(_wiggleDeformId, wiggleAmount);
            }

            if (wiggleAmount >= limit && bForward)
            {
                bForward = false;
                limit = limit / 2;
                speed *= -1f;
            }
            else if (wiggleAmount <= -limit && !bForward)
            {
                bForward = true;
                limit = limit / 2;
                speed *= -1f;
            }

            // When does it stop. maybe reset values as well?
            if (limit < 0.01f)
            {
                _bWiggling = false;
                for (int n = 0; n < _wiggleMats.Length; n++)
                {
                    _wiggleMats[n].SetFloat(_wiggleDeformId, 0f);
                }
            }
            
            yield return null;
        } while (_bWiggling);
        
        yield break;
    }
    
    private IEnumerator PosIndicator()
    {
        float minDistance = 10f;

        do
        {
            if (!_playerCanInteract && !PlayerController.Instance._canInteract && !_lightIsOn && !_lightIsflashing)
            {
                if (Vector3.Distance(transform.position, PlayerController.Instance.transform.position) < minDistance)
                {
                    _posIndicator.SetActive(true);
                }
                else
                {
                    _posIndicator.SetActive(false);
                }
            }
            else
            {
                _posIndicator.SetActive(false);
            }

            yield return _wait03;
        } while (true);
    }
}
