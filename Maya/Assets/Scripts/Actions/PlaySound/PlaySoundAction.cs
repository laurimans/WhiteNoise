using UnityEngine;

[CreateAssetMenu(fileName = "NewSoundAction", menuName = "Actions/Sound")]
public class PlaySoundAction : InteractableAction
{
    public AudioClip soundClip;

    public override bool Execute(InteractableObject owner)
    {
        if (soundClip == null) return true;

        AudioSource localSource = owner.GetComponent<AudioSource>();

        if (localSource != null)
        {
            AudioManager.Instance.Play3DSFX(soundClip, localSource);
        }
        else
        {
            AudioManager.Instance.PlaySFX(soundClip);
        }
        return true;
    }
}
