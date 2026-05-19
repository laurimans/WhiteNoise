using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogueAction", menuName = "Actions/Dialogue")]
public class DialogueAction : InteractableAction
{
    private string lastItemID;
    private int currentIndex = 0;
    private GamePhase lastRecordedPhase;

    public static event Action<string> OnDialogueSaid;


    public override bool Execute(InteractableObject owner)
    {
        string id = owner.GetID();
        int phaseIndex = (int)GameManager.Instance.GetCurrentPhase();

        string fullKey = $"{id}_P{phaseIndex}";

        if (id != lastItemID || GameManager.Instance.GetCurrentPhase() != lastRecordedPhase)
        {
            currentIndex = 0;
            lastItemID = id;
            lastRecordedPhase = GameManager.Instance.GetCurrentPhase();
        }

        List<string> lines = LocalizationManager.Instance.GetLines(fullKey);

        if (lines == null || lines.Count == 0)
        {
            Debug.LogWarning($"No se encontraron líneas para la clave: {fullKey}");
            return true;
        }

        string textToSay = (currentIndex < lines.Count)
            ? lines[currentIndex]
            : lines[lines.Count - 1];

        currentIndex++;

        OnDialogueSaid?.Invoke(textToSay);

        return true;
    }

}
