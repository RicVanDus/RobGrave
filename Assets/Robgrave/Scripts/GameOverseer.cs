using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
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

    public int score = 0;
    public int currentLives = 0;
    public int maxLives = 0;
    public int currentLevel = 0;

    public int upgradeTools;
    public int upgradeRG;
    
    private Scene _gameScene;
    [Header("UI")]
    [SerializeField] private EventSystem _eventSystem;
    [SerializeField] private PauseMenu _pauseMenu;
    
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
        Playing += PlayingGame;
    }

    private void OnDisable()
    {
        Menu -= ShowMainMenu;
        StartGame -= LoadGame;
        Pause -= ShowPauseMenu;
        GameOver -= ShowGameOver;
        Extract -= ShowExtracted;
        Playing -= PlayingGame;
    }

    private void Start()
    {
        _gameScene = SceneManager.GetSceneByName("Game");
        
        DisableAllOverlays();
        
        SetGameState(GameState.Menu);

        Cursor.visible = false;
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

    private IEnumerator QueueScene(String sceneName, bool isLevel, bool setActive)
    {
        bool _isUnloaded = false;
        loadingScreen.SetActive(true);

        do
        {
            Debug.Log("UNLOADING SCENES CHECK");
            int sceneCount = SceneManager.sceneCount;

            if (sceneCount == 1)
            {
                _isUnloaded = true;
                LoadScene(sceneName, isLevel, setActive);
            }

            yield return null;

        } while (_isUnloaded == false);
    }
    
    private void ShowMainMenu()
    {
        DisableAllOverlays();
        UnloadAllScenes();
        
    StartCoroutine(QueueScene("MainMenu", true, false));
    }
    
    private void LoadGame()
    {
        loadingScreen.SetActive(true);
        UnloadAllScenes();
        StartCoroutine(QueueScene("Graveyard_01", true, true));
    }
    
    //Unloads all scenes except GAME scene
    private void UnloadAllScenes()
    {
        int index = SceneManager.sceneCount;

        if (index > 1)
        {
            for (int i = 0; i < index; i++)
            {
                var _scene = SceneManager.GetSceneAt(i);
                if (_scene.name != "Game")
                {
                    UnloadScene(_scene.name);
                }
            }
        }
    }
    
    private void ShowGameOver()
    {
        gameOverScreen.SetActive(true);
    }
    
    private void ShowPauseMenu()
    {
       // _eventSystem.firstSelectedGameObject = _pauseMenu.defaultSelected;
        DisableAllOverlays();
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }
    
    private void ShowExtracted()
    {
        extractScreen.SetActive(true);
        score = PlayerController.Instance.score;
        maxLives = PlayerController.Instance.maxLives;
        currentLives = PlayerController.Instance.currentLives;
        Time.timeScale = 0f;
    }
    
    private void DisableAllOverlays()
    {
        extractScreen.SetActive(false);
        gameOverScreen.SetActive(false);
        pauseMenu.SetActive(false);
        loadingScreen.SetActive(false);
    }
    
    private void PlayingGame()
    {
        DisableAllOverlays();
        Time.timeScale = 1f;
    }
}