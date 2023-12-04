using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

public class UIMessage : MonoBehaviour
{
    public MessageIcon icon;
    public string titleText;
    public string subText;
    public UIMessageType type;

    private float _messageTimer;

    public int newPosition;
    private int _currentPosition;

    [SerializeField] private TextMeshProUGUI _tmpTitle;
    [SerializeField] private TextMeshProUGUI _tmpText;
    [SerializeField] private Light _light;
    [SerializeField] private GameObject _iconQuad;

    private Color redColor = new Color(0.84f, 0f, 0f);
    private Color greenColor = new Color(0f, 0.74f, 0.17f);
    private Color yellowColor = new Color(0.74f, 0.68f, 0f);
    
    private Vector3 _defaultPos = new Vector3(-198f, -5.75f, 21f);

    private WaitForSeconds _wait01 = new WaitForSeconds(0.1f);
    private bool _isVisible = true;
        
    void Start()
    {
        _currentPosition = newPosition;
        UpdateMessage();        
    }


    private void UpdateMessage()
    {
        if (type == UIMessageType.Good)
        {
            _light.color = greenColor;
            _tmpTitle.color = greenColor;
        }
        else if (type == UIMessageType.Bad)
        {
            _light.color = redColor;
            _tmpTitle.color = redColor;
        }
        else
        {
            _light.color = yellowColor;
            _tmpTitle.color = yellowColor;
        }

        _tmpText.text = titleText;
        _tmpText.text = subText;
        Material mat = GetComponent<Renderer>().material;

        //icon
        Texture2D iconTexture;

        switch (icon)
        {
            default :
                iconTexture = UIMessages.Instance.icons[0];
                break;
        }
        
        mat.SetTexture("Albedo", iconTexture);

        //position
        Vector3 newPosition = _defaultPos;

        float posY = 1.1f * _currentPosition;
        newPosition.y += posY;         
        
        //rotation
        Vector3 rotationStart = transform.eulerAngles;
        Vector3 rotationEnd = rotationStart;
        rotationStart.x = 90f;

        transform.eulerAngles = rotationStart;

        transform.DORotate(rotationEnd, 0.5f).SetEase(Ease.OutBounce);
    }
    
    //check for position in queue & update Timer
    void Update()
    {
        if (_currentPosition != newPosition)
        {
            StartCoroutine(ToPosition(newPosition));
            _currentPosition = newPosition;
        }

        _messageTimer += Time.deltaTime;

        if (_messageTimer > UIMessages.Instance.messageStayTime)
        {
            transform.gameObject.SetActive(false);
            UIMessages.Instance.HideMessage(this);
        }
    }

    // tween an animation at the right spot
    private void SpawnMessage()
    {
        
    }
    
    
    private IEnumerator ToPosition(int newIndex)
    {
        Vector3 targetPosition = _defaultPos;

        float posY = 1.1f * newIndex;
        targetPosition.y += posY;

        float t = 0.1f;
        
        while (_isVisible)
        {
            Vector3 newPosition = Vector3.Lerp(transform.position, targetPosition, t);
            
            transform.position = newPosition;
            
            yield return _wait01;    
        }
    }

    // send to the underworld and hide
    private IEnumerator HidingMessage()
    {
        yield break;
    }
    
}


public enum MessageIcon
{
    GreenGhost,
    BlueGhost,
    PurpleGhost,
    OrangeGhost,
    CryptKey,
    GhostUpgradedBlue,
    GhostUpgradedPurple,
    
}