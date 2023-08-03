using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ValuePopup : MonoBehaviour
{
   
    public Text valueText;
    
    public void PopUpScore(int score, bool isPositive, int multiplier)
    {
        string _text;
        Color _color = new Color(1f, 1f, 0f);

        if (isPositive)
        {
            _text = "+$";
        }
        else
        {
            _text = "-$";
            _color = new Color(1f, 0f, 0f);
        }

        _text += score.ToString();

        if (multiplier > 0)
        {
            _text += " (+" + multiplier.ToString() + "0%)";
        }

        valueText.text = _text;
        valueText.color = _color;

        Sequence _seq = DOTween.Sequence();

        _seq.Append(transform.DOMoveY(transform.position.y + 2, 0.5f).SetEase(Ease.OutBounce));
        _seq.AppendInterval(1.5f);
        _seq.OnComplete(Kill);
   
    }

    private void Kill()
    {
        Destroy(gameObject);
    }
}
