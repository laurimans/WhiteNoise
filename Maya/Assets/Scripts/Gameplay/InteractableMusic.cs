using UnityEngine;

public class InteractableMusic : InteractableObject
{
    private bool isPlaying;

    protected override void Start()
    {
        audioSource.loop = true;
        audioSource.playOnAwake = false;
    }

    public override void OnObjectClicked()
    {
        InteractableData data = GetPhaseData();

        if (isPlaying)
        {
            audioSource.Stop();
            isPlaying = false;
        } else 
        {
            if (data.sound != null)
            {
                audioSource.clip = data.sound;
                audioSource.Play();
                isPlaying = true;
            }
        }

        //HandleSpriteChange(data);
    }
}
