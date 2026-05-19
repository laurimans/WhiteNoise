using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NewPickUpCameraAction", menuName = "Actions/PickUpCamera")]
public class PickUpCameraAction : InteractableAction
{
    public static event Action OnCameraPicked;
    public override bool Execute(InteractableObject owner)
    {
        if (owner.GetInteractionData()) return true;

        OnCameraPicked?.Invoke();
        owner.gameObject.SetActive(false);

        return true;
    }
}
