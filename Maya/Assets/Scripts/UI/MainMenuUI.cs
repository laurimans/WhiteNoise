using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject MainMenuPanel;
    [SerializeField] private GameObject SettingsPanel;
    [SerializeField] private GameObject CreditsPanel;

    private void Start()
    {
        SettingsPanel.SetActive(false);
        CreditsPanel.SetActive(false);
        MainMenuPanel.SetActive(true);
    }

    public void NewGame()
    {
        GameStateManager.Instance.StartGame();
    }

    public void Credits()
    {
        MainMenuPanel.SetActive(false);
        CreditsPanel.SetActive(true);
    }

    public void Settings()
    {
        MainMenuPanel.SetActive(false);
        SettingsPanel.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void ReturnToMenu()
    {
        SettingsPanel.SetActive(false);
        CreditsPanel.SetActive(false);
        MainMenuPanel.SetActive(true);
    }
}
