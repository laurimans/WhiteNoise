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

    [SerializeField] private InputManager InputManager;

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

        if (InputManager != null)
        {
            InputManager.OnInteractableItemClicked += InteractionWithItem;
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
        Debug.Log("Has interactuado con:" + item.name);
    }
}
