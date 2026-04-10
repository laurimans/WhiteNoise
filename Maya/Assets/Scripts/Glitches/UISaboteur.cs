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

    void Awake()
    {
        GameManager.OnPhaseChanged += CheckForGlitchPhase;
    }

    void CheckForGlitchPhase(GamePhase currentPhase)
    {
        if (currentPhase == GamePhase.ThursdayMorning)
        {
            ApplyUISwap();
        }
    }

    void ApplyUISwap()
    {
        isGlitched = true;

        // Intercambiar  listeners
        journalButton.onClick.RemoveAllListeners();
        pauseButton.onClick.RemoveAllListeners();

        journalButton.onClick.AddListener(() => pauseMenu.PauseFromHUD());
        pauseButton.onClick.AddListener(() => journalUI.OpenJournal());

        // Rotar boton
        journalIcon.localRotation = Quaternion.Euler(0, 0, 180f);

        Debug.Log("UI Corrupta: Botones intercambiados.");
    }

    void Update()
    {
        if (isGlitched)
        {
            if (Random.value > 0.99f)
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
