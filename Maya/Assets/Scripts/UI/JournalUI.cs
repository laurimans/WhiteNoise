using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JournalUI : MonoBehaviour
{
    [SerializeField] private GameObject HUDPanel;
    [SerializeField] private JournalManager journal;

    [Header("Journal Texts")]
    [SerializeField] private TextMeshProUGUI dateText;
    [SerializeField] private TextMeshProUGUI bodyText;

    [Header("Journal Buttons")]
    [SerializeField] private Button nextButton;
    [SerializeField] private Button prevButton;

    private int currentIndex = 0;

    private void Start()
    {
        this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
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
        if (currentIndex < journal.GetEntriesCount() - 1)
        {
            currentIndex++;
            UpdateJournalUI();
        }
    }

    public void PreviousPage()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            UpdateJournalUI();
        }
    }

    public void OpenJornal()
    {
        currentIndex = journal.GetEntriesCount() - 1;

        UpdateJournalUI();

        this.gameObject.SetActive(true);
    }

    private void UpdateJournalUI()
    {
        JournalEntry entry = journal.GetEntry(currentIndex);

        if (entry != null)
        {
            dateText.text = entry.GetDate();
            bodyText.text = entry.GetBody();
        }

        // Bloqueo botones
        int totalEntries = journal.GetEntriesCount();

        nextButton.interactable = currentIndex < totalEntries - 1;
        prevButton.interactable = currentIndex > 0;
    }

}
