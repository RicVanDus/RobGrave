using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New GiftBox", menuName = "RobGrave/Giftboxes/Create Item")]
public class GiftBoxItemTemplate : GiftBoxTemplate
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Apply()
    {
        // add powerup to playercontroller (int) and destroy this gameobject
    }
}