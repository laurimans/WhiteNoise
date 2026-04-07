using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DayPhaseData", menuName = "Scriptable Objects/DayPhaseData")]
public class DayPhaseData : ScriptableObject
{
    public GamePhase id_day;
    public int tasksNumber;
    public int cluesNumber;

    public AudioClip transitionAudio;
}


