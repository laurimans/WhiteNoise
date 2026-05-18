using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogueAction", menuName = "Actions/Dialogue")]
public class DialogueAction : InteractableAction
{
    [Header("Dialogues")]
    [TextArea] public List<string> dialoguesList;

    private int currentDialogueIndex = 0;
    public static event Action<string> OnDialogueSaid;


    public override bool Execute(InteractableObject owner)
    {
        string textToSay = "";
        if (currentDialogueIndex < dialoguesList.Count)
        {
            textToSay = dialoguesList[currentDialogueIndex];
            currentDialogueIndex++;
        }
        else
        {
            textToSay = dialoguesList[dialoguesList.Count - 1];
        }

        OnDialogueSaid?.Invoke(textToSay);

        return true;
    }

}
