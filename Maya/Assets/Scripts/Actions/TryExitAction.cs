using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NewExitAction", menuName = "Actions/Exit")]
public class TryExitAction : InteractableAction
{
    public static event Action OnExitSucess;

    public override bool Execute(InteractableObject owner)
    {
        if(GameManager.Instance.IsExitLocked() == true)
        {
            Debug.Log("Salida bloqueada.");
            var data = owner.GetPhaseData();

            return true;
        } 

        Debug.Log("Cambiando de fase");
        OnExitSucess?.Invoke();
        return false; // Para ejecución de acciones
    }

}
