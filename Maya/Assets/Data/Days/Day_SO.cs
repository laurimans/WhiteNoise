using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Day_SO", menuName = "Scriptable Objects/Day_SO")]
public class Day_SO : ScriptableObject
{
    public int id_day;
    public int tasksNumber;
    public int cluesNumber;

    public List<ItemBehaviour> itemBehaviour;
}


[System.Serializable]
public class ItemBehaviour
{
    public string idItem;
    [TextArea] public string dialogue;

    [Header("Object Type")]
    public bool isTask;
    public bool isClue;
    public bool isDayStateChanger;

    [Header("Effects")]
    public AudioClip soundEffect;
    public Sprite specialSprite;
}
