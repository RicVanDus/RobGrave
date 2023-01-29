using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "RobGrave/Create new Level")]
public class LevelProperties : ScriptableObject
{
    public int level;
    public int ghostSpawnType0;
    public int ghostSpawnType1;
    public int ghostSpawnType2;
    public int valuablesRequired;

    public int graveType1;
    public int graveType2;
    public float timeBetweenLightning;

    [Header("Start Time")] 
    public int startHours;
    public int startMinutes;
    public float startSeconds;

    [Header("Weather")]
    public Enum_Weather weather;

    [Header("Map")] public Enum_Map map;
}

