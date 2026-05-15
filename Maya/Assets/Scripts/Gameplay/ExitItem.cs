using System;
using Unity.VisualScripting;
using UnityEngine;

public class ExitItem : InteractableObject
{
    [SerializeField] private bool isLocked = true;
    public static event Action OnExitClicked;

    protected override void OnEnable()
    {
        base.OnEnable();

        if(GameManager.Instance != null)
            isLocked = GameManager.Instance.IsExitLocked();
    }

    public override void OnObjectClicked()
    {
        if (isLocked)
        {
            base.OnObjectClicked();
            
        } else
        {
            Debug.Log("Cambiando de fase");
            OnExitClicked?.Invoke();
        }
    }
}
