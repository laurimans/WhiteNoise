using UnityEngine;

public class AnimationOpacity : MonoBehaviour
{
    private SpriteRenderer sRenderer;
    private bool isNight = true;

    [SerializeField] [Range(0f, 1f)] private float opacity = 1; 

    private void Awake()
    {
        sRenderer = GetComponent<SpriteRenderer>();
        GameManager.OnPhaseChanged += HandlePhaseChange;
        UpdateVisuals();
    }

    private void OnDestroy()
    {
        GameManager.OnPhaseChanged -= HandlePhaseChange;
    }

    private void OnEnable()
    {
        UpdateVisuals();
    }

    private void HandlePhaseChange(GamePhase gamePhase)
    {
        isNight = gamePhase.ToString().Contains("Night");
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        Color newColor = sRenderer.color;
        newColor.a = isNight? opacity : 1;
        sRenderer.color = newColor;
    }

}
