using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ExtractMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _extractText;

    private void OnEnable()
    {
        GameOverseer.Instance.Extract += UpdateText;
        UpdateText();
    }


    private void OnDisable()
    {
        GameOverseer.Instance.Extract -= UpdateText;
    }

    public void NextLevel()
    {
        GameOverseer.Instance.SetGameState(GameState.StartGame);
    }

    public void ToMainMenu()
    {
        GameOverseer.Instance.SetGameState(GameState.Menu);
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }

    public void UpdateText()
    {
        
        _extractText.text = "You have escaped with $" + PlayerController.Instance.score + "!";
    }
}
