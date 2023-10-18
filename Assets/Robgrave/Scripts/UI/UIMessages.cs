using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;


public class UIMessages : MonoBehaviour
{
    
    // instance
    // method to add messages (enum). Which will queue up in an array (in case more events adding messages come in).
    

    public float messageStayTime;
    public Image iconGreenGhost;

    public static UIMessages Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        UIMessage newGhost = new UIMessage();
        newGhost.type = UIMessageType.Warning;
        newGhost.mainText = "Additional ghost spawned!";
        newGhost.icon = iconGreenGhost;
    }
    
    
    public void CreateMessage()
    {
        
    }
}

public enum UIMessageType
{
    Unlocked,
    Warning,
    PowerUp
}


public class UIMessage
{
    public Image icon;
    public string mainText;
    public string subText;
    public UIMessageType type;
}