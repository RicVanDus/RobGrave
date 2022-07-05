using UnityEngine;
using UnityEngine.UI;

public class UIPlayerInfo : MonoBehaviour
{
    public Text hitPoints;
    
    void Update()
    {
        hitPoints.text = "Lives left: " + PlayerController.Instance.hitPoints.ToString();
        
    }
}
