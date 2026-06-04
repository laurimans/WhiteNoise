using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InteractableData", menuName = "Scriptable Objects/InteractableData")]
public class InteractableData : ScriptableObject
{
    [Header("Visual")]
    public Sprite initialSprite;
    public Sprite darkSprite;
    public Sprite otherSprite;
    public bool startDisable;
    public bool activateOtherItem;

    [Header("Phase Actions")]
    public List<InteractableAction> actions;
}
