using UnityEngine;

[CreateAssetMenu(fileName = "RoomData_SO", menuName = "Scriptable Objects/RoomData_SO")]
public class RoomData_SO : ScriptableObject
{
    public string roomID;
    public AudioClip ambientClip;
}
