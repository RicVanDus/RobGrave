using UnityEngine;
using UnityEngine.UI;

public class UIPlayerInfo : MonoBehaviour
{

    public Text score;
    public Text preScore;
    public Text targetScore;

    public Transform graphicGhostsTarget;
    public Image graphicGhost;
    public Image graphicLives;
    public Image graphicLivesEmpty;
    public Transform graphicLivesTarget;

    

    private void Start()
    {
        UpdateLives();
        PlayerController.Instance.Respawned += UpdateLives;
        EnemyManager.Instance.EnemyUpdate += UpdateGhosts;
    }

    private void Awake()
    {
        
        
    }

    void Update()
    {
        UpdateScore();
    }

    public void UpdateGhosts()
    {
        int nbChildren = graphicGhostsTarget.childCount;

        for (int i = nbChildren - 1; i >= 0; i--)
        {
            Destroy(graphicGhostsTarget.GetChild(i).gameObject);
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
            

            

            Image _newGhostGraphic = Instantiate(graphicGhost, graphicGhostsTarget);
            _newGhostGraphic.color = _ghostColor;
            
        }

    }

    public void UpdateScore()
    {
        score.text = PlayerController.Instance.score.ToString();

        if (PlayerController.Instance.preScore == 0)
        {
            preScore.text = "";
        }
        else if (PlayerController.Instance.preScore > 0)
        {
            preScore.text = "+" + PlayerController.Instance.preScore.ToString();
        }
        else
        {
            preScore.text = "-" + PlayerController.Instance.preScore.ToString();
        }

        targetScore.text = "/ " + GameManager.Instance.thisLevel.valuablesRequired.ToString();

        if (PlayerController.Instance.score < GameManager.Instance.thisLevel.valuablesRequired)
        {
            targetScore.color = new Color(1f, 0f, 0f);
        } 
        else
        {
            targetScore.color = new Color(0f, 1f, 0f);
        }

    }

    public void UpdateLives()
    {
        int nbChildren = graphicLivesTarget.childCount;

        for (int i = nbChildren - 1; i >= 0; i--)
        {
            Destroy(graphicLivesTarget.GetChild(i).gameObject);
        }

        for (int i = 0; i < PlayerController.Instance.maxLives; i++)
        {
            if (i+1 <= PlayerController.Instance.hitPoints)
            {
                Image liveHeart = Instantiate(graphicLives, graphicLivesTarget);
            } else {
                Image liveHeart = Instantiate(graphicLivesEmpty, graphicLivesTarget);
            }            
        }
    }
}
