using System.Collections.Generic;
using UnityEngine;
using System;


public class LocalizationManager : MonoBehaviour
{
    [Header("Json Files")]
    [SerializeField] private TextAsset dialoguesJsonFile;
    [SerializeField] private TextAsset phonecallsJsonFile;
    [SerializeField] private TextAsset journalJsonFile;

    private Dictionary<string, List<string>> dialogueDatabase = new Dictionary<string, List<string>>();
    private Dictionary<string, List<PhoneMessageEntry>> phoneDatabase = new Dictionary<string, List<PhoneMessageEntry>>();
    private Dictionary<string, JournalEntryData> journalDatabase = new Dictionary<string, JournalEntryData>();


    #region Singleton
    public static LocalizationManager Instance { get; private set; }

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
            LoadDatabase();
            LoadPhoneCalls();
            LoadJournalEntries();
        }
    }

    #endregion

    private void LoadDatabase()
    {
        if(dialoguesJsonFile != null)
        {
            DialogueListWrapper itemsData = JsonUtility.FromJson<DialogueListWrapper>(dialoguesJsonFile.text);
            foreach (var entry in itemsData.items)
            {
                if (!dialogueDatabase.ContainsKey(entry.key))
                    dialogueDatabase.Add(entry.key, entry.lines);
            }

            Debug.Log("Base de datos de diálogos cargada.");
        }
    }

    private void LoadPhoneCalls()
    {
        if(phonecallsJsonFile != null)
        {
            PhoneCallListWrapper data = JsonUtility.FromJson<PhoneCallListWrapper>(phonecallsJsonFile.text);
            foreach (var call in data.calls)
            {
                phoneDatabase.Add(call.key, call.dialogue);
            }
            Debug.Log("Base de datos de llamadas telefonicas cargada.");
        }
    }

    private void LoadJournalEntries()
    {
        if(journalJsonFile != null)
        {
            JournalListWrapper data = JsonUtility.FromJson<JournalListWrapper>(journalJsonFile.text);
            foreach (var entry in data.entries)
            {
                journalDatabase.Add(entry.key, entry);
            }
        }
    }

    public List<string> GetLines(string key)
    {
        if(dialogueDatabase.TryGetValue(key, out List<string> lines))
        {
            return lines;
        }

        return new List<string> { null };
    }

    public List<PhoneMessageEntry> GetPhoneCall(string key)
    {
        phoneDatabase.TryGetValue(key, out var dialogue);
        return dialogue;
    }

    public JournalEntryData GetJournalEntry(string key)
    {
        journalDatabase.TryGetValue(key, out var entry);
        return entry;
    }
}
