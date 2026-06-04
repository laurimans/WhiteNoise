using UnityEngine;

public class RoomLightGlitch : MonoBehaviour
{
    private SpriteRenderer sRenderer;
    private Room roomScript;

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
        roomScript.ToggleLight();

    }

}
