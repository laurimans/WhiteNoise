using System;
using System.Collections;
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
    [SerializeField] private bool isExitLock = true;

    [Header("Days and Rooms")]
    [SerializeField] private List<DayPhaseData> daysList;
    [SerializeField] private Room[] roomList;
    [SerializeField] private TransitionUI transitionPanel;
    [SerializeField] private AudioSource[] roomAudioSources;

    [Header("Journal System")]
    [SerializeField] private JournalManager journalManager;
    [SerializeField] private JournalUI journalUI;

    [Header("Glitches")]
    [SerializeField] private GlitchManager glitchManager;

    // Day Phase
    private DayPhaseData currentPhaseData;
    private bool isJournalPickedUp = false;
    private bool glitchTriggered = false;

    //Eventos
    public static event Action<GamePhase> OnPhaseChanged;
    public static event Action OnGameEnd;
    public static event Action<bool> OnExitLockChange;
    public static event Action OnTaskDone;
    public static event Action OnTransitionStart;
    public static event Action OnTransitionEnd;

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

        GiveClueAction.OnClueFound += AddClue;
        CompleteTaskAction.OnTaskComplete += AddTaskDone;

        TryExitAction.OnExitSucess += NextPhase;
        PickUpJournalAction.OnPickUpJournal += PickUpJournal;
    }

    private void OnDisable()
    {
        if (InputManager.Instance != null)
            InputManager.Instance.OnInteractableItemClicked -= InteractionWithItem;

        GiveClueAction.OnClueFound -= AddClue;
        CompleteTaskAction.OnTaskComplete -= AddTaskDone;

        TryExitAction.OnExitSucess -= NextPhase;
        PickUpJournalAction.OnPickUpJournal -= PickUpJournal;
    }

    public GamePhase GetCurrentPhase() => currentPhase;
    public bool IsExitLocked() => isExitLock;
    public DayPhaseData GetCurrentPhaseData() => daysList[(int)currentPhase];
    public bool IsJournalPickedUp() => isJournalPickedUp;

    public void PickUpJournal()
    {
        isJournalPickedUp = true;
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
   
        OnTransitionStart?.Invoke();
        // Transition
        StartCoroutine(transitionPanel.PhaseTransition(() =>
        {
            LoadDayPhase((GamePhase)nextPhase);
            OnTransitionEnd?.Invoke();
        }, daysList[nextPhase].transitionAudio));

        
        CursorManager.Instance.SetDefaultCursor();
    }

    private IEnumerator WaitAndTriggerGlitch(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (!glitchTriggered && currentPhase == GamePhase.ThursdayMorning)
        {
            glitchTriggered = true;
            isExitLock = true;

            glitchManager.StartFinalSequence();
        }
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
        isExitLock = true;

        OnPhaseChanged?.Invoke(currentPhase);

        Debug.Log($"Iniciando: {currentPhase.ToString()}");

        AudioManager.Instance.SetupDayAmbience(roomList, currentPhase);

        if (_currentPhase == GamePhase.ThursdayMorning)
        {
            GameObject camBtn = GameObject.Find("CameraButton");
            if (camBtn != null) camBtn.SetActive(false);

            if (glitchManager != null)
            {
                glitchManager.StartCoroutine("RandomScreenGlitches");
            }

            StartCoroutine(WaitAndTriggerGlitch(45f));
        }

        if (_currentPhase == GamePhase.FinalDay)
        {
            glitchManager.StopAllGlitches();
        }

        UpdateJournalForNewPhase();
    }

    private void UpdateJournalForNewPhase()
    {
        string journalKey = $"JOURNAL_P{(int)currentPhase}";
        var entryData = LocalizationManager.Instance.GetJournalEntry(journalKey);

        if (entryData != null)
        {
            journalManager.AddEntry(entryData.date, entryData.content);
            Debug.Log(entryData.title);
        }
        Debug.Log(journalKey);
    }


    private void InteractionWithItem(InteractableObject item)
    {
        item.OnObjectClicked();

        Debug.Log("Has interactuado con:" + item.name);
    }

    public void AddClue(string clueId)
    {
        if (!isJournalPickedUp) return; // Sin diario n puede apuntar listas

        OnTaskDone?.Invoke();
        cluesFound++;

        string textForJournal = currentPhaseData.GetClueText(clueId);

        journalManager.AddClueToCurrentEntry(textForJournal);

        journalUI.TypeNewClue("\n- " + textForJournal);

        HandleExitLock();
    }


    public void AddTaskDone(string taskId)
    {
        OnTaskDone?.Invoke();
        tasksDone++;
        HandleExitLock();

        Debug.Log($"Tarea completada: {taskId}. Total: {tasksDone}/{currentPhaseData.tasksNumber}");
    }
}
