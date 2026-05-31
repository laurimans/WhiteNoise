using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ClueDictionary
{
    public string itemID; 
    [TextArea] public string journalText;
}

[CreateAssetMenu(fileName = "DayPhaseData", menuName = "Scriptable Objects/DayPhaseData")]
public class DayPhaseData : ScriptableObject
{
    public GamePhase id_day;
    public int tasksNumber;
    public int cluesNumber;

    public AudioClip transitionAudio;

    [Header("Journal Entry")]
    public string dateText;
    [TextArea] public string bodyText;
    public List<ClueDictionary> cluesForThisDay;

    [Header("Phone Call")]
    public bool hasPhoneCall;

    [Header("Room Settings")]
    public RoomID startingRoom;

    public string GetClueText(string id)
    {
        var entry = cluesForThisDay.Find(c => c.itemID == id);
        return entry != null ? entry.journalText : "";
    }
}


