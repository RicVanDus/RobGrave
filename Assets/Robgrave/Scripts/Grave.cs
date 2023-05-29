using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


// TODO: make UI variable and handle visibility from this script (including Tween anim)

public class Grave : Interactable
{
    public int graveType = 0;
    public int maxDepth;
    public int currentDepth = 0;
    public float diggingProgress = 0f;
    public int defiledDepth = 0;
    public float diggingtTime = 3.0f;
    public float defileTime = 5.0f;
    public float defileProgress = 0f;
    public int valuables;
    public bool hasGiftbox;

    [SerializeField] private Text _depthNumber;

    Gravedirt _gravedirt;    

    public bool playerIsDigging = false;
    public bool graveIsDifiling = false;
    private bool graveIsDug = false;
    private bool graveTouched = false;
    private bool graveDefiled = false;
    private bool _rotatedPlayer = false;
    [SerializeField] private GameObject _coffin;
    private GameObject _coffinLid;
    [HideInInspector] public GameObject _graveStone;
    private bool _gravestoneIsShaking = false;

    private Color _currentGraveStoneColor;
    
    protected override void Start()
    {
        SetGraveType(graveType);
        SetDepthText();

        _gravedirt = GetComponentInChildren<Gravedirt>();
        _coffinLid = _coffin.transform.GetChild(0).gameObject;
        _graveStone = transform.Find("gravestone").gameObject;
        

    }

    protected override void Update()
    {
        if (PlayerCanInteract && !graveIsDug)
        {
            CheckPlayerInteract();
        }
        else
        {
            playerIsDigging = false;

        }

        _gravedirt.ChangeDirtColor(PlayerCanInteract);

        PlayerIsDiggingOrNot();
    }

    private void CheckPlayerInteract()
    {
        if (PlayerController.Instance._interacting)
        {
            playerIsDigging = true;
            if (_rotatedPlayer == false)
            {
                RotatePlayer();    
            }
        }
        else
        {
            playerIsDigging = false;
            _rotatedPlayer = false;
        }
    }

    private void SetDepthText()
    {
        _depthNumber.text = currentDepth + " / " + maxDepth;
    }

    public void SetGraveType(int graveType)
    {
        switch (graveType)
        {
            case 0:
                maxDepth = 3;
                valuables = Random.Range(125, 250);
                _currentGraveStoneColor = GameManager.Instance.graveType0Color;
                break;

            case 1:
                maxDepth = 4;
                valuables = Random.Range(350, 500);
                _coffin.transform.position += new Vector3(0f, -0.3f, 0f);
                _currentGraveStoneColor = GameManager.Instance.graveType1Color;
                break;

            case 2:
                maxDepth = 5;
                valuables = Random.Range(600, 1000);
                _coffin.transform.position += new Vector3(0f, -0.5f, 0f);
                _currentGraveStoneColor = GameManager.Instance.graveType2Color;
                break;
        }
    }

    private void PlayerIsDiggingOrNot()
    {
        if (playerIsDigging)
        {
            PlayerController.Instance.movementDisabled = true;
            PlayerController.Instance.IsDigging(true);
            
            diggingProgress += (Time.deltaTime * PlayerController.Instance.digSpeedMultiplier);
            if (defileProgress > 0f)
            {
                defileProgress -= Time.deltaTime;
            }
            graveTouched = true;
            graveIsDifiling = false;
            _gravestoneIsShaking = false;
            StopCoroutine(ShakingHeadStone());
            SetGraveStoneColor(_currentGraveStoneColor);

            if (diggingProgress >= diggingtTime)
            {
                currentDepth += 1;
                diggingProgress = 0f;
                SetDepthText();
                _gravedirt.DirtHeight(currentDepth);
                if (currentDepth >= maxDepth)
                {
                    _coffinLid.SetActive(false);
                    PlayerController.Instance.IsDigging(false);
                    graveIsDug = true;
                    
                    Color _defaultColor = GameManager.Instance.graveDefaultColor;
                    SetGraveStoneColor(_defaultColor);
                    _gravedirt.DirtHeight(6);

                    SpawnLoot();
                }
            }
        }
        else
        {
            diggingProgress -= Time.deltaTime;
            if (diggingProgress <= 0f && graveTouched && !graveIsDug && !graveDefiled)
            {
                DefiledGrave();
            }
        }
        diggingProgress = Mathf.Clamp(diggingProgress, 0.0f, diggingtTime);
    }


    private void SpawnLoot()
    {
        int spawnNr = Random.Range(17, 26);
        int giftBoxType = 0;

        if (GameManager.Instance.cryptKeyGrave == this)
        {
            giftBoxType = 1;
        }
        else if (hasGiftbox)
        {
            giftBoxType = CalculateGiftboxType();
        }

        if (defiledDepth > 0)
        {
            valuables = (int)(valuables - (valuables * (0.2f * defiledDepth)));
            spawnNr = spawnNr - defiledDepth;
        }
        LootManager.Instance.SpawnLoot(valuables, spawnNr, giftBoxType);
    }

    private void DefiledGrave()
    {
        int _defDepth = currentDepth - defiledDepth;

        if (_defDepth > 0 && currentDepth != maxDepth && graveDefiled == false)
        {
            graveIsDifiling = true;
            defileProgress += Time.deltaTime;
            
            if (!_gravestoneIsShaking)
            {
                _gravestoneIsShaking = true;
                StartCoroutine(ShakingHeadStone());
            }

            if (defileProgress >= defileTime)
            {
                int _rnd1 = Random.Range(0, 5);
                int _rnd2 = Random.Range(0, 5);

                if (_rnd1 == _rnd2)
                {
                    graveDefiled = true;
                    EnemyManager.Instance.SpawnEnemy(graveType);
                }

                defiledDepth++;
                defileProgress = 0f;
                graveIsDifiling = false;
            }
        }
        else
        {
            graveIsDifiling = false;
            _gravestoneIsShaking = false;
            StopCoroutine(ShakingHeadStone());
            SetGraveStoneColor(_currentGraveStoneColor);
        }
    }

    private void RotatePlayer()
    {
        _rotatedPlayer = true;
        
        PlayerController.Instance.RotateToObject(this.GameObject());
    }

    private IEnumerator ShakingHeadStone()
    {
        Vector3 _oldRotation = _graveStone.transform.rotation.eulerAngles;
        Vector3 _newRotation = Vector3.zero;
        
        Color _warningColor = GameManager.Instance.graveWarningColor;

        float _colorTime = 0.5f;
        float _colorTimer = 0f;
        bool _red = false;
        
        do
        {
            _colorTimer += 0.1f;
            //headstone shaking
            float _x = Random.Range(-3f, 3f);
            float _z = Random.Range(-3f, 3f);

            _newRotation.z = _z;

            _graveStone.transform.rotation = Quaternion.Euler(_oldRotation + _newRotation);
            
            //Blinking red
            if (_colorTimer >= _colorTime)
            {
                if (_red)
                {
                    SetGraveStoneColor(_currentGraveStoneColor);
                    _red = false;
                }
                else
                {
                    SetGraveStoneColor(_warningColor);
                    _red = true;
                }

                _colorTimer = 0f;
            }

            yield return new WaitForSeconds(0.1f);
        } while (graveIsDifiling);
    }

    private void SetGraveStoneColor(Color _color)
    {
        _graveStone.GetComponent<MeshRenderer>().material.SetColor("_Color", _color);
    }

    private int CalculateGiftboxType()
    {
        float _diceRoll = Random.Range(0f, 99f);
        float _maxPurple = 97f;
        float _maxBlue = 90f;
        int _giftType = 2;

        if (graveType == 2)
        {
            _maxPurple = 50f;
            _maxBlue = 10f;
        }
        else if (graveType == 1)
        {
            _maxPurple = 90f;
            _maxBlue = 50f;            
        }

        if (_diceRoll > _maxBlue)
        {
            _giftType = 3;
        }

        if (_diceRoll > _maxPurple)
        {
            _giftType = 4;
        }

        return _giftType;
    }
}


