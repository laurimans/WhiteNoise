using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private RoomID roomID;
    [SerializeField] private RoomData[] phasesData;

    private SpriteRenderer sRenderer;
    private GamePhase currentPhase = 0;
    private GamePhase lastPhase = GamePhase.FinalDay;
    private bool lightIsOn = true;
    private RoomData currentRoomData;

    public bool dialogueDone = false;

    void Awake()
    {
        currentPhase = 0;
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

    public RoomID GetID() => roomID;
    public bool GetLightData() => lightIsOn;
    public RoomData GetPhaseData() => phasesData[(int)currentPhase];

    public RoomData GetRoomDataAt(int index)
    {
        if (phasesData == null || index < 0 || index >= phasesData.Length)
        {
            return null;
        }
        
        return phasesData[index]; 
    }

    private void RefreshRoom(GamePhase _currentPhase)
    {
        if (_currentPhase != lastPhase) // Cambio de fase
        {
            lastPhase = _currentPhase;
            currentPhase = _currentPhase;
            dialogueDone = false;
            currentRoomData = phasesData[(int)currentPhase];

            if (phasesData == null || currentRoomData == null)
            {
                Debug.LogError($"La habitacion {roomID} no tiene comportamiento para la fase {currentPhase.ToString()}");
                gameObject.SetActive(false);
                return;
            }

            lightIsOn = currentRoomData.lightsStartsOn;
            sRenderer.sprite = phasesData[(int)currentPhase].defaultBackground;
        }
    }

    public void ToggleLight()
    {
        lightIsOn = !lightIsOn;

        if (currentRoomData.defaultBackground != null && currentRoomData.otherBackground)
        {
            Sprite sprite = lightIsOn ? currentRoomData.defaultBackground : currentRoomData.otherBackground;
            sRenderer.sprite = sprite;
        }
    }

    private void OnDestroy()
    {
        GameManager.OnPhaseChanged -= RefreshRoom;
    }
}
