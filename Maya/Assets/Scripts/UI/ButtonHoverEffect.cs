using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private RectTransform textTransform;
    [SerializeField] private Vector2 offset = new Vector2(5f, -5f);

    private Vector2 originalPosition;
    private bool isInitialized = false;

    void Awake()
    {
        if (isInitialized) return;

        if (textTransform == null)
            textTransform = GetComponentInChildren<TextMeshProUGUI>()?.rectTransform;

        if (textTransform != null)
        {
            originalPosition = textTransform.anchoredPosition;
            isInitialized = true;
        }
    }

    void OnDisable()
    {
        ResetPosition();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (textTransform != null)
        {
            textTransform.anchoredPosition = originalPosition + offset;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ResetPosition();
    }

    private void ResetPosition()
    {
        if (textTransform != null && isInitialized)
        {
            textTransform.anchoredPosition = originalPosition;
        }
    }
}
    

