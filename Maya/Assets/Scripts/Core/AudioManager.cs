using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource ambientSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;

    [SerializeField] private AudioMixer mixer;


    #region Singleton
    public static AudioManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    #endregion

    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (musicSource.clip == clip) return;
        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.Play();
    }

    public void PlayRoomAmbience(AudioClip clip)
    {
        //if (ambientSource.clip == clip) return;
        ambientSource.clip = clip;
        ambientSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;

        sfxSource.PlayOneShot(clip);
    }

    public void PlayDialogueSFX(AudioClip clip, bool randomizePitch = true)
    {
        if (clip == null) return;

        if (randomizePitch)
        {
            sfxSource.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
        }
        else
        {
            sfxSource.pitch = 1f;
        }

        sfxSource.PlayOneShot(clip);
    }

    public void Play3DSFX(AudioClip clip, Vector3 position)
    {
        if (clip == null) return;
        AudioSource.PlayClipAtPoint(clip, position);
    }

    public void UpdateRoomContext(RoomData_SO roomData)
    {
        AudioMixerSnapshot s = mixer.FindSnapshot("Snapshot_" + roomData.roomID);
        if (s != null) s.TransitionTo(0f);
    }
}
