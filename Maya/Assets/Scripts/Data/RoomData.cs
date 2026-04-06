using UnityEngine;

[CreateAssetMenu(fileName = "RoomData", menuName = "Scriptable Objects/RoomData")]
public class RoomData : ScriptableObject
{
    public AudioClip ambientClip;

    public Sprite defaultBackground;
    public Sprite otherBackgroung;
}
