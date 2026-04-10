using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PhoneUI : MonoBehaviour
{
    [SerializeField] private Transform chatContent;
    [SerializeField] private GameObject mayaBubblePrefab;
    [SerializeField] private GameObject momBubblePrefab;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private Button closeButton;

    public void AddMessage(string text, bool isMaya)
    {
        GameObject prefab = isMaya ? mayaBubblePrefab : momBubblePrefab;
        GameObject newBubble = Instantiate(prefab, chatContent);
        newBubble.GetComponentInChildren<TextMeshProUGUI>().text = text;

        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
    }

    public void SetCloseButton(bool active) => closeButton.gameObject.SetActive(active);
}

