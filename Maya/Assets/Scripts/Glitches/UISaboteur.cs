using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UISaboteur : MonoBehaviour
{
    [Header("Buttons to Swap")]
    [SerializeField] private Button journalButton;
    [SerializeField] private Button pauseButton;

    [Header("Visual Effects")]
    [SerializeField] private CanvasGroup mainHUD;
    [SerializeField] private RectTransform journalIcon;

    [Header("Menus")]
    [SerializeField] private PauseMenuUI pauseMenu;
    [SerializeField] private JournalUI journalUI;

    private bool isGlitched = false;


    private void Start()
    {
        journalButton.onClick.RemoveAllListeners();
        pauseButton.onClick.RemoveAllListeners();

        journalButton.onClick.AddListener(OnJournalClick);
        pauseButton.onClick.AddListener(OnPauseClick);
    }

    public void EnableUISabotage()
    {
        isGlitched = true;
        journalIcon.localRotation = Quaternion.Euler(0, 0, 180f);
    }

    public void DisableUISabotage()
    {
        isGlitched = false;
        journalIcon.localRotation = Quaternion.Euler(0, 0, 0f);
        mainHUD.alpha = 1f;
    }

    void OnJournalClick()
    {
        if (isGlitched) pauseMenu.OnPauseButtonPressed();
        else journalUI.OpenJournal();
    }

    void OnPauseClick()
    {
        if (isGlitched) journalUI.OpenJournal();
        else pauseMenu.OnPauseButtonPressed();
    }

    void Update()
    {
        if (isGlitched)
        {
            if (Random.value > 0.995f)
            {
                StartCoroutine(FlashUI());
            }
        }
    }

    // Parpadeo de UI
    IEnumerator FlashUI()
    {
        mainHUD.alpha = 0.5f;
        yield return new WaitForSeconds(0.1f);
        mainHUD.alpha = 1f;
    }
}
