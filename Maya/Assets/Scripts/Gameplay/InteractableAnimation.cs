using UnityEngine;
using System.Collections;

public class InteractableAnimation : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private GameObject visualObject;

    [Header("Timer")]
    [SerializeField] private bool autoTurnOff = false;
    [SerializeField] private float activeDuration = 2.5f;

    private bool isOn = false;
    private bool isPlayingTimer = false;

    private void Awake()
    {
        GameManager.OnPhaseChanged += ForceTurnOffOnPhaseChange;
    }

    private void OnDestroy()
    {
        GameManager.OnPhaseChanged -= ForceTurnOffOnPhaseChange;
    }

    void OnEnable()
    {
        if (autoTurnOff)
        {
            DesactivateObject();
        }
        else
        {
            if (visualObject != null) visualObject.SetActive(isOn);
        }
    }

    private void ForceTurnOffOnPhaseChange(GamePhase phase)
    {
        StopAllCoroutines();
        DesactivateObject();
    }

    public void ToggleAnimation()
    {
        isOn = !isOn;
        if (visualObject != null) visualObject.SetActive(isOn);

        if (isOn && autoTurnOff)
        {
            StartCoroutine(AutoTurnOffRoutine());
        }
    }

    private IEnumerator AutoTurnOffRoutine()
    {
        isPlayingTimer = true;

        yield return new WaitForSeconds(activeDuration);

        DesactivateObject();

        isPlayingTimer = false;
    }

    public void SetActive(bool state)
    {
        isOn = state;
        if (visualObject != null) visualObject.SetActive(state);
    }

    public void DesactivateObject()
    {
        if (visualObject != null)
        {
            visualObject.SetActive(false);
            isOn = false;
            isPlayingTimer = false;
        }
    }
}
