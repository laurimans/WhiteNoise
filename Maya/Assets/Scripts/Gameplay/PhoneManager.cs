using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneManager : MonoBehaviour
{
    public static PhoneManager Instance { get; private set; }
    [SerializeField] private PhoneUI phoneUI;

    // Eventos
    public bool isPhoneRinging = false;
    public event Action OnRingTimeReached;
    public event Action OnConversationFinished;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
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
        OnRingTimeReached?.Invoke();
    }

    public void StartCall(string callKey)
    {
        isPhoneRinging = false;
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