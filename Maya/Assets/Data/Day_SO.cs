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
    public GameObject item;
    [TextArea] public string dialogue;
    public bool isTask;
    public bool isClue;
}
