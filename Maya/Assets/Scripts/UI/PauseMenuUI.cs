using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject PauseMenuPanel;
    [SerializeField] private GameObject HUDPanel;

    private void OnEnable()
    {

        GameStateManager.OnGamePause += ShowPauseMenu;
        GameStateManager.OnGameResume += HidePauseMenu;
        
        
    }

    private void OnDisable()
    {
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
}
