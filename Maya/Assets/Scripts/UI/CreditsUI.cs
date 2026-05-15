using UnityEngine;

public class CreditsUI : MonoBehaviour
{
    [SerializeField] private GameObject creditsPanel;

    private void Start()
    {
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

}
