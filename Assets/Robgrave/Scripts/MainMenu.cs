using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button defaultSelected;
    public AudioClip music;

    private void Awake()
    {
        transform.gameObject.SetActive(true);
    }

    private void Start()
    {
        AudioManager.Instance.PlayMusic(music);
    }

    public void StartGame()
    {
        transform.gameObject.SetActive(false);
        GameOverseer.Instance.SetGameState(GameState.StartGame);
        defaultSelected.Select();
        
    }
    
    
    public void Quit()
    {
        Application.Quit();        
    }
}