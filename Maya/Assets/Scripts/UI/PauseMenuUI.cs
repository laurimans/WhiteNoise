using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject PauseMenuPanel;
    [SerializeField] private GameObject HUDPanel;
    [SerializeField] private GameObject JournalPanel;

    private bool comeFromHUD;

    public void PauseFromHUD()
    {
        PauseMenuPanel.SetActive(true);
        HUDPanel.SetActive(false);
        comeFromHUD = true;
    }

    public void PauseFromJournal()
    {
        PauseMenuPanel.SetActive(true);
        comeFromHUD = false;
    }

    public void Return()
    {
        PauseMenuPanel.SetActive(false);
        if( comeFromHUD)
        {
            HUDPanel.SetActive(true);
        }
        else
        {
            JournalPanel.SetActive(true);
        }
    }

    public void Quit()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
