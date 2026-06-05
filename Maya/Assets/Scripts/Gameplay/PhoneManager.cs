using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneManager : MonoBehaviour
{
    public static PhoneManager Instance { get; private set; }
    [SerializeField] private PhoneUI phoneUI;

    [Header("Phone Configuration")]
    [SerializeField] private string phoneID = "CALL_MOM";
    [SerializeField] private float delayBeforeRinging = 20f;

    [Header("Audio")]
    [SerializeField] private AudioClip phoneTone;
    [SerializeField] private AudioSource phoneAudioSource;

    // Eventos
    public bool isPhoneRinging = false;
    public event Action OnRingTimeReached;
    public event Action OnConversationFinished;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        GameManager.OnPhaseChanged += CheckPhaseForCall;

        if (GameManager.Instance != null) CheckPhaseForCall(GameManager.Instance.GetCurrentPhase());
    }

    private void OnDestroy()
    {
        GameManager.OnPhaseChanged -= CheckPhaseForCall;
    }
    private void CheckPhaseForCall(GamePhase phase)
    {
        string callKey = $"{phoneID}_P{(int)phase}";
        var callData = LocalizationManager.Instance.GetPhoneCall(callKey);

        if (callData != null && callData.Count > 0)
        {
            StartCallTimer(delayBeforeRinging);
        }
        else
        {
            isPhoneRinging = false;
            StopAllCoroutines();
            StopRingingSound();
        }
    }

    public void StartCallTimer(float delay)
    {
        isPhoneRinging = false;
        StopAllCoroutines();
        StartCoroutine(RingTimerRoutine(delay));
    }

    private IEnumerator RingTimerRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        isPhoneRinging = true;
        PlayRingingSound();
        OnRingTimeReached?.Invoke();
    }

    private void PlayRingingSound()
    {
        if (phoneAudioSource != null && phoneTone != null)
        {
            phoneAudioSource.clip = phoneTone;
            phoneAudioSource.loop = true;
            phoneAudioSource.Play();
        }
    }

    private void StopRingingSound()
    {
        if (phoneAudioSource != null) phoneAudioSource.Stop();
    }

    public void StartCall(string callKey)
    {
        isPhoneRinging = false;
        StopRingingSound();
        phoneUI.gameObject.SetActive(true);
        phoneUI.SetCloseButton(false);

        var conversation = LocalizationManager.Instance.GetPhoneCall(callKey);

        StopAllCoroutines();
        StartCoroutine(PlayConversation(conversation));
    }

    IEnumerator PlayConversation(List<PhoneMessageEntry> conversation)
    {
        foreach (var msg in conversation)
        {
            bool isMaya = msg.speaker.ToLower() == "maya";
            phoneUI.AddMessage(msg.text, isMaya);
            yield return new WaitForSeconds(2.5f); 
        }

        phoneUI.SetCloseButton(true);
    }

    public void FinishCall()
    {
        phoneUI.gameObject.SetActive(false);
        OnConversationFinished?.Invoke();
        GameManager.Instance.AddTaskDone("PhoneCall");
    }
}