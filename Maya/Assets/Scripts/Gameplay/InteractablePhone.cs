using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class InteractablePhone : MonoBehaviour
{
    [Header("Phone")]
    [SerializeField] private GameObject phoneVisual;
    [SerializeField] private PhoneManager phoneManager;
    [SerializeField] private AudioClip phoneTone;
    [SerializeField] private AudioSource phoneSource;

    private bool isRinging = false;

    void Start()
    {
        GameManager.OnPhaseChanged += CheckPhase;
        if (GameManager.Instance != null) CheckPhase(GameManager.Instance.GetCurrentPhase());
    }

    void OnDestroy()
    {
        GameManager.OnPhaseChanged -= CheckPhase;
    }


    private void CheckPhase(GamePhase phase)
    {
        var data = GameManager.Instance.GetCurrentPhaseData();
        if (data != null && data.hasPhoneCall)
        {
            ShowAndRing();
        }
        else
        {
            StopAndHide();
        }
    }

    private void Update()
    {
        if (Keyboard.current.wKey.wasPressedThisFrame)
        {
            ShowAndRing();
            Debug.Log("Debugueando telefono");
        }
    }

    public void ShowAndRing()
    {
        isRinging = true;
        phoneVisual.SetActive(true);
        HandleSound(isRinging);
    }

    public void StopAndHide()
    {
        isRinging = false;
        phoneVisual.SetActive(false);
    }


    private void OnMouseDown()
    {
        if (isRinging)
        {
            isRinging = false;
            HandleSound(isRinging);

            if (phoneManager != null)
            {
                phoneManager.StartCall();
                phoneManager.OnConversationFinished += StopAndHide;
            }
        }
    }

    private void HandleSound(bool isRinging)
    {
        if (isRinging)
        {
            if (phoneSource != null && phoneTone != null)
            {
                phoneSource.clip = phoneTone;
                phoneSource.loop = true;
                phoneSource.Play();
            }
            
        } else
        {
            phoneSource.clip = null;
            phoneSource.Stop();
        }
    }

}
