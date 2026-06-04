using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;
using System;

public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject PauseMenuPanel;
    [SerializeField] private GameObject HUDPanel;
    [SerializeField] private GameObject journalUI;
    [SerializeField] private GameObject cameraButton;
    [SerializeField] private GameObject journalButton;

    public Action OnPauseButtonAction;

    private void Start()
    {
        if (cameraButton != null) cameraButton.SetActive(false);
        if (journalButton != null) journalButton.SetActive(false);

        PauseMenuPanel.SetActive(false);
        OnPauseButtonAction = OnPauseButtonPressed;
    }

    private void OnEnable()
    {
        PickUpCameraAction.OnCameraPicked += ActivateCameraButton;
        PickUpJournalAction.OnPickUpJournal += ActivateJournalButton;
        GameStateManager.OnGamePause += ShowPauseMenu;
        GameStateManager.OnGameResume += HidePauseMenu;
        GameManager.OnExitUnlock += HideCameraButton;
    }

    private void OnDisable()
    {
        PickUpCameraAction.OnCameraPicked -= ActivateCameraButton;
        PickUpJournalAction.OnPickUpJournal -= ActivateJournalButton;

        GameStateManager.OnGamePause -= ShowPauseMenu;
        GameStateManager.OnGameResume -= HidePauseMenu;
        GameManager.OnExitUnlock -= HideCameraButton;
    }

    private void HideCameraButton()
    {
        cameraButton.SetActive(false);
    }

    private void ShowPauseMenu()
    {
        PauseMenuPanel.SetActive(true);
        HUDPanel.SetActive(false);
    }

    private void HidePauseMenu()
    {
        PauseMenuPanel.SetActive(false);

        if (journalUI != null && !journalUI.gameObject.activeInHierarchy)
        {
            HUDPanel.SetActive(true);
        }
        else if (journalUI == null)
        {
            HUDPanel.SetActive(true);
        }
    }

    public void OnPauseButtonClicked()
    {
        OnPauseButtonAction?.Invoke();
    }

    public void OnPauseButtonPressed()
    {
        GameStateManager.Instance.TogglePause();
    }

    public void Quit()
    {
        GameStateManager.Instance.ReturnToMenu();
    }

    public void ActivateCameraButton()
    {
        cameraButton.SetActive(true);
    }

    public void ActivateJournalButton()
    {
        journalButton.SetActive(true);
    }
}
