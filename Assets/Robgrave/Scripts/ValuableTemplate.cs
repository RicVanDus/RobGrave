using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Valuable", menuName = "RobGrave/Create Valuable")]
public class ValuableTemplate : ScriptableObject
{
    public new string name;
    public int value;
    public GameObject mesh;
    public float mass;

}
