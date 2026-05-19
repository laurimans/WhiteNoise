using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject PauseMenuPanel;
    [SerializeField] private GameObject HUDPanel;
    [SerializeField] private GameObject cameraButton;
    [SerializeField] private GameObject journalButton;

    private void OnEnable()
    {
        cameraButton.SetActive(false);
        PickUpCameraAction.OnCameraPicked += ActivateCameraButton;
        PickUpJournalAction.OnPickUpJournal += ActivateJournalButton;
        GameStateManager.OnGamePause += ShowPauseMenu;
        GameStateManager.OnGameResume += HidePauseMenu;
    }

    private void OnDisable()
    {
        PickUpCameraAction.OnCameraPicked -= ActivateCameraButton;
        PickUpJournalAction.OnPickUpJournal -= ActivateJournalButton;

        GameStateManager.OnGamePause -= ShowPauseMenu;
        GameStateManager.OnGameResume -= HidePauseMenu;
    }

    private void ShowPauseMenu()
    {
        PauseMenuPanel.SetActive(true);
        HUDPanel.SetActive(false);
    }

    private void HidePauseMenu()
    {
        PauseMenuPanel.SetActive(false);
        HUDPanel.SetActive(true);
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
