using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private float typingSpeed = 0.05f;
    [SerializeField] private AudioClip typingSound;

    private Coroutine typingCoroutine;
    private bool isTyping = false;
    private bool isLineComplete = false;
    private string currentFullText = "";
    private float timeStarted = 0f;

    void Awake()
    {
        dialoguePanel.SetActive(false);
    }


    private void Start()
    {
        InteractableObject.OnDialogueSaid += ShowDialogue;
        DialogueAction.OnDialogueSaid += ShowDialogue;
        RoomNavigation.OnDialogueSaid += ShowDialogue;
    }

    void Update()
    {
        if (Pointer.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            OnSubmitPressed();
        }

        if (Keyboard.current.enterKey.wasPressedThisFrame || Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            OnSubmitPressed();
        }
    }

    public void ShowDialogue(string text)
    {
        if (string.IsNullOrEmpty(text)) return;

        timeStarted = Time.time;
        currentFullText = text;
        dialoguePanel.SetActive(true);
        isLineComplete = false;

        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeLine(text));
    }

    private IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogueText.text = "";

        int charCount = 0;
        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;

            if (charCount % 2 == 0 && typingSound != null)
            {
                AudioManager.Instance.PlayDialogueSFX(typingSound);
            }

            charCount++;

            yield return new WaitForSeconds(typingSpeed);
        }

        CompleteText();
    }

    private void CompleteText()
    {
        isTyping = false;
        isLineComplete = true;
        dialogueText.text = currentFullText;
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
    }

    public void OnSubmitPressed()
    {
        if (!dialoguePanel.activeSelf) return;
        if (Time.time - timeStarted < 0.1f) return;

        if (isTyping)
        {
            CompleteText();
        }
        else if (isLineComplete)
        {
            CloseDialogue();
        }
    }

    private void CloseDialogue()
    {
        dialoguePanel.SetActive(false);
        isLineComplete = false;
    }

    private void OnDestroy()
    {
        InteractableObject.OnDialogueSaid -= ShowDialogue;
        DialogueAction.OnDialogueSaid -= ShowDialogue;
        RoomNavigation.OnDialogueSaid -= ShowDialogue;
    }

}
