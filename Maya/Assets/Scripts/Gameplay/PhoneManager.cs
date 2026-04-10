using UnityEngine;
using System.Collections;
using System;

public class PhoneManager : MonoBehaviour
{
    public static PhoneManager Instance { get; private set; }
    [SerializeField] private PhoneUI phoneUI;

    // Eventos
    public event Action OnConversationFinished;

    public void StartCall()
    {
        phoneUI.gameObject.SetActive(true);
        phoneUI.SetCloseButton(false);

        StopAllCoroutines();
        StartCoroutine(PlayConversation());
    }

    IEnumerator PlayConversation()
    {
        var conversation = GameManager.Instance.GetCurrentPhaseData().conversation;

        foreach (var msg in conversation)
        {
            phoneUI.AddMessage(msg.messageContent, msg.isMaya);
            yield return new WaitForSeconds(2f); 
        }

        phoneUI.SetCloseButton(true);
        GameManager.Instance.AddTaskDone("living_call");
    }

    public void FinishCall()
    {
        phoneUI.gameObject.SetActive(false);
        OnConversationFinished?.Invoke();
        GameManager.Instance.AddTaskDone("PhoneCall");
    }
}