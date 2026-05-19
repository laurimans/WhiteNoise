using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneManager : MonoBehaviour
{
    public static PhoneManager Instance { get; private set; }
    [SerializeField] private PhoneUI phoneUI;

    // Eventos
    public event Action OnConversationFinished;

    public void StartCall(string callKey)
    {
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