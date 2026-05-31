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
    [SerializeField] private GameObject journalCanvas;

    [Header("Journal Texts")]
    [SerializeField] private TextMeshProUGUI dateText;
    [SerializeField] private TextMeshProUGUI bodyText;

    [Header("Journal Buttons")]
    [SerializeField] private Button nextButton;
    [SerializeField] private Button prevButton;

    [Header("Photo System")]
    [SerializeField] private Image photoContainer; 
    [SerializeField] private List<Photo> roomPhotos;

    [Header("Animation Settings")]
    [SerializeField] private RectTransform journalPanelRect;
    [SerializeField] private float animationDuration = 0.4f;
    [SerializeField] private Vector2 hiddenPosition = new Vector2(0, -1500f); 
    [SerializeField] private Vector2 visiblePosition = Vector2.zero;
    [SerializeField] private AnimationCurve animationCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    private int currentIndex = 0;
    private Coroutine typewriterCoroutine;
    private Coroutine animationCoroutine;

    private void Start()
    {
        journalCanvas.SetActive(false);
        HUDPanel.SetActive(true);
    }

    private void OnEnable()
    {
        OpenAnimation();
        OpenJournalAction.OnJournalClicked += OpenJournal;
    }

    private void OnDisable()
    {
        CloseAnimation();
        OpenJournalAction.OnJournalClicked -= OpenJournal;
    }

    public void QuitJournal()
    {
        CloseAnimation();

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

    private void OpenAnimation()
    {
        if (animationCoroutine != null) StopCoroutine(animationCoroutine);

        if (!journalCanvas.activeSelf)
        {
            journalPanelRect.anchoredPosition = hiddenPosition;
        }

        animationCoroutine = StartCoroutine(SlidePanel(visiblePosition, false));
    }

    private void CloseAnimation()
    {
        if (animationCoroutine != null) StopCoroutine(animationCoroutine);

        animationCoroutine = StartCoroutine(SlidePanel(hiddenPosition, true));
    }

    private IEnumerator SlidePanel(Vector2 targetPosition, bool isClosing)
    {
        float elapsedTime = 0;
        Vector2 startPosition = journalPanelRect.anchoredPosition;

        while (elapsedTime < animationDuration)
        {
            float t = elapsedTime / animationDuration;

            float curveValue = animationCurve.Evaluate(t);

            journalPanelRect.anchoredPosition = Vector2.LerpUnclamped(startPosition, targetPosition, curveValue);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        journalPanelRect.anchoredPosition = targetPosition;

        if (isClosing)
        {
            journalCanvas.SetActive(false);
            HUDPanel.SetActive(true);
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
        journalCanvas.SetActive(true);
        HUDPanel.SetActive(false);

        OpenAnimation();

        if (journal.GetEntriesCount() > 0)
        {
            currentIndex = journal.GetEntriesCount() - 1;
            UpdateJournalUI();
        }
    }

    public void TypeNewEntry(string date, string content)
    {
        journalCanvas.SetActive(true);
        HUDPanel.SetActive(false);
        OpenAnimation();

        if (journal.GetEntriesCount() > 0)
        {
            currentIndex = journal.GetEntriesCount() - 1;
            UpdateJournalUI();
        }

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
        journalCanvas.SetActive(true);
        HUDPanel.SetActive(false);
        OpenAnimation();

        if (journal.GetEntriesCount() > 0)
        {
            currentIndex = journal.GetEntriesCount() - 1;

            UpdateJournalUI();

            string fullText = journal.GetEntry(currentIndex).GetBody();
            bodyText.text = fullText.Substring(0, fullText.Length - newText.Length);
        }

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

    public void OpenWithPhoto(RoomID roomID)
    {
        journal.AddEntry("Foto: " + roomID.ToString(), "Evidencia recogida.");
        
        OpenJournal();
    }

}
