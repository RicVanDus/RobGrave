using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMessage : MonoBehaviour
{
    public Image icon;
    public string mainText;
    public string subText;
    public UIMessageType type;

    private float _messageTimer;

    public int newPosition;
    private int _currentPosition;
        
    void Start()
    {
        
    }

    
    //check for position in queue
    void Update()
    {
        
    }

    // tween an animation at the right spot
    private void SpawnMessage()
    {
        
    }
    
    
    // constantly checks its position in the queue at update and when currentPosition != newPosition then fires this one, moving it to the appropriate spot.
    private IEnumerator ToPosition()
    {
        yield break;
    }
}
