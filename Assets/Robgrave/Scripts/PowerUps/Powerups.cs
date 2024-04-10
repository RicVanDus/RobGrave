using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PowerupType {
    green, blue, purple
}

[CreateAssetMenu(fileName = "New powerup", menuName = "RobGrave/Powerups")]
public class Powerups : ScriptableObject
{
    public PowerupType type;
    [Space]
    public string name;
    public string description;
    [SerializeField, TextArea] public string technical;
    public string item;
    public Sprite icon;
    [Space] 
    public float duration;
    public float value;
}