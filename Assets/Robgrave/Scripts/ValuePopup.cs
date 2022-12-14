using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ValuePopup : MonoBehaviour
{
   
    public Text valueText;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PopUpScore(int score, bool isPositive)
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
