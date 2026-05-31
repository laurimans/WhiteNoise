using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using System.Collections;

public class InteractablePhone : MonoBehaviour
{
    [Header("Phone")]
    [SerializeField] private string phoneID = "CALL_MOM";
    [SerializeField] private GameObject phoneVisual;
    [SerializeField] private PhoneManager phoneManager;
    [SerializeField] private AudioClip phoneTone;
    [SerializeField] private AudioSource phoneSource;

    [Header("Timers")]
    [SerializeField] private float delayBeforeRinging = 60f;

    private bool isRinging = false;

    void Start()
    {
        GameManager.OnPhaseChanged += CheckPhase;

        if (PhoneManager.Instance != null)
            PhoneManager.Instance.OnRingTimeReached += HandleGlobalRing;

        if (GameManager.Instance != null) CheckPhase(GameManager.Instance.GetCurrentPhase());
    }

    void OnDestroy()
    {
        GameManager.OnPhaseChanged -= CheckPhase;
        if (PhoneManager.Instance != null)
            PhoneManager.Instance.OnRingTimeReached -= HandleGlobalRing;
    }


    private void CheckPhase(GamePhase phase)
    {
        string callKey = $"{phoneID}_P{(int)phase}";

        var callData = LocalizationManager.Instance.GetPhoneCall(callKey);

        if (callData != null && callData.Count>0)
        {
            if(PhoneManager.Instance != null)
                PhoneManager.Instance.StartCallTimer(delayBeforeRinging);
        }
        else
        {
            if (PhoneManager.Instance != null) PhoneManager.Instance.isPhoneRinging = false;
            StopAndHide();
        }
    }

    private void HandleGlobalRing()
    {
        if (gameObject.activeInHierarchy)
        {
            ShowAndRing();
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
                string callKey = $"{phoneID}_P{(int)GameManager.Instance.GetCurrentPhase()}";

                phoneManager.StartCall(callKey);
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
