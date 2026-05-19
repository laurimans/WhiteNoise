using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JournalEntry
{
    [SerializeField] private string date;
    [SerializeField] [TextArea] private string body;
    [SerializeField] private List<string> clues;

    public JournalEntry(string _date, string _body)
    {
        this.date = _date;
        this.body = _body;
        clues = new List<string>();
    }

    public string GetDate() => date;
    public string GetBody() => body;
    public string GetClue(int index) => clues[index];

    public void AddClue(string clueToAdd)
    {
        clues.Add(clueToAdd);
    }
}

public class JournalManager : MonoBehaviour
{
    [SerializeField] public List<JournalEntry> journalEntries;

    public int GetEntriesCount() => journalEntries.Count;

    public JournalEntry GetEntry(int index)
    {
        return journalEntries[index];
    }

    public JournalEntry GetLastEntry()
    {
        return journalEntries[journalEntries.Count - 1];
    }


    public void AddEntry(string _date, string _body)
    {
        JournalEntry newEntry = new JournalEntry(_date, _body);
        journalEntries.Add(newEntry);

        Debug.Log($"Entrada añadida al diario: {_date}. Total entradas: {journalEntries.Count}");
    }

    public void AddClue(int index, string clue)
    {
        journalEntries[index].AddClue(clue);
    }

    public void AddClueToCurrentEntry(string clueText)
    {
        if (journalEntries.Count > 0)
        {
            JournalEntry currentEntry = GetLastEntry();
            string updatedBody = currentEntry.GetBody() + "\n- " + clueText;

            journalEntries[journalEntries.Count - 1] = new JournalEntry(currentEntry.GetDate(), updatedBody);
        }
    }

}
    

