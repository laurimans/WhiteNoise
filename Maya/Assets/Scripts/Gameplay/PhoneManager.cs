using UnityEngine;
using System.Collections;

public class PhoneManager : MonoBehaviour
{
    public static PhoneManager Instance { get; private set; }
    [SerializeField] private PhoneUI phoneUI;
    private int messageIndex = 0;

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
        GameManager.Instance.AddTaskDone("PhoneCall");
    }
}