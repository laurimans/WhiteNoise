using UnityEngine;

public abstract class InteractableAction : ScriptableObject
{
    public abstract bool Execute(InteractableObject owner);
}
