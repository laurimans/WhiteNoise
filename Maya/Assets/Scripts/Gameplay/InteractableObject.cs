using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public string itemID;

    [Header("Ordering System")]
    [SerializeField] private GameObject objectToActivateGO;
    public bool wasInteractedInThisPhase = false;

    [Header("Phases")]
    [SerializeField] private InteractableData[] phasesData;

    public SpriteRenderer sRenderer;
    private GamePhase currentPhase = 0;
    private GamePhase lastPhase;
    private int currentDialogueIndex = 0;
    [Header("Animator")]
    [SerializeField] protected InteractableAnimation interactableAnimation;

    public static event Action<string> OnDialogueSaid; //

    [Header("Sound")]
    [SerializeField] protected AudioSource audioSource;


    protected virtual void Awake()
    {
        sRenderer = GetComponent<SpriteRenderer>();

        GameManager.OnPhaseChanged += RefreshObject;
    }

    protected virtual void Start()
    {
        if (itemID == null) Debug.LogWarning($"El item {itemID} no tiene id");
        if (sRenderer == null) Debug.LogError($"El item {itemID} no tiene spriteRenderer");
        if (this.GetComponent<BoxCollider2D>() == null) Debug.LogError($"El item {itemID} no tiene Collider");

        if (phasesData.Length != Enum.GetValues(typeof(GamePhase)).Length) Debug.LogWarning($"El item {itemID} no tiene todos los comportamientos");
    }

    protected virtual void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            RefreshObject(currentPhase);
        }
    }

    public string GetID() => itemID;
    public InteractableData GetPhaseData() => phasesData[(int)currentPhase];

    public GameObject GetTargetObject() => objectToActivateGO;

    public void MarkAsInteracted() => wasInteractedInThisPhase = true;

    private void RefreshObject (GamePhase _currentPhase)
    {
        if (_currentPhase != lastPhase) // Cambio de fase
        {
            currentDialogueIndex = 0;
            lastPhase = _currentPhase;
            currentPhase = _currentPhase;

            wasInteractedInThisPhase = false;

            if (audioSource != null) audioSource.clip = null;
            if (interactableAnimation != null) interactableAnimation.SetActive(false); // Apagar animaciones
        }

        if (phasesData == null || (int)currentPhase >= phasesData.Length || phasesData[(int)currentPhase] == null)
        {
            //Debug.Log($"El objeto {itemID} esta desactivado");
            gameObject.SetActive(false);
        } else
        {
            bool startsDisable = phasesData[(int)currentPhase].startDisable;
            bool shouldBeActivate = wasInteractedInThisPhase? startsDisable : !startsDisable;

            HandleObjectActivation(!shouldBeActivate);
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

        if (data.actions != null)
        {
            foreach (var action in data.actions)
            {
                bool shouldContinue = action.Execute(this);
                if (!shouldContinue) break;
            }
        }

        ////////////////////////////////// Dialogues
        if (data.dialoguesList.Count > 0)
        {
            //HandleDialogue(data);
        }

        //////////////////////////////// Sound
        if (data.sound != null)
        {
            //HandleSoundEffect(data);
        }

        /////////////////////////////// Sprite change
        if (data.otherSprite != null)
        {
            //HandleSpriteChange(data);
        }

        //////////////////////////////// Clue or Task
        if (data.isTask && !wasInteractedInThisPhase)
        {
            //wasInteractedInThisPhase = true;
            //GameManager.Instance.AddTaskDone(itemID);
        }
           
        if (data.isClue && !wasInteractedInThisPhase)
        {
            //wasInteractedInThisPhase = true;
            //GameManager.Instance.AddClue(itemID);
        }

        /*
        if (data.activateOtherItem)
        {
            this.RefreshObject(currentPhase);
            wasInteractedInThisPhase = true;

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
        }*/

        //if (interactableAnimation != null) interactableAnimation.ToggleAnimation();

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

        OnDialogueSaid?.Invoke(textToSay);
    }

    private void HandleSoundEffect(InteractableData data)
    {
        if(audioSource == null)
        {
            AudioManager.Instance.PlaySFX(data.sound);
        } else
        {
            AudioManager.Instance.Play3DSFX(data.sound, audioSource);
        }
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
