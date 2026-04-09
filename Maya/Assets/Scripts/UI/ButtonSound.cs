using UnityEngine;
using UnityEngine.UI;

public enum UISoundType { 
    Click, 
    Page 
}
public class ButtonSound : MonoBehaviour
{
    [SerializeField] private UISoundType soundToPlay;

    void Start()
    {
        Button btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(PlaySound);
        }
    }

    public void PlaySound()
    {
        if (AudioManager.Instance == null) return;

        if (soundToPlay == UISoundType.Click)
            AudioManager.Instance.PlayClickButton();
        else
            AudioManager.Instance.PlayTurnPageButton();
    }
}
