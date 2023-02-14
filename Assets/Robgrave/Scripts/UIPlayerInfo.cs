using System;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.UI;

public class UIPlayerInfo : MonoBehaviour
{

    public Text score;
    public Text preScore;
    public Text targetScore;
    public TextMeshProUGUI gameTime;

    public Transform graphicGhostsTarget;
    public GameObject graphicGhost;
    public Image graphicLives;
    public Image graphicLivesEmpty;
    public Transform graphicLivesTarget;

    private string _startTime;
    private string _currentGameTime;
    
    private void OnEnable()
    {
        PlayerController.Instance.GettingCaught += UpdateLives;
        EnemyManager.Instance.EnemyUpdate += UpdateGhosts;
        GameOverseer.Instance.Playing += UpdateAll;
    }

    private void OnDisable()
    {
        PlayerController.Instance.GettingCaught -= UpdateLives;
        EnemyManager.Instance.EnemyUpdate -= UpdateGhosts;
        GameOverseer.Instance.Playing -= UpdateAll;
    }

    private void Start()
    {
        UpdateLives();
    }
 
    void Update()
    {
        UpdateScore();
        UpdateTime();
    }

    public void UpdateGhosts()
    {
        int nbChildren = graphicGhostsTarget.childCount;

        if (nbChildren > 0)
        {
            for (int i = nbChildren - 1; i >= 0; i--)
            {
                Destroy(graphicGhostsTarget.GetChild(i).gameObject);
            }    
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
            
            GameObject _newGhostGraphic = Instantiate(graphicGhost, graphicGhostsTarget);
           _newGhostGraphic.GetComponent<Image>().color = _ghostColor;
            // Filling up the ghost as its evolving
            Image _ghostFill = _newGhostGraphic.transform.GetChild(0).GetComponent<Image>();
            _ghostFill.fillAmount = _ghost.ghostEvolveProgress;
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

        if (PlayerController.Instance.goalAchieved)
        {
            targetScore.color = new Color(0f, 1f, 0f);
        } 
        else
        {
            targetScore.color = new Color(1f, 0f, 0f);
        }
    }

    public void UpdateLives()
    {
        Debug.Log("UPDATING LIVES: " + PlayerController.Instance.currentLives);
        int nbChildren = graphicLivesTarget.childCount;

        for (int i = nbChildren - 1; i >= 0; i--)
        {
            Destroy(graphicLivesTarget.GetChild(i).gameObject);
        }

        for (int i = 0; i < PlayerController.Instance.maxLives; i++)
        {
            if (i+1 <= PlayerController.Instance.currentLives)
            {
                Image liveHeart = Instantiate(graphicLives, graphicLivesTarget);
            } else {
                Image liveHeart = Instantiate(graphicLivesEmpty, graphicLivesTarget);
            }            
        }
    }

    private void UpdateTime()
    {
        gameTime.text = GameManager.Instance.gameTime;
    }

    private (int, int, float) TimeFormatConverter(string input)
    {
        string sHours = "";
        string sMinutes = "";
        string sSeconds = "";

        int hInd = input.IndexOf(":");
        int mInd = input.IndexOf("'");
        int sInd = input.IndexOf(".");

        for (int ih = 0; ih < hInd; ih++)
        {
            sHours += input[ih];
        }
        
        for (int im = hInd + 1; im < mInd; im++)
        {
            sMinutes += input[im];
        }
        
        for (int isec = mInd + 1; isec < sInd; isec++)
        {
            sSeconds += input[isec];
        }

        int iHours = Int32.Parse(sHours);
        int iMinutes = Int32.Parse(sMinutes);
        float fSeconds = float.Parse(sSeconds);
        
        return (iHours, iMinutes, fSeconds);
    }
    
    public void UpdateAll()
    {
        UpdateLives();
        UpdateGhosts();
        UpdateScore();
    }
    
    
    
}
