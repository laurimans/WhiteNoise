using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct RoomSetup
{
    public GameObject room;
    public RoomData_SO data;

}
public class RoomNavigation : MonoBehaviour
{
    private int currentIndex = 0;
    [SerializeField] private List<RoomSetup> roomList;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
            roomList[i].room.SetActive(i == index);
        }

        AudioManager.Instance.UpdateRoomContext(roomList[index].data);

        Debug.Log("Has entrado en: " + roomList[index].data.name);
    }
}

