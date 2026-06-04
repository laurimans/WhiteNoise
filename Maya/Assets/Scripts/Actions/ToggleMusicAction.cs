using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "NewMusicAction", menuName = "Actions/Music")]
public class ToggleMusicAction : InteractableAction
{
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

        if (audioSource.isPlaying && audioSource.clip == music)
        {
            audioSource.Stop();
        }
        else
        {
            audioSource.clip = music;
            audioSource.Play();
        }

        return true;
    }
}
