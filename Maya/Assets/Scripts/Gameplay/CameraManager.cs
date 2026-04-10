using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    [Header("UI & FX")]
    [SerializeField] private GameObject flashObject;
    [SerializeField] private GameObject cameraLense;
    [SerializeField] private JournalUI journalUI;
    [SerializeField] private GameObject HUDPanel;
    [SerializeField] private AudioClip cameraAudio;

    private bool isAiming = false;
    private HashSet<string> roomsPhotographed = new HashSet<string>();
    private const int TOTAL_PHOTOS_REQUIRED = 5;

    public void ToggleAiming()
    {
        isAiming = !isAiming;

        HUDPanel.SetActive(!isAiming);
        cameraLense.SetActive(isAiming);

        Debug.Log(isAiming ? "Apuntando con la cámara" : "Cámara guardada");
    }

    void Update()
    {
        if (isAiming && Mouse.current.leftButton.wasPressedThisFrame)
        {
            TryTakePhoto();
        }
    }

    private void TryTakePhoto()
    {
        // Buscamos la habitación actual
        GameObject roomObj = GameObject.FindGameObjectWithTag("Room");
        if (roomObj == null) return;

        Room room = roomObj.GetComponent<Room>();
        string roomID = room.GetID();

        if (roomsPhotographed.Contains(roomID))
        {
            Debug.Log("Ya has hecho una foto aquí.");
            ToggleAiming();
            return;
        }

        StartCoroutine(TakePhotoRoutine(roomID));

    }

    IEnumerator TakePhotoRoutine(string roomID)
    {
        isAiming = false;

        yield return new WaitForSeconds(0.3f);
        flashObject.SetActive(true);
        AudioManager.Instance.PlaySFX(cameraAudio);
        yield return new WaitForSeconds(0.2f);
        
        flashObject.SetActive(false);

        yield return new WaitForSeconds(0.7f);

        roomsPhotographed.Add(roomID);
        GameManager.Instance.AddTaskDone("photo_" + roomID);

        isAiming = false;
        cameraLense.SetActive(false);
        HUDPanel.SetActive(false);

        journalUI.OpenWithPhoto(roomID);

        Debug.Log($"Foto guardada de: {roomID}. Total: {roomsPhotographed.Count}/{TOTAL_PHOTOS_REQUIRED}");
    }

    public bool HasFinishedPhotos() => roomsPhotographed.Count >= TOTAL_PHOTOS_REQUIRED;
}
