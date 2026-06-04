using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PhoneUI : MonoBehaviour
{
    [SerializeField] private Transform chatContent;
    [SerializeField] private GameObject mayaBubblePrefab;
    [SerializeField] private GameObject momBubblePrefab;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private Button closeButton;

    private void OnDisable()
    {
        ClearChat();
    }

    public void AddMessage(string text, bool isMaya)
    {
        GameObject prefab = isMaya ? mayaBubblePrefab : momBubblePrefab;
        GameObject newBubble = Instantiate(prefab, chatContent);
        newBubble.GetComponentInChildren<TextMeshProUGUI>().text = text;

        newBubble.transform.localScale = Vector3.one;

        LayoutRebuilder.ForceRebuildLayoutImmediate(chatContent.GetComponent<RectTransform>());

        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;

        StartCoroutine(ScrollToBottom());
    }

    private IEnumerator ScrollToBottom()
    {
        yield return new WaitForEndOfFrame();

        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
    }

    public void SetCloseButton(bool active) => closeButton.gameObject.SetActive(active);

    public void ClearChat()
    {
        foreach (Transform child in chatContent)
        {
            Destroy(child.gameObject);
        }
    }

}

