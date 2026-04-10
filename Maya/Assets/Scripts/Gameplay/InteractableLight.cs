using UnityEngine;

public class InteractableLight : InteractableObject
{
    [Header("Light Settings")]
    [SerializeField] private GameObject room;
    private bool isLightOn = true;

    public override void OnObjectClicked()
    {
        base.OnObjectClicked();

        isLightOn = !isLightOn;
        ToggleLights();
    }

    private void ToggleLights()
    {
        if (room != null)
        {
            SpriteRenderer sRenderer = room.GetComponent<SpriteRenderer>();
            RoomData roomData = room.GetComponent<Room>().GetPhaseData();


            if (roomData.defaultBackground != null && roomData.otherBackground)
            {
                Sprite sprite = isLightOn? roomData.defaultBackground: roomData.otherBackground;
                sRenderer.sprite = sprite;
            }  
        }
    }

}
