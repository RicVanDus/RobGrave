using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    
    
    public void StartGame()
    {
        GameOverseer.Instance.SetGameState(GameState.StartGame);
    }
    
    
    public void Quit()
    {
        Application.Quit();        
    }
}
