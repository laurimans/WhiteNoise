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
    Final
}



public class GameManager : MonoBehaviour
{
    public int cluesFound { get; private set; } = 0;
    public int tasksDone { get; private set; } = 0;

    [SerializeField] private List<Day_SO> daysList;

    // Day Phase
    private GamePhase currentPhase = 0;
    private Day_SO currentPhaseData;
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

    private void LoadDayPhase(GamePhase _currentPhase)
    {
        currentPhase = _currentPhase;
        currentPhaseData = daysList[(int)currentPhase];

        // Reiniciar los contadores
        tasksDone = 0;
        cluesFound = 0;


        OnPhaseChanged?.Invoke(currentPhase);
        Debug.Log($"Iniciando: {currentPhase.ToString()}");
    }


    private void InteractionWithItem(InteractableObject item)
    {
        item.OnObjectClicked();


        // Dialogo
        //if (!string.IsNullOrEmpty(behaviour.dialogue))
            //DialogueManager.Instance.ShowDialogue(behaviour.dialogue);
        

        // Sound Effect
        //if (behaviour.soundEffect != null) 
            //AudioManager.Instance.Play3DSFX(behaviour.soundEffect, item.transform.position); ;

        // Tasks and Clues

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

    private void CheckEndDayPhase()
    {
        if (tasksDone== currentPhaseData.tasksNumber)
        {
            // Pemitir acabar la fase
        }
    }



    private void OnDestroy()
    {
        if (InputManager.Instance != null)
            InputManager.Instance.OnInteractableItemClicked -= InteractionWithItem;
        
    }
}
