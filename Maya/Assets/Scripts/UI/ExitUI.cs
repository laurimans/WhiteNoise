using UnityEngine;

public class ExitUI : MonoBehaviour
{
    public void ReturnToPause()
    {
        this.gameObject.SetActive(false);
    }

    public void ReturnToMenu()
    {
        GameStateManager.Instance.ReturnToMenu();
    }
}
