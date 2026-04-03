using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    public event Action<InteractableObject> OnInteractableItemClicked;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (Pointer.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            ProcessClick();
        }
    }

    private void ProcessClick()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);

        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);

        if (hit.collider != null)
        {
            if (hit.collider.TryGetComponent<InteractableObject>(out InteractableObject item))
            {
                OnInteractableItemClicked?.Invoke(item);
            }
        }

    }

}
