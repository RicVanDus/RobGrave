using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class UIScore : MonoBehaviour
{
    [SerializeField] private GameObject _nrRotator1;
    [SerializeField] private GameObject _nrRotator2;
    [SerializeField] private GameObject _nrRotator3;
    [SerializeField] private GameObject _nrRotator4;
    [SerializeField] private GameObject _nrRotator5;

    [SerializeField] private GameObject _preScoreObj;
    [SerializeField] private TextMeshProUGUI _preScoreText;
    private bool _preScoreOut;

    [SerializeField] private GameObject _reqScoreObj;
    [SerializeField] private TextMeshProUGUI _reqScoreText;
    [SerializeField] private Light _reqScoreLight;
    private bool _reqScoreMet;
    private int _reqScore;

    private Color _redColor = Color.red;
    private Color _greenColor = Color.green;

    private Vector3 _preScoreObjInPos;
    private Vector3 _preScoreObjOutPos;

    void Start()
    {
        _reqScore = GameManager.Instance.thisLevel.valuablesRequired;
        _preScoreObjInPos = _preScoreObj.transform.position;
        _preScoreObjOutPos = _preScoreObjInPos;
        _preScoreObjOutPos.z = 6.89f;
        _reqScoreText.text = "/ " + _reqScore.ToString();
    }

    public void UpdateScore(int scoreAdded, int oldScore)
    {
        int score = PlayerController.Instance.score;
        
        if (_preScoreOut)
        {
            _preScoreOut = false;
            _preScoreObj.transform.DOLocalMoveZ(5f, 0.7f).SetEase(Ease.InExpo);
        }

        if (scoreAdded != 0)
        {
            RotateNumber(_nrRotator1, scoreAdded);

            if (score > 9 || oldScore > 9)
            {
                int rot = scoreAdded + (oldScore % 10); 
                RotateNumber(_nrRotator2, rot/10);                
            }
            if (score > 99 || oldScore > 99)
            {
                int rot = scoreAdded + (oldScore % 100); 
                RotateNumber(_nrRotator3, rot/100);
            }
            if (score > 999 || oldScore > 999)
            {
                int rot = scoreAdded + (oldScore % 1000); 
                RotateNumber(_nrRotator4, rot/1000);
            }
            if (score > 9999 || oldScore > 9999)
            {
                int rot = scoreAdded + (oldScore % 10000); 
                RotateNumber(_nrRotator5, rot/10000);
            }
        }
        else
        {
            return;
        }

        if (score >= _reqScore && !_reqScoreMet)
        {
            ReqScoreToggle(true);
            _reqScoreMet = true;
        }
        else if (score < _reqScore && _reqScoreMet)
        {
            ReqScoreToggle(false);
            _reqScoreMet = false;
        }
    }

    public void UpdatePreScore()
    {
        int preScore = PlayerController.Instance.preScore;

        if (preScore != 0)
        {
            if (!_preScoreOut)
            {
                _preScoreOut = true;
                _preScoreObj.transform.DOLocalMoveZ(6.89f, 0.7f).SetEase(Ease.InExpo);
            }

            if (preScore > 0)
            {
                _preScoreText.text = "+";
            }
            else
            {
                _preScoreText.text = "";
            }

            _preScoreText.text += preScore.ToString(); 
        }
    }

    private void ReqScoreToggle(bool isMet)
    {
        // toggle the light and textcolor
    }

    private void RotateNumber(GameObject NrRotator, int amount)
    {
        Vector3 rot = Vector3.zero;
        rot.x += 36f * amount;

        Debug.Log("Rotating X: " + 36f * amount + " || Score: " + PlayerController.Instance.score );

        NrRotator.transform.DOLocalRotate(rot, 3f, RotateMode.LocalAxisAdd).SetEase(Ease.OutExpo);
    }
}
