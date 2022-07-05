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
    public float timeToMidnight;

    public Enum_Weather weather;

}
