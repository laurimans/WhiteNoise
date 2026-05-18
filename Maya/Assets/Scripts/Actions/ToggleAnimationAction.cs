using UnityEngine;

[CreateAssetMenu(fileName = "NewAnimationAction", menuName = "Actions/Animation")]
public class ToggleAnimationAction : InteractableAction
{
    public override bool Execute(InteractableObject owner)
    {
        InteractableAnimation anim = owner.GetComponent<InteractableAnimation>();

        if(anim != null)
        {
            anim.ToggleAnimation();
        } else
        {
            Debug.Log($"El objeto {owner.name} no tiene InteractableAnimation");
        }

        return true;
    }
}
