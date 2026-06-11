using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    MainMenu,
    Playing,
    Paused,
    GameOver
}
public class GameStateManager : MonoBehaviour
{
    [SerializeField] private GameState currentState = GameState.MainMenu;
    public GameState CurrentState => currentState;
    public bool IsGameplayUIBlocked { get; private set; } = false;

    public static event Action OnGamePause;
    public static event Action OnGameResume;
    public static event Action OnGameStart;
    public static event Action OnGameOver;
    public static event Action OnGameplayExit;

    #region Singleton
    public static GameStateManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    #endregion


    private void OnEnable()
    {
        // Evento pausa
        // Evento empezar juego

        GameManager.OnGameEnd += EndGame;
    }

    private void OnDisable()
    {
        GameManager.OnGameEnd -= EndGame;
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        currentState = GameState.Playing;
        SceneManager.LoadScene("GameScene");
        OnGameStart?.Invoke();
    }

    public void SetGameplayUIBlock(bool isBlocked)
    {
        IsGameplayUIBlocked = isBlocked;
    }

    public void TogglePause()
    {
        if (currentState == GameState.Playing)
        {
            OnGamePause?.Invoke();
            Debug.Log("Pause");
            currentState = GameState.Paused;
            Time.timeScale = 0f;
            
            return;
        }
        else if (currentState == GameState.Paused)
        {
            OnGameResume?.Invoke();
            Debug.Log("Resume");
            currentState = GameState.Playing;
            Time.timeScale = 1f;
            
            return;
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        OnGameplayExit?.Invoke();

        IsGameplayUIBlocked = false;
        currentState = GameState.Playing;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        OnGameStart?.Invoke();
    }


    private void EndGame()
    {
        currentState = GameState.GameOver;
        OnGameOver?.Invoke();
    }

    public void ReturnToMenu()
    {
        OnGameplayExit?.Invoke();
        currentState = GameState.MainMenu;
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenuScene");
    }
}
