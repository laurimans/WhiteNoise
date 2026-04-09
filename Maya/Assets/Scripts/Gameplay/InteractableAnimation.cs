using UnityEngine;

public class InteractableAnimation : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private GameObject visualObject;
    private bool isOn = false;

    void OnEnable()
    {
        if (visualObject != null)
        {
            visualObject.SetActive(isOn);
        }
    }

    public void ToggleAnimation()
    {
        isOn = !isOn;
        if (visualObject != null) visualObject.SetActive(isOn);
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
        }
    }
}
