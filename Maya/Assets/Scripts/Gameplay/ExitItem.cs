using UnityEngine;

public class ExitItem : InteractableObject
{
    public override void OnObjectClicked()
    {
        if (GameManager.Instance.CanFinishPhase())
        {
            Debug.Log("Cambiando de fase");
            GameManager.Instance.NextPhase();

        } else
        {
            base.OnObjectClicked();
        }
    }
}
