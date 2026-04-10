using UnityEngine;

public class RoomLightGlitch : MonoBehaviour
{
    private SpriteRenderer sRenderer;
    private Room roomScript;
    private bool lastIsOnState = false;

    void Awake()
    {
        sRenderer = GetComponent<SpriteRenderer>();
        roomScript = GetComponent<Room>();
    }

    void OnEnable()
    {
        LightGlitch.OnSOSLightPulse += UpdateLightState;
    }

    void OnDisable()
    {
        LightGlitch.OnSOSLightPulse -= UpdateLightState;
    }

    private void UpdateLightState(bool isOn)
    {
        //if (GameManager.Instance.GetCurrentPhase() != GamePhase.ThursdayMorning) return;

        var data = roomScript.GetPhaseData();
        sRenderer.sprite = isOn ? data.defaultBackground : data.otherBackground;

    }

}
