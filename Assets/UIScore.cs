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

    void Start()
    {
        _reqScore = GameManager.Instance.thisLevel.valuablesRequired;
    }

    public void UpdateScore(int scoreAdded)
    {
        int score = PlayerController.Instance.score;
        
        if (scoreAdded != 0)
        {
            
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

    private void ReqScoreToggle(bool isMet)
    {
        // toggle the light and textcolor
    }

    private void RotateNumber(GameObject NrRotator, int amount)
    {
        Vector3 rot = NrRotator.transform.eulerAngles;
        rot.y += 36f * amount;

        NrRotator.transform.DORotate(rot, 3f).SetEase(Ease.OutExpo);
    }
}
