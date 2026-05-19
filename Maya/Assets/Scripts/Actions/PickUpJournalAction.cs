using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NewPickUpJournalAction", menuName = "Actions/PickUpJournal")]
public class PickUpJournalAction : InteractableAction
{
    public static event Action OnPickUpJournal;
    public override bool Execute(InteractableObject owner)
    {
        OnPickUpJournal?.Invoke();
        return true;
    }
}
