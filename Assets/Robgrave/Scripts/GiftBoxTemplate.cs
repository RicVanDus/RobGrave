using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public abstract class GiftBoxTemplate : ScriptableObject
{

    public Rarity rarity;
    public GiftType giftType;
    public float duration;
    public float value;

    public abstract void Apply();

}

public enum GiftType
{
    Active,
    Timed,
    ToolsUpgrade,
    CharacterUpgrade,
    CryptKey
}

public enum Rarity
{
    Green,
    Blue,
    Purple
}
