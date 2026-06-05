using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewActivateObjectAction", menuName = "Actions/ActivateObject")]
public class ActivateObjectAction : InteractableAction
{
    [SerializeField] private float delayTime = 0;
    public override bool Execute(InteractableObject owner)
    {
        GameObject target = owner.GetTargetObject();
        
        if (target != null)
        {
            InteractableObject targetScript = target.GetComponent<InteractableObject>();

            owner.StartCoroutine(Delay(targetScript, owner));
        } else
        {
            Debug.Log($"El objeto {owner.name} no tiene objetivo para activar");
        }

        return true;
    }

    IEnumerator Delay(InteractableObject targetScript, InteractableObject owner)
    {
        yield return new WaitForSeconds(delayTime);

        owner.MarkAsInteracted();
        owner.gameObject.SetActive(false);

        if (targetScript != null)
        {
            targetScript.MarkAsInteracted();
            targetScript.gameObject.SetActive(true);
        }

    }
}
