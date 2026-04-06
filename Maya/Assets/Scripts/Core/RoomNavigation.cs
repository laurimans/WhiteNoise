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
        for (int i = 0; i < roomList.Count; i++)
        {
            roomList[i].SetActive(i == index);
        }

        RoomData roomData = roomList[index].GetComponent<Room>().GetPhaseData();
        string roomID = roomList[index].GetComponent<Room>().GetID();

        if (roomData != null)
        {
            AudioManager.Instance.UpdateRoomContext(roomData, roomID);
            //Debug.Log("Has entrado en: " + roomList[index].name);
        }
    }
}