using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using System.Collections;

public class InteractablePhone : MonoBehaviour
{
    [Header("Phone")]
    [SerializeField] private string phoneID = "CALL_MOM";
    [SerializeField] private GameObject phoneVisual;
    [SerializeField] private PhoneManager phoneManager;

    private SpriteRenderer sRenderer;
    private BoxCollider2D bCollider;
    private bool isRinging = false;

    void Awake()
    {
        sRenderer = GetComponent<SpriteRenderer>();
        bCollider = GetComponent<BoxCollider2D>();
    }

    void Start()
    {
       if (PhoneManager.Instance != null)
            PhoneManager.Instance.OnRingTimeReached += HandleGlobalRing;
    }

    void OnEnable()
    {
        if (PhoneManager.Instance != null && PhoneManager.Instance.isPhoneRinging)
        {
            ShowRinging();
        }
        else
        {
            StopAndHide();
        }
    }

    void OnDestroy()
    {
        if (PhoneManager.Instance != null)
            PhoneManager.Instance.OnRingTimeReached -= HandleGlobalRing;
    }

    private void HandleGlobalRing()
    {
        if (gameObject.activeInHierarchy)
        {
            ShowRinging();
        }
    }

    public void ShowRinging()
    {
        isRinging = true;

        phoneVisual.SetActive(true);

        sRenderer.enabled = true;
        bCollider.enabled = true;
    }

    public void StopAndHide()
    {
        isRinging = false;

        phoneVisual.SetActive(false);

        sRenderer.enabled = false;
        bCollider.enabled = false;
    }


    private void OnMouseDown()
    {
        if (isRinging)
        {
            isRinging = false;

            if (phoneManager != null)
            {
                string callKey = $"{phoneID}_P{(int)GameManager.Instance.GetCurrentPhase()}";

                phoneManager.StartCall(callKey);
                phoneManager.OnConversationFinished += StopAndHide;
            }
        }
    }
}
