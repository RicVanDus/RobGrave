using UnityEngine;
using UnityEngine.UI;

public class UIPlayerInfo : MonoBehaviour
{
    public Text hitPoints;
    public Text score;

    void Update()
    {
        hitPoints.text = "Lives left: " + PlayerController.Instance.hitPoints.ToString();
        score.text = "Score: " + PlayerController.Instance.score.ToString();
    }
}
