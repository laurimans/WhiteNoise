using UnityEngine;

public class JournalUI : MonoBehaviour
{
    [SerializeField] private GameObject HUDPanel;

    private void Start()
    {
        this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        this.gameObject.SetActive(true);
        HUDPanel.SetActive(false);
    }

    private void OnDisable()
    {
        this.gameObject.SetActive(false);
        HUDPanel.SetActive(true);
    }

    public void QuitJournal()
    {
        this.gameObject.SetActive(false);
        HUDPanel.SetActive(true);
    }

    public void NextPage()
    {

    }

    public void PreviousPage()
    {

    }

    public void OpenJornal()
    {
        this.gameObject.SetActive(true);
    }
}
