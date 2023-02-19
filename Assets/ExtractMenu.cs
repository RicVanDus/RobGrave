using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ExtractMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _extractText;

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
}
