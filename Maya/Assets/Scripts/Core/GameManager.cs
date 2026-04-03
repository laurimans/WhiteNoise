using System.Collections.Generic;
using UnityEngine;
using System;

public enum Day
{
    Sunday,
    Monday,
    Tuesday,
    Wednesday, 
    Thursday,
    Friday
}
public class GameManager : MonoBehaviour
{
    public Day_SO currentDay { get; private set; }
    public int cluesFound { get; private set; } = 0;
    public int tasksDone { get; private set; } = 0;
    public List<Day_SO> daysList = new List<Day_SO>();
    public List<ItemBehaviour> currentObjectBehaviourList = new List<ItemBehaviour>();

    

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
        StartNewDay(daysList[0]); // Day 1: Sunday

        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnInteractableItemClicked += InteractionWithItem;
        }


    }

    private void StartNewDay(Day_SO dayToStart)
    {
        // Reiniciar los contadores
        tasksDone = 0;
        cluesFound = 0;

        //currentObjectBehaviourList = dayToStart.itemBehaviour;
        currentDay = dayToStart;

        
    }

    private void InteractionWithItem(InteractableObject item)
    {
        ItemBehaviour behaviour = currentDay.itemBehaviour.Find(x => x.idItem == item.GetID());

        if (behaviour == null) return;

        // Dialogue
        Debug.Log($"Maya piensa: {behaviour.dialogue}");

        if (!string.IsNullOrEmpty(behaviour.dialogue))
        {
            DialogueManager.Instance.ShowDialogue(behaviour.dialogue);
        }

        // Sound Effect
        if (behaviour.soundEffect != null) AudioManager.Instance.Play3DSFX(behaviour.soundEffect, item.transform.position); ;

        // Tasks and Clues
        if (behaviour.isClue) AddClue(behaviour.idItem);
        if (behaviour.isTask) AddTaskDone(behaviour.idItem);

        if (behaviour.isDayStateChanger) CheckEndDayPhase();

        Debug.Log("Has interactuado con:" + item.name);
    }

    private void AddClue(string clueId)
    {
        cluesFound++;
    }

    private void CheckEndDayPhase()
    {

    }

    private void AddTaskDone(string taskId)
    {
        tasksDone++;
    }

    private void OnDestroy()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnInteractableItemClicked -= InteractionWithItem;
        }
    }
}
