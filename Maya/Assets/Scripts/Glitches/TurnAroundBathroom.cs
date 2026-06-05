using UnityEngine;

public class TurnAroundBathroom : MonoBehaviour
{

    private bool isThursday = false;

    private void Awake()
    {
        GameManager.OnPhaseChanged += HandlePhaseChange;
        UpdateVisuals();
    }

    private void OnDestroy()
    {
        GameManager.OnPhaseChanged -= HandlePhaseChange;
    }

    private void OnEnable()
    {
        UpdateVisuals();
    }

    private void HandlePhaseChange(GamePhase gamePhase)
    {
        isThursday = gamePhase == GamePhase.ThursdayMorning;
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        if (isThursday)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);

        } else
        {
            transform.rotation = Quaternion.identity;
        }
    }
}
