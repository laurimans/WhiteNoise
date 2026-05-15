using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum GamePhase
{
    SundayNight,
    MondayMorning,
    MondayNight,
    TuesdayMorning,
    TuesdayNight,
    WednesdayMorning,
    WednesdayNight,
    ThursdayMorning,
    FinalDay
}

public class GameManager : MonoBehaviour
{
    [Header("Game State")]
    [SerializeField] private GamePhase currentPhase = 0; 
    [SerializeField] private int cluesFound = 0;
    [SerializeField] private int tasksDone = 0;

    [Header("Days and Rooms")]
    [SerializeField] private List<DayPhaseData> daysList;
    [SerializeField] private Room[] roomList;
    [SerializeField] private TransitionUI transitionPanel;
    [SerializeField] private AudioSource[] roomAudioSources;

    [Header("Journal System")]
    [SerializeField] private JournalManager journalManager;
    [SerializeField] private JournalUI journalUI;

    // Day Phase
    private DayPhaseData currentPhaseData;
    private bool isJournalPickedUp = false;
    [SerializeField] private bool isExitLock = true;

    //Eventos
    public static event Action<GamePhase> OnPhaseChanged;
    public static event Action OnGameEnd;
    public static event Action<bool> OnExitLockChange;

    #region Singleton
    public static GameManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    #endregion
    
    
    void Start()
    {
        Initialize();
        LoadDayPhase(0);
    }

    private void OnEnable()
    {
        if (InputManager.Instance != null)
            InputManager.Instance.OnInteractableItemClicked += InteractionWithItem;

        ExitItem.OnExitClicked += NextPhase;
    }

    private void OnDisable()
    {
        if (InputManager.Instance != null)
            InputManager.Instance.OnInteractableItemClicked -= InteractionWithItem;

        ExitItem.OnExitClicked -= NextPhase;
    }

    public GamePhase GetCurrentPhase() => currentPhase;
    public bool IsExitLocked() => isExitLock;
    public DayPhaseData GetCurrentPhaseData() => daysList[(int)currentPhase];

    public void PickUpJournal(bool isPicked)
    {
        isJournalPickedUp = isPicked;
    }

    public void HandleExitLock()
    {
        bool lastState = isExitLock;

        isExitLock = true;
        if (tasksDone >= currentPhaseData.tasksNumber && cluesFound >= currentPhaseData.cluesNumber) isExitLock = false;

        if(isExitLock != lastState) OnExitLockChange?.Invoke(isExitLock);
    }

    public void WriteDailyJournal()
    {
        journalManager.AddEntry(currentPhaseData.dateText, currentPhaseData.bodyText);
        journalUI.TypeNewEntry(currentPhaseData.dateText, currentPhaseData.bodyText);
    }

    public void NextPhase()
    {
        if(currentPhase >= GamePhase.FinalDay) 
        {
            Debug.Log("Juego Finalizado");
            OnGameEnd?.Invoke();
            return;
        }

        int nextPhase = (int)currentPhase + 1;

        // Transition
        StartCoroutine(transitionPanel.PhaseTransition(() =>
        {
            LoadDayPhase((GamePhase)nextPhase);
        }, daysList[nextPhase].transitionAudio));

        CursorManager.Instance.SetDefaultCursor();
    }

 
    private void Initialize()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.RegisterRoomSources(roomAudioSources);
        }
    }

    private void LoadDayPhase(GamePhase _currentPhase)
    {
        currentPhase = _currentPhase;
        currentPhaseData = daysList[(int)currentPhase];

        // Reiniciar los contadores
        tasksDone = 0;
        cluesFound = 0;

        OnPhaseChanged?.Invoke(currentPhase);

        Debug.Log($"Iniciando: {currentPhase.ToString()}");

        AudioManager.Instance.SetupDayAmbience(roomList, currentPhase);

        if (_currentPhase == GamePhase.ThursdayMorning)
        {
            GameObject camBtn = GameObject.Find("CameraButton");
            if (camBtn != null) camBtn.SetActive(false);
        }

        journalManager.AddEntry(currentPhaseData.dateText, currentPhaseData.bodyText);
    }


    private void InteractionWithItem(InteractableObject item)
    {
        item.OnObjectClicked();

        Debug.Log("Has interactuado con:" + item.name);
    }

    public bool AddClue(string clueId)
    {
        if (!isJournalPickedUp) return false; // Sin diario n puede apuntar listas

        cluesFound++;

        string textForJournal = currentPhaseData.GetClueText(clueId);

        journalManager.AddClueToCurrentEntry(textForJournal);

        journalUI.TypeNewClue("\n- " + textForJournal);

        HandleExitLock();
        return true;
    }


    public void AddTaskDone(string taskId)
    {
        tasksDone++;
        HandleExitLock();

        Debug.Log($"Tarea completada: {taskId}. Total: {tasksDone}/{currentPhaseData.tasksNumber}");
    }
}
