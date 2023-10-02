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
    
    void Start()
    {
        UIMessage newGhost = new UIMessage();
        newGhost.type = UIMessageType.Warning;
        newGhost.mainText = "Additional ghost spawned!";
        newGhost.icon = iconGreenGhost;
    }

    void Update()
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