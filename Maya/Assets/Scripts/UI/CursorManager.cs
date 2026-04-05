using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D defaultCursor;
    [SerializeField] private Texture2D interactableCursor;
    [SerializeField] private Vector2 hotSpot = Vector2.zero;

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

    public void SetInteractableCursor()
    {
        Cursor.SetCursor(interactableCursor, hotSpot, CursorMode.Auto);
    }

    public void SetDefaultCursor()
    {
        Cursor.SetCursor(defaultCursor, hotSpot, CursorMode.Auto);
    }
}
