using UnityEngine;

public class CameraBoxInteractable : InteractableObject
{
    [SerializeField] private GameObject cameraUIButton;

    public override void OnObjectClicked()
    {
        base.OnObjectClicked(); 

        if (cameraUIButton != null)
        {
            cameraUIButton.SetActive(true);
            GameManager.Instance.AddTaskDone("camera");
        }
    }
}
