using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using Random = UnityEngine.Random;


public class UIMessages : MonoBehaviour
{
    
    // instance
    // method to add messages (enum). Which will queue up in an array (in case more events adding messages come in).

    public float messageStayTime;
    public Image iconGreenGhost;
    public List<UIMessage> visibleMessages;
    public List<UIMessage> messageArchive;
     

    public Mesh[] messageBlockMeshes;
    public Texture2D[] icons;

    public GameObject messagePrefab;

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
    }
    
    // creates new instance UIMessage, adds it to the queue and messageArchive
    public void CreateMessage(string title, string text, UIMessageType type, MessageIcon icon)
    {
        var newMessage = Instantiate(messagePrefab, transform);

        // assign random mesh
        if (messageBlockMeshes.Length > 0)
        {
            newMessage.GetComponent<MeshFilter>().mesh = messageBlockMeshes[Random.Range(0, messageBlockMeshes.Length)];
        }

        UIMessage newUIMessage = newMessage.GetComponent<UIMessage>();
        newUIMessage.type = type;
        newUIMessage.icon = icon;
        newUIMessage.titleText = title;
        newUIMessage.subText = text;
        
        visibleMessages.Add(newUIMessage);
        messageArchive.Add(newUIMessage);
        
        newUIMessage.newPosition = visibleMessages.Count - 1;
    }

    // hides it. Removes it from the queue, makes it invisible 
    public void HideMessage(UIMessage message)
    {
        visibleMessages.Remove(message);

        //Update position for all queued messages
        for (int i = 0; i < visibleMessages.Count; i++)
        {
            var UImsg = visibleMessages[i];
            UImsg.newPosition = i;
        }
    }
}

public enum UIMessageType
{
    Good,
    Bad,
    Neutral
}


