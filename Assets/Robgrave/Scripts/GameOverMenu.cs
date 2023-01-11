using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
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
