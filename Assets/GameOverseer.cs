using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum GameState
{
    Menu,
    StartGame,
    Playing,
    Pause,
    Extract,
    Shop,
    GameOver
}

public class GameOverseer : MonoBehaviour
{
    public static GameOverseer Instance;
    public GameState gameState;

    public Action StartGame;
    public Action Pause;
    public Action Playing;
    public Action Menu;
    public Action Shop;
    public Action Extract;
    public Action GameOver;

    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject extractScreen;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject pauseMenu;
    
    private Scene _gameScene;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        Menu += ShowMainMenu;
        StartGame += LoadGame;
        Pause += ShowPauseMenu;
        GameOver += ShowGameOver;
        Extract += ShowExtracted;
        Playing += DisableAllOverlays;
    }

    private void OnDisable()
    {
        Menu -= ShowMainMenu;
        StartGame -= LoadGame;
        Pause -= ShowPauseMenu;
        GameOver -= ShowGameOver;
        Extract -= ShowExtracted;
        Playing -= DisableAllOverlays;
    }

    private void Start()
    {
        _gameScene = SceneManager.GetSceneByName("Game");
        
        DisableAllOverlays();
        
        SetGameState(GameState.Menu);
    }

    private void LoadScene(String sceneName, bool setActive, bool isLevel)
    {
        var _scene = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        if (setActive)
        {
            loadingScreen.SetActive(true);
            
            StartCoroutine(SetActiveScene(_scene, sceneName, isLevel));
            _scene.allowSceneActivation = true;
        }
    }

    private void UnloadScene(String sceneName)
    {
        // unload scene and make game scene active

        SceneManager.UnloadSceneAsync(sceneName);
        SceneManager.SetActiveScene(_gameScene);
    }

    public void SetGameState(GameState ToGameState)
    {
        this.gameState = ToGameState;

        switch (gameState)
        {
            case GameState.Menu :
                Menu?.Invoke();
                break;
            case GameState.StartGame :
                StartGame?.Invoke();
                break;
            case GameState.Pause :
                Pause?.Invoke();
                break;
            case GameState.Playing :
                Playing?.Invoke();
                break;
            case GameState.Extract :
                Extract?.Invoke();
                break;
            case GameState.Shop :
                Shop?.Invoke();
                break;
            case GameState.GameOver :
                GameOver?.Invoke();
                break;
        }
    }
    
    // Loops until scene is loaded, then sets active
    private IEnumerator SetActiveScene(AsyncOperation scene, String sceneName, bool isLevel)
    {
        bool _isLoaded = false;

        do
        {
            Debug.Log("LOADING PROGRESS: " + scene.progress);
            _isLoaded = scene.isDone;

            if (_isLoaded)
            {
                loadingScreen.SetActive(false);
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
                if (isLevel) GameManager.Instance.StartingGame();
            }
            yield return null;

        } while (_isLoaded == false);
    }


    private void ShowMainMenu()
    {
        DisableAllOverlays();
        LoadScene("MainMenu", true, false);
    }

    private void LoadGame()
    {
        UnloadScene("MainMenu");
        LoadScene("Graveyard_01", true, true);
    }

    private void ShowGameOver()
    {
        gameOverScreen.SetActive(true);
    }

    private void ShowPauseMenu()
    {
        DisableAllOverlays();
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }
    
    private void ShowExtracted()
    {
        extractScreen.SetActive(true);
    }

    private void DisableAllOverlays()
    {
        extractScreen.SetActive(false);
        gameOverScreen.SetActive(false);
        pauseMenu.SetActive(false);
        loadingScreen.SetActive(false);
        Time.timeScale = 1f;
    }
}
