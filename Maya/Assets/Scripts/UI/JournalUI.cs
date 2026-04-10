using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct Photo
{
    public string idRoom;
    public Sprite normalPhoto;
    public Sprite changedPhoto;
}

public class JournalUI : MonoBehaviour
{
    [SerializeField] private GameObject HUDPanel;
    [SerializeField] private JournalManager journal;
    [SerializeField] private GameObject journalPanel;

    [Header("Journal Texts")]
    [SerializeField] private TextMeshProUGUI dateText;
    [SerializeField] private TextMeshProUGUI bodyText;

    [Header("Journal Buttons")]
    [SerializeField] private Button nextButton;
    [SerializeField] private Button prevButton;

    [Header("Photo System")]
    [SerializeField] private Image photoContainer; 
    [SerializeField] private List<Photo> roomPhotos; 

    private int currentIndex = 0;
    private Coroutine typewriterCoroutine;

    private void Start()
    {
        journalPanel.SetActive(false);
        HUDPanel.SetActive(true);
    }


    public void QuitJournal()
    {
        journalPanel.SetActive(false);
        HUDPanel.SetActive(true);

        if (CursorManager.Instance != null) CursorManager.Instance.SetDefaultCursor();
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

    public void OpenJournal()
    {
        journalPanel.SetActive(true);
        HUDPanel.SetActive(false);

        if (journal.GetEntriesCount() > 0)
        {
            currentIndex = journal.GetEntriesCount() - 1;
            UpdateJournalUI();
        }
    }

    public void TypeNewEntry(string date, string content)
    {
        journalPanel.SetActive(true); 
        dateText.text = date;

        if (typewriterCoroutine != null) StopCoroutine(typewriterCoroutine);
        typewriterCoroutine = StartCoroutine(TypeSentece(content));
    }

    IEnumerator TypeSentece(string sentence)
    {
        bodyText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            bodyText.text += letter;
            yield return new WaitForSeconds(0.05f);
        }

        UpdateJournalUI();
    }

    public void TypeNewClue(string newText)
    {
        journalPanel.SetActive(true);
        if (typewriterCoroutine != null) StopCoroutine(typewriterCoroutine);
        typewriterCoroutine = StartCoroutine(AppendText(newText));
    }

    IEnumerator TypeSentenceWithGlitch(string sentence)
    {
        bodyText.text = "";
        string characters = "!@#$%^&*()_+=<>?/";

        for (int i = 0; i < sentence.Length; i++)
        {
            if (GameManager.Instance.GetCurrentPhase() == GamePhase.WednesdayMorning && i > sentence.Length / 2)
            {
                bodyText.text += characters[Random.Range(0, characters.Length)];
            }
            else
            {
                bodyText.text += sentence[i];
            }
            yield return new WaitForSeconds(0.05f);
        }
    }

    IEnumerator AppendText(string textToAppend)
    {
        foreach (char letter in textToAppend.ToCharArray())
        {
            bodyText.text += letter;
            yield return new WaitForSeconds(0.04f);
        }
    }

    private void UpdateJournalUI()
    {
        JournalEntry entry = journal.GetEntry(currentIndex);
        if (entry == null) return;

        dateText.text = entry.GetDate();
        bodyText.text = entry.GetBody();

        if (entry.GetDate().Contains("Foto:"))
        {
            photoContainer.gameObject.SetActive(true);
            string roomID = entry.GetDate().Replace("Foto: ", "");

            ShowCorrectPhoto(roomID);
        }
        else
        {
            photoContainer.gameObject.SetActive(false);
        }

        int totalEntries = journal.GetEntriesCount();
        nextButton.interactable = currentIndex < totalEntries - 1;
        prevButton.interactable = currentIndex > 0;
    }

    public void ShowCorrectPhoto(string roomID)
    {
        bool found = false;
        foreach (var photoObj in roomPhotos)
        {
            if (photoObj.idRoom.Contains(roomID))
            {             
                if (GameManager.Instance.GetCurrentPhase() == GamePhase.ThursdayMorning)
                {
                    photoContainer.sprite = photoObj.changedPhoto;
                } else
                {
                    photoContainer.sprite = photoObj.normalPhoto;
                }

                found = true;
                break;
            }
        }

        photoContainer.gameObject.SetActive(found);
    }

    public void OpenWithPhoto(string roomID)
    {
        journal.AddEntry("Foto: " + roomID, "Evidencia recogida.");
        
        OpenJournal();
    }

}
