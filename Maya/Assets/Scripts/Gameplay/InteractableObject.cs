using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] private string itemID;
    [SerializeField] private InteractableData[] phasesData;

    private SpriteRenderer sRenderer;
    private GamePhase currentPhase;
    private int currentDialogueIndex;

    void OnEnable()
    {
        GameManager.OnPhaseChanged += RefreshObject;
    }

    void OnDisable()
    {
        GameManager.OnPhaseChanged -= RefreshObject;
    }

    void Awake()
    {
        sRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        if (itemID == null) Debug.LogWarning($"El item {itemID} no tiene id");
        if (sRenderer == null) Debug.LogError($"El item {itemID} no tiene spriteRenderer");

        // Comprobar que tenga collider

        if (phasesData.Length != Enum.GetValues(typeof(GamePhase)).Length) Debug.LogError($"El item {itemID} no tiene todos los comportamientos");
    }

    public string GetID() => itemID;

    private void RefreshObject (GamePhase _currentPhase)
    {
        currentPhase = _currentPhase;
        currentDialogueIndex = 0;

        Sprite sprite = phasesData[(int)currentPhase].initialSprite;
        if (sprite!=null) sRenderer.sprite = sprite;
        
    }

    public void OnObjectClicked()
    {
        InteractableData data = phasesData[(int)currentPhase];

        // Dialogues
        if (data.dialoguesList.Count > 0)
        {
            string textToSay = "";
            if (currentDialogueIndex < data.dialoguesList.Count)
            {
                textToSay = data.dialoguesList[currentDialogueIndex];
                currentDialogueIndex++;
            }
            else
            {
                textToSay = data.dialoguesList[data.dialoguesList.Count - 1];
            }

            DialogueManager.Instance.ShowDialogue(textToSay);
            Debug.Log($"{itemID}: {textToSay}");
        }

        // Sound
        if (data.sound != null)
        {
            AudioManager.Instance.PlaySFX(data.sound);
        }

        // Sprite change
        if (data.otherSprite != null && sRenderer.sprite != data.otherSprite)
        {
            sRenderer.sprite = data.otherSprite;
        }

        // Clue or Task
        if (data.isTask) GameManager.Instance.AddTaskDone(itemID);
        if (data.isClue) GameManager.Instance.AddClue(itemID);
    }

}
