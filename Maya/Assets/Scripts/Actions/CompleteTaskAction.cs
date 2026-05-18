using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTaskAction", menuName = "Actions/Task")]
public class CompleteTaskAction : InteractableAction
{
    public static event Action<string> OnTaskComplete;

    public override bool Execute(InteractableObject owner)
    {
        if (owner.wasInteractedInThisPhase == true) return true;

        owner.wasInteractedInThisPhase = true;
        OnTaskComplete?.Invoke(owner.itemID);

        return true;
    }
}
