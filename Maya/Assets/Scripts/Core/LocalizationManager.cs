using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class DialogueEntry
{
    public string key;
    public List<string> lines;
}

[Serializable]
public class DialogueListWrapper
{
    public List<DialogueEntry> items;
}

public class LocalizationManager : MonoBehaviour
{
    private Dictionary<string, List<string>> dialogueDatabase = new Dictionary<string, List<string>>();
    [SerializeField] private TextAsset jsonFile;

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
        }
    }

    #endregion

    private void LoadDatabase()
    {
        if(jsonFile != null)
        {
            DialogueListWrapper itemsData = JsonUtility.FromJson<DialogueListWrapper>(jsonFile.text);
            foreach (var entry in itemsData.items)
            {
                if (!dialogueDatabase.ContainsKey(entry.key))
                    dialogueDatabase.Add(entry.key, entry.lines);
            }

            Debug.Log("Base de datos de diálogos cargada.");
        }
        else
        {
            Debug.LogError("No se encontró el archivo ItemsDialogues en Resources.");
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
}
