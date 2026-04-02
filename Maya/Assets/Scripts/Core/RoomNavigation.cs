using System.Collections.Generic;
using UnityEngine;

public class RoomNavigation : MonoBehaviour
{
    [SerializeField] private List<GameObject> rooms;
    private int currentIndex = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentIndex = 0;
        ShowRoom(currentIndex);
    }

    public void NextRoom()
    {
        currentIndex = (currentIndex + 1) % rooms.Count;
        ShowRoom(currentIndex);
    }

    public void PreviousRoom()
    {
        currentIndex = (currentIndex - 1 + rooms.Count) % rooms.Count;
        ShowRoom(currentIndex);
    }

    private void ShowRoom(int index)
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            rooms[i].SetActive(i == index);
        }

        Debug.Log("Has entrado en: " + rooms[index].name);
    }
}
