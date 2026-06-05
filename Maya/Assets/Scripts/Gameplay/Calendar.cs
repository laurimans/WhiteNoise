using System;
using UnityEngine;

public class Calendar : MonoBehaviour
{
    [SerializeField] private GameObject[] marks;

    private void Awake()
    {
        GameManager.OnPhaseChanged += HandlePhaseChange;
    }
    private void OnDestroy()
    {
        GameManager.OnPhaseChanged += HandlePhaseChange;
    }

    private void Start()
    {
        foreach (GameObject go in marks)
        {
            go.SetActive(false);
        }
    }

    private void HandlePhaseChange(GamePhase gamePhase)
    {
        switch (gamePhase)
        {
            case GamePhase.MondayMorning:
                marks[0].SetActive(true);
                break;

            case GamePhase.TuesdayMorning:
                marks[1].SetActive(true);
                break;

            case GamePhase.WednesdayMorning:
                marks[2].SetActive(true);
                break;

            case GamePhase.ThursdayMorning:
                marks[3].SetActive(true);
                break;

            case GamePhase.FinalDay:
                foreach (GameObject go in marks)
                {
                    go.SetActive(false);
                }
                break;
        }
    }
}
