using UnityEngine;

[CreateAssetMenu(fileName = "NewToggleLightAction", menuName = "Actions/ToggleLight")]
public class ToggleRoomLightAction : InteractableAction
{
    public override bool Execute(InteractableObject owner)
    {
        GameObject roomGO = owner.GetTargetObject();

        if (roomGO != null)
        {
            Room room = roomGO.GetComponent<Room>();

            if (room != null)
            {
                room.ToggleLight();
            }
        }

        return true;
    }
}
