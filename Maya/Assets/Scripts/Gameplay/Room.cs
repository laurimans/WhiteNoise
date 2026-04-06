using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private string roomID;
    [SerializeField] private RoomData[] phasesData;
    private SpriteRenderer sRenderer;

    private GamePhase currentPhase = GamePhase.None;
    private GamePhase lastPhase = GamePhase.None;

    void Awake()
    {
        sRenderer = GetComponent<SpriteRenderer>();
        GameManager.OnPhaseChanged += RefreshRoom;
    }

    void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            RefreshRoom(GameManager.Instance.GetCurrentPhase());
        }
    }



    public string GetID() => roomID;
    public RoomData GetPhaseData() => phasesData[(int)currentPhase];

    private void RefreshRoom(GamePhase _currentPhase)
    {
        if (_currentPhase != lastPhase) // Cambio de fase
        {
            lastPhase = _currentPhase;
            currentPhase = _currentPhase;

            if (phasesData == null || (int)currentPhase >= phasesData.Length || phasesData[(int)currentPhase] == null)
            {
                Debug.LogError($"La habitacion {roomID} no tiene comportamiento para la fase {currentPhase.ToString()}");
                gameObject.SetActive(false);
                return;
            }

            sRenderer.sprite = phasesData[(int)currentPhase].defaultBackground;
        }
    }

    private void OnDestroy()
    {
        GameManager.OnPhaseChanged -= RefreshRoom;
    }
}
