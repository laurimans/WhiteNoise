using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ConfirmationUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private GameObject hudPanel;

    private Action onConfirmAction;

    private void Start()
    {
        panel.SetActive(false);
    }

    public void ShowPanel(string message, Action onConfirm)
    {
        messageText.text = message;
        onConfirmAction = onConfirm;

        if (hudPanel != null) hudPanel.SetActive(false);

        panel.SetActive(true);
    }

    public void OnYesClicked()
    {
        panel.SetActive(false);
        onConfirmAction?.Invoke(); 
    }

    public void OnNoClicked()
    {
        panel.SetActive(false);
        if (hudPanel != null) hudPanel.SetActive(true);
    }
}
