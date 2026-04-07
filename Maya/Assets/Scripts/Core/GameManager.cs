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
    Final,
    None
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private int cluesFound = 0;
    [SerializeField] private int tasksDone = 0;

    [SerializeField] private List<DayPhaseData> daysList;
    [SerializeField] private Room[] roomList;
    [SerializeField] private TransitionUI transitionPanel;

    // Day Phase
    private GamePhase currentPhase = 0;
    private DayPhaseData currentPhaseData;
    private bool isNight;

    //Eventos
    public static event Action<GamePhase> OnPhaseChanged;



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
            DontDestroyOnLoad(gameObject);
        }
    }

    #endregion
    
    
    void Start()
    {
        LoadDayPhase(0);

        if (InputManager.Instance != null)
            InputManager.Instance.OnInteractableItemClicked += InteractionWithItem;
    }

    public GamePhase GetCurrentPhase() => currentPhase;

    public bool CanFinishPhase()
    {
        if (currentPhaseData == null) return false;

        bool canFinish = false;

        if (tasksDone >= currentPhaseData.tasksNumber && cluesFound >= currentPhaseData.cluesNumber) canFinish = true;

        return canFinish;
    }

    public void NextPhase()
    {
        int nextPhase = (int)currentPhase + 1;

        if (nextPhase < Enum.GetValues(typeof(GamePhase)).Length && nextPhase != (int) GamePhase.None)
        {
            StartCoroutine(transitionPanel.PhaseTransition(() =>
            {
                LoadDayPhase((GamePhase)nextPhase);
            }, daysList[nextPhase].transitionAudio));
        }
        else
        {
            Debug.Log("¡Fin del juego o Créditos!");
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
    }


    private void InteractionWithItem(InteractableObject item)
    {
        item.OnObjectClicked();

        Debug.Log("Has interactuado con:" + item.name);
    }

    public void AddClue(string clueId)
    {
        cluesFound++;
    }

    public void AddTaskDone(string taskId)
    {
        tasksDone++;
        Debug.Log($"Tarea completada: {taskId}. Total: {tasksDone}/{currentPhaseData.tasksNumber}");
    }




    private void OnDestroy()
    {
        if (InputManager.Instance != null)
            InputManager.Instance.OnInteractableItemClicked -= InteractionWithItem;
        
    }
}
