using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] private string idItem;
    [SerializeField] private SpriteRenderer itemSprite;

    //[SerializeField] private GameObject itemPrefab;
    //[SerializeField] private List<Sprite> itemImages;

    void Start()
    {
        if (idItem == null) Debug.LogError("El item no tiene id");
        if (itemSprite == null) Debug.LogError("El item no tiene sprite");
        //if (itemPrefab == null) Debug.LogError("El item no tiene prefab");

        // Comprobar que tenga collider
    }

    public string GetID()
    {
        return idItem;
    }



}
