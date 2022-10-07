using UnityEngine;
using UnityEngine.UI;

public class UIPlayerInfo : MonoBehaviour
{
    public Text hitPoints;
    public Text score;

    public Transform graphicGhostsTarget;
    public Image graphicGhost;

    private void Start()
    {
        
    }

    private void Awake()
    {
        EnemyManager.Instance.EnemyUpdate += UpdateGhosts;
    }

    void Update()
    {
        hitPoints.text = PlayerController.Instance.hitPoints.ToString();
        //score.text = "Score: " + PlayerController.Instance.score.ToString();
    }

    public void UpdateGhosts()
    {
        int nbChildren = graphicGhostsTarget.childCount;

        for (int i = nbChildren - 1; i >= 0; i--)
        {
            DestroyImmediate(graphicGhostsTarget.GetChild(i).gameObject);
        }

        for (int i = 0; i < EnemyManager.Instance.enemies.Count; i++)
        {
            Enemy _ghost = EnemyManager.Instance.enemies[i];
            Color _ghostColor = EnemyManager.Instance.GhostType1;


            switch (_ghost.ghostType)
            {
                case 0:
                    _ghostColor = EnemyManager.Instance.GhostType1;
                    break;
                case 1:
                    _ghostColor = EnemyManager.Instance.GhostType2;
                    break;
                case 2:
                    _ghostColor = EnemyManager.Instance.GhostType3;
                    break;
                case 3:
                    _ghostColor = EnemyManager.Instance.GhostType4;
                    break;
            }
            

            Debug.Log(_ghost.ghostType);

            Image _newGhostGraphic = Instantiate(graphicGhost, graphicGhostsTarget);
            _newGhostGraphic.color = _ghostColor;
            
        }

    }
}
