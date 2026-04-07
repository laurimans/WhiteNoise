using UnityEngine;

public class InteractableMusic : InteractableObject
{
    [SerializeField] private bool isPlaying;
    public override void OnObjectClicked()
    {

        if (isPlaying)
        {
            AudioManager.Instance.StopMusic();
            isPlaying = false;
        } else 
        {
            AudioManager.Instance.PlayMusic(GetPhaseData().sound);
            isPlaying = true;
        }     
    }
}
