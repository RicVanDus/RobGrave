using System;
using TMPro;
using UnityEngine;



public class BtnPrompt : MonoBehaviour
{

    public TextMeshProUGUI tmp1;
    public TextMeshProUGUI tmp2;
    

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetTextAndIcon(String txt1, String txt2)
    {
        tmp1.text = txt1;
        tmp2.text = txt2;
    }
}

