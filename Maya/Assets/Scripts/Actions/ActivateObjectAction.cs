using UnityEngine;

[CreateAssetMenu(fileName = "NewActivateObjectAction", menuName = "Actions/ActivateObject")]
public class ActivateObjectAction : InteractableAction
{
    public override bool Execute(InteractableObject owner)
    {
        GameObject target = owner.GetTargetObject();
        
        if (target != null)
        {
            InteractableObject targetScript = target.GetComponent<InteractableObject>();

            if (targetScript != null)
            {
                targetScript.MarkAsInteracted();
                target.SetActive(true);
            }

            owner.MarkAsInteracted();
            owner.gameObject.SetActive(false);

        } else
        {
            Debug.Log($"El objeto {owner.name} no tiene objetivo para activar");
        }

        return true;
    }
}
