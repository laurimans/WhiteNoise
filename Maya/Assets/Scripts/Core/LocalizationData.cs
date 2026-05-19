using UnityEngine;
using System;
using System.Collections.Generic;


[Serializable]
public class PhoneMessageEntry
{
    public string speaker; // "Maya" o "Mamá" [cite: 383, 385]
    public string text;
}

[Serializable]
public class PhoneCallEntry
{
    public string key;
    public List<PhoneMessageEntry> dialogue;
}

[Serializable]
public class PhoneCallListWrapper
{
    public List<PhoneCallEntry> calls;
}

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

[Serializable]
public class JournalEntryData
{
    public string key;
    public string date;
    public string title;
    public string content;
}

[Serializable]
public class JournalListWrapper
{
    public List<JournalEntryData> entries;
}
