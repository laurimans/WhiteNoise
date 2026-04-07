using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource ambientSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;

    [SerializeField] private AudioMixer mixer;
    [SerializeField] private AudioSource[] audioRooms;

    [Header("Efectos de sonido")]
    [SerializeField] private AudioClip clickButtonClip;
    [SerializeField] private AudioClip pageJournalClip;

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

    public void PlayClickButton()
    {
        PlayUISound(clickButtonClip);
    }

    public void PlayTurnPageButton()
    {
        PlayUISound(pageJournalClip);
    }

    private void PlayUISound(AudioClip clip)
    {
        sfxSource.clip = clip;
        sfxSource.loop = false;
        sfxSource.Play();
    }

    public void SetupDayAmbience(Room[] rooms, GamePhase gamePhase)
    {
        for (int i = 0; i < audioRooms.Length; i++)
        {
            if (i >= rooms.Length || rooms[i] == null) continue;

            RoomData data = rooms[i].GetRoomDataAt((int)gamePhase);

            if (data != null && data.ambientClip != null)
            {
                audioRooms[i].clip = data.ambientClip;
                audioRooms[i].loop = true;
                audioRooms[i].Play();
            } else
            {
                audioRooms[i].clip = null;
                audioRooms[i].Stop();
            }
        }
    }
    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (musicSource.clip == clip) return;
        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.Play();
    }

    public void StopMusic () 
    { 
        musicSource.Stop();
        musicSource.clip = null;
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

    public void UpdateRoomContext(string roomID)
    {
        AudioMixerSnapshot s = mixer.FindSnapshot("Snapshot_" + roomID);
        if (s != null) s.TransitionTo(0f);
    }
}
