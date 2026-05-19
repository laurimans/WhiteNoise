using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NewOpenJournalAction", menuName = "Actions/OpenJournal")]
public class OpenJournalAction : InteractableAction
{
    public static event Action OnJournalClicked;
    public override bool Execute(InteractableObject owner)
    {
        OnJournalClicked?.Invoke();
        return true;
    }
}
