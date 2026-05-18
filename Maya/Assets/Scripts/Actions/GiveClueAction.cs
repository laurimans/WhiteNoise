using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewClueAction", menuName = "Actions/Clue")]
public class GiveClueAction : InteractableAction
{
    public static event Action<string> OnClueFound;

    public override bool Execute(InteractableObject owner)
    {
        if (owner.wasInteractedInThisPhase == true) return true;

        owner.wasInteractedInThisPhase = true;
        OnClueFound?.Invoke(owner.itemID);
        return true;
    }
}
