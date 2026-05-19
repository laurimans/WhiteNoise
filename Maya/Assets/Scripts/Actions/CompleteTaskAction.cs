using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTaskAction", menuName = "Actions/Task")]
public class CompleteTaskAction : InteractableAction
{
    public static event Action<string> OnTaskComplete;

    public override bool Execute(InteractableObject owner)
    {
        if (owner.GetInteractionData() == true) return true;

        owner.MarkAsInteracted();
        OnTaskComplete?.Invoke(owner.itemID);

        return true;
    }
}
