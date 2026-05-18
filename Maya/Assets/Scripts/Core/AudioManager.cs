using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource UISource;
    [SerializeField] private AudioSource musicSource;

    [SerializeField] private AudioMixer mixer;
    private AudioSource[] audioRooms;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip clickButtonClip;
    [SerializeField] private AudioClip pageJournalClip;
    [SerializeField] private AudioClip taskDoneClip;

    [Header("Music")]
    [SerializeField] private AudioClip menuMusicClip;

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

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }

    #endregion

    private void OnEnable()
    {
        GameManager.OnTaskDone += PlayTaskDoneSound;
    }

    private void OnDisable()
    {
        
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenuScene")
        {
            if (menuMusicClip != null)
            {
                PlayMusic(menuMusicClip, true); 
            }
        }
        else
        {
            StopMusic(); 
        }
    }

    private void PlayTaskDoneSound()
    {
        PlayUISound(taskDoneClip);
    }


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
        UISource.clip = clip;
        UISource.loop = false;
        UISource.Play();
    }

    public void RegisterRoomSources(AudioSource[] sources)
    {
        audioRooms = sources;
        Debug.Log("Fuentes de audio de las habitaciones registradas.");
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

    private void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (musicSource.clip == clip) return;
        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.Play();
    }

    private void StopMusic () 
    { 
        musicSource.Stop();
        musicSource.clip = null;
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;

        UISource.PlayOneShot(clip);
    }

    public void PlayDialogueSFX(AudioClip clip, bool randomizePitch = true)
    {
        if (clip == null) return;

        if (randomizePitch)
        {
            UISource.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
        }
        else
        {
            UISource.pitch = 1f;
        }

        UISource.PlayOneShot(clip);
    }

    public void Play3DSFX(AudioClip clip, AudioSource audioSource)
    {
        if (clip == null) return;
        if (audioSource == null) return;

        audioSource.clip = clip;
        audioSource.loop = false;
        audioSource.Play();        
    }

    public void UpdateRoomContext(string roomID)
    {
        AudioMixerSnapshot s = mixer.FindSnapshot("Snapshot_" + roomID);
        if (s != null) s.TransitionTo(0f);
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
