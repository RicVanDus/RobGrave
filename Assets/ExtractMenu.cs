using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtractMenu : MonoBehaviour
{
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
