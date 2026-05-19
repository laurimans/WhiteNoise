using UnityEngine;

[CreateAssetMenu(fileName = "RoomData", menuName = "Scriptable Objects/RoomData")]
public class RoomData : ScriptableObject
{
    public AudioClip ambientClip;

    public Sprite defaultBackground;
    public Sprite otherBackground;
    public bool lightsStartsOn = true;

    [TextArea] public string initialDialogue;
}
