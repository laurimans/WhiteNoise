using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "NewMusicAction", menuName = "Actions/Music")]
public class ToggleMusicAction : InteractableAction
{
    private bool isPlaying = false;
    [SerializeField] private AudioClip music;

    public override bool Execute(InteractableObject owner)
    {
        InteractableData data = owner.GetPhaseData();
        AudioSource audioSource = owner.GetAudioSource();

        if (audioSource == null || music == null)
        {
            Debug.Log($"El objeto {owner.GetID()} no tiene AudioSource o AudioClip");
            return true;
        }

        if (isPlaying)
        {
            audioSource.Stop();
            isPlaying = false;
        }
        else
        {
            audioSource.clip = music;
            audioSource.Play();
            isPlaying = true;
        }

        return true;
    }
}
