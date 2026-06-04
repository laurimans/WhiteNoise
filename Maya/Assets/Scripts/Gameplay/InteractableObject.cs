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
    [SerializeField] private bool wasInteractedInThisPhase = false;

    [Header("Phases")]
    [SerializeField] private InteractableData[] phasesData;

    [Header("Audio Source")]
    [SerializeField] protected AudioSource audioSource;

    private GamePhase currentPhase = 0;
    private GamePhase lastPhase;
    private Room parentRoom;
    private SpriteRenderer sRenderer;
    private bool isLightOn = true;

    // Dialogue
    private int currentDialogueIndex = 0;


     void Awake()
     {
        sRenderer = GetComponent<SpriteRenderer>();
        parentRoom = GetComponentInParent<Room>();

        GameManager.OnPhaseChanged += RefreshObject;
      }

    void Start()
    {
        if (itemID == null) Debug.LogWarning($"El item {itemID} no tiene id");
        if (sRenderer == null) Debug.LogError($"El item {itemID} no tiene spriteRenderer");
        if (this.GetComponent<BoxCollider2D>() == null) Debug.LogError($"El item {itemID} no tiene Collider");

        if (phasesData.Length != Enum.GetValues(typeof(GamePhase)).Length) Debug.LogWarning($"El item {itemID} no tiene todos los comportamientos");
        if(audioSource != null)
        {
            audioSource.loop = true;
            audioSource.playOnAwake = false;
        }
    }

    void OnEnable()
    {
        if (parentRoom != null)
        {
            parentRoom.OnLightToggled += HandleLightReaction;

            isLightOn = parentRoom.GetLightData();
        }

        RefreshObject(currentPhase);
    }

    void OnDisable()
    {
        if (parentRoom != null)
        {
            parentRoom.OnLightToggled -= HandleLightReaction;
        }
    }

    private void OnDestroy()
    {
        GameManager.OnPhaseChanged -= RefreshObject;
    }

    public string GetID() => itemID;
    public InteractableData GetPhaseData() => phasesData[(int)currentPhase];

    public GameObject GetTargetObject() => objectToActivateGO;
    public int GetDialogueIndex() => currentDialogueIndex;

    public bool GetInteractionData() => wasInteractedInThisPhase;
    public void MarkAsInteracted() => wasInteractedInThisPhase = true;
    public AudioSource GetAudioSource() => audioSource;

    public void IncrementDialogueCounter()
    {
        currentDialogueIndex++;
    }

    private void RefreshObject (GamePhase _currentPhase)
    {
        if (_currentPhase != lastPhase) // Cambio de fase
        {
            lastPhase = _currentPhase;
            currentPhase = _currentPhase;

            wasInteractedInThisPhase = false;
            currentDialogueIndex = 0;

            if (audioSource != null) audioSource.clip = null;
        }

        if (phasesData == null || (int)currentPhase >= phasesData.Length || phasesData[(int)currentPhase] == null)
        {
            //Debug.Log($"El objeto {itemID} esta desactivado");
            gameObject.SetActive(false);
            return;
        }

        InteractableData data = phasesData[(int)currentPhase];

        bool startsDisable = data.startDisable;
        bool shouldBeActivate = !startsDisable; 

        if (wasInteractedInThisPhase)
        {
            if (!startsDisable)
            {              
                shouldBeActivate = !data.activateOtherItem;
            }
            else
            {
                shouldBeActivate = true;
            }
        }

        HandleObjectActivation(!shouldBeActivate);

    }

    private void HandleLightReaction(bool lightState)
    {
        isLightOn = lightState;
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        InteractableData data = phasesData[(int)currentPhase];
        if (data.darkSprite == null) return;

        Sprite spriteToUse = isLightOn ? data.initialSprite : data.darkSprite;

        if (spriteToUse != null) sRenderer.sprite = spriteToUse;
    }

    private void HandleObjectActivation(bool shouldBeDisabled)
    {
        if (shouldBeDisabled)
        {
            Debug.Log($"El objeto {itemID} esta desactivado");
            gameObject.SetActive(false);
            return;
        }

        InteractableData data = phasesData[(int)currentPhase];
        Sprite spriteToUse = data.initialSprite;

        if(wasInteractedInThisPhase && data.otherSprite != null)
        {
            spriteToUse = data.otherSprite;
        }

        if (spriteToUse != null)
        {
            sRenderer.sprite = spriteToUse;
        }
        else
        {
            sRenderer.sprite = null;
        }

        this.gameObject.SetActive(true);
        UpdateVisuals();
    }


    public virtual void OnObjectClicked()
    {
        
        InteractableData data = phasesData[(int)currentPhase];

        if (data.actions != null)
        {
            bool needsJournal = GameManager.Instance.GetCurrentPhase() >= GamePhase.TuesdayMorning;
            bool hasJournal = GameManager.Instance.IsJournalPickedUp();

            bool givesClue = false;

            foreach (var act in data.actions)
            {
                if (act.GetType().Name == "GiveClueAction") givesClue = true;
            }

            if (givesClue && needsJournal && !hasJournal)
            {
                Debug.Log("No puedes investigar esto sin tu diario.");
            
                return;
            }

            foreach (var action in data.actions)
            {
                bool shouldContinue = action.Execute(this);
                if (!shouldContinue) break;
            }
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



}
