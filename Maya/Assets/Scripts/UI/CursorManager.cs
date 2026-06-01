using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D defaultCursor;
    [SerializeField] private Texture2D interactableCursor;
    [SerializeField] private Vector2 hotSpot = Vector2.zero;

    private bool wasCursorVisibleBeforePause = true;

    #region Singleton
    public static CursorManager Instance { get; private set; }
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
            SetDefaultCursor();
        }
    }

    #endregion

    private void OnEnable()
    {
        GameManager.OnTransitionStart += HideCursor;
        GameManager.OnTransitionEnd += ShowCursor;

        GameStateManager.OnGamePause += OnGamePaused;
        GameStateManager.OnGameResume += OnGameResumed;
    }

    private void OnDisable()
    {
        GameManager.OnTransitionStart -= HideCursor;
        GameManager.OnTransitionEnd -= ShowCursor;

        GameStateManager.OnGamePause -= OnGamePaused;
        GameStateManager.OnGameResume -= OnGameResumed;
    }

    private void HideCursor()
    {
        Cursor.visible = false;
    }

    private void ShowCursor()
    {
        Cursor.visible = true;
        SetDefaultCursor();
    }

    public void SetInteractableCursor()
    {
        Cursor.SetCursor(interactableCursor, hotSpot, CursorMode.Auto);
    }

    public void SetDefaultCursor()
    {
        Cursor.SetCursor(defaultCursor, hotSpot, CursorMode.Auto);
    }

    private void OnGamePaused()
    {
        wasCursorVisibleBeforePause = Cursor.visible;
        Cursor.visible = true;
        SetDefaultCursor();
    }

    private void OnGameResumed()
    {
        Cursor.visible = wasCursorVisibleBeforePause;
    }
}
