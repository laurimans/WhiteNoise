using System;
using UnityEngine;
using System.Threading.Tasks;

[CreateAssetMenu(fileName = "NewClueAction", menuName = "Actions/Clue")]
public class GiveClueAction : InteractableAction
{
    public static event Action<string> OnClueFound;

    public override bool Execute(InteractableObject owner)
    {
        if (owner.GetInteractionData() == true) return true;

        owner.MarkAsInteracted();

        InvokeWithDelay(owner.itemID);

        return true;
    }

    private async void InvokeWithDelay(string itemID)
    {
        await Task.Delay(1000);
        OnClueFound?.Invoke(itemID);
    }
}
