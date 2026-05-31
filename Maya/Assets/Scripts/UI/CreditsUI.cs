using UnityEngine;

public class CreditsUI : MonoBehaviour
{
    [SerializeField] private GameObject creditsPanel;
    public static System.Action OnReturnToMenu;
    private GameStateManager gameStateManager;

    private void Start()
    {
        gameStateManager = (GameStateManager)FindAnyObjectByType(typeof(GameStateManager));
        creditsPanel.SetActive(false);
    }

    private void OnEnable()
    {
        GameStateManager.OnGameOver += ShowCredits;
    }

    private void OnDisable()
    {
        GameStateManager.OnGameOver -= ShowCredits;
    }

    private void ShowCredits()
    {
        creditsPanel.SetActive(true);
    }

    public void OnReturnToMenuClicked()
    {
        gameStateManager.ReturnToMenu();
    }

}
