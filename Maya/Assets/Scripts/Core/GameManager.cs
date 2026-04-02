using System.Collections.Generic;
using UnityEngine;

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
        StartNewDay(0); // Day 1: Sunday



    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void StartNewDay(int dayId)
    {
        currentDay = daysList[dayId];
        tasksDone = 0;
        cluesFound = 0;
    }
}
