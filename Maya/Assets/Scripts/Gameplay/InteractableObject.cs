using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] private string itemID;

    [Header("Ordering System")]
    [SerializeField] private GameObject objectToActivateGO;
    private bool wasInteractedInThisPhase = false;

    [SerializeField] private InteractableData[] phasesData;

    private SpriteRenderer sRenderer;
    private GamePhase currentPhase = 0;
    private GamePhase lastPhase;
    private int currentDialogueIndex = 0;

    void Awake()
    {
        sRenderer = GetComponent<SpriteRenderer>();

        GameManager.OnPhaseChanged += RefreshObject;
    }

    void Start()
    {
        if (itemID == null) Debug.LogWarning($"El item {itemID} no tiene id");
        if (sRenderer == null) Debug.LogError($"El item {itemID} no tiene spriteRenderer");
        if (this.GetComponent<BoxCollider2D>() == null) Debug.LogError($"El item {itemID} no tiene Collider");

        if (phasesData.Length != Enum.GetValues(typeof(GamePhase)).Length-1) Debug.LogWarning($"El item {itemID} no tiene todos los comportamientos");
    }

    void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            RefreshObject(currentPhase);
        }
    }

    public string GetID() => itemID;
    public InteractableData GetPhaseData() => phasesData[(int)currentPhase];

    private void RefreshObject (GamePhase _currentPhase)
    {
        if (_currentPhase != lastPhase) // Cambio de fase
        {
            currentDialogueIndex = 0;
            lastPhase = _currentPhase;
            currentPhase = _currentPhase;

            wasInteractedInThisPhase = false;
        }

        if (phasesData == null || (int)currentPhase >= phasesData.Length || phasesData[(int)currentPhase] == null)
        {
            //Debug.Log($"El objeto {itemID} esta desactivado");
            gameObject.SetActive(false);
        } else
        {
            bool isCurrentlyDisabled = phasesData[(int)currentPhase].startDisable;
            bool finalActiveState = wasInteractedInThisPhase ? !isCurrentlyDisabled : isCurrentlyDisabled;

            HandleObjectActivation(finalActiveState);
        }  
    }

    private void HandleObjectActivation(bool shouldBeDisabled)
    {
        if (shouldBeDisabled)
        {
            Debug.Log($"El objeto {itemID} esta desactivado");
            gameObject.SetActive(false);
            return;
        }

        Sprite sprite = phasesData[(int)currentPhase].initialSprite;

        if (sprite != null)
        {
            sRenderer.sprite = sprite;
        }
        else
        {
            sRenderer.sprite = null;
        }

        this.gameObject.SetActive(true);
    }


    public virtual void OnObjectClicked()
    {

        InteractableData data = phasesData[(int)currentPhase];

        // Dialogues
        if (data.dialoguesList.Count > 0)
        {
            HandleDialogue(data);
        }

        // Sound
        if (data.sound != null)
        {
            HandleSoundEffect(data);
        }

        // Sprite change
        if (data.otherSprite != null)
        {
            HandleSpriteChange(data);
        }

        if (data.activateOtherItem)
        {
            this.wasInteractedInThisPhase = true;
            this.RefreshObject(currentPhase);

            if (objectToActivateGO != null)
            {
                objectToActivateGO.SetActive(true);
                InteractableObject nextScript = objectToActivateGO.GetComponent<InteractableObject>();
                if (nextScript != null)
                {
                    nextScript.wasInteractedInThisPhase = true;
                    nextScript.RefreshObject(currentPhase);
                }
            }
        }

        // Clue or Task
        if (data.isTask) GameManager.Instance.AddTaskDone(itemID);
        if (data.isClue) GameManager.Instance.AddClue(itemID);
        
    }

    private void HandleDialogue(InteractableData data)
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
        //Debug.Log($"{itemID}: {textToSay}");
    }

    private void HandleSoundEffect(InteractableData data)
    {
        AudioManager.Instance.PlaySFX(data.sound);
    }

    private void HandleSpriteChange(InteractableData data)
    {
        if(sRenderer.sprite != data.otherSprite)
        {
            sRenderer.sprite = data.otherSprite;
        } else
        {
            sRenderer.sprite = data.initialSprite;
        }
    }

    public void OnMouseEnter()
    {
        if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            CursorManager.Instance.SetInteractableCursor();
        }
    }

    private void OnMouseExit()
    {
        CursorManager.Instance.SetDefaultCursor();
    }

    void OnDestroy()
    {
        GameManager.OnPhaseChanged -= RefreshObject;
    }

}
