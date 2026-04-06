using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class RoomNavigation : MonoBehaviour
{
    private int currentIndex = 0;
    [SerializeField] private List<GameObject> roomList;
    
    void Start()
    {
        currentIndex = 0;
        ShowRoom(currentIndex);
    }

    public void NextRoom()
    {
        currentIndex = (currentIndex + 1) % roomList.Count;
        ShowRoom(currentIndex);
    }

    public void PreviousRoom()
    {
        currentIndex = (currentIndex - 1 + roomList.Count) % roomList.Count;
        ShowRoom(currentIndex);
    }

    private void ShowRoom(int index)
    {
        if (index < 0 || index >= roomList.Count || roomList[index] == null) return;

        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i] != null) roomList[i].SetActive(i == index);
        }

        Room roomComponent = roomList[index].GetComponent<Room>();

        if (roomComponent != null)
        {
            RoomData roomData = roomComponent.GetPhaseData();
            string roomID = roomComponent.GetID(); 

            if (roomData != null)
            {
                AudioManager.Instance.UpdateRoomContext(roomID);

                if(roomData.initialDialogue != null && !roomComponent.dialogueDone)
                {
                    DialogueManager.Instance.ShowDialogue(roomData.initialDialogue);
                    roomComponent.dialogueDone = true;
                }
            }
            else
            {
                Debug.LogWarning($"La habitación {roomID} no tiene RoomData para esta fase.");
            }
        }
    }
}