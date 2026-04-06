using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject PauseMenuPanel;
    [SerializeField] private GameObject HUDPanel;

    public void Pause()
    {
        PauseMenuPanel.SetActive(true);
        HUDPanel.SetActive(false); 
    }

    public void Return()
    {
        PauseMenuPanel.SetActive(false);
        HUDPanel.SetActive(true);
    }

    public void Quit()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
