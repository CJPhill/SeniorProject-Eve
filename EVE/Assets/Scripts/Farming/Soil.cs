using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Soil : MonoBehaviour, IInteractable
{
    private const float INTERACT_RADIUS = 5f;

    private Transform _playerTransform;
    [SerializeField] public InventoryManager inventoryManager;

    private InventoryItem holding = null;
    private GameObject cropPrefab;

    public bool planted = false;

    private void Start() {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update() {
        if (UserInput.instance.Interact && isPlayerInRange() && !planted)
        {   
            receiveInteract();
        }
    }

    public void receiveInteract() {
        holding = inventoryManager.ItemHeld();
        
        cropPrefab = Resources.Load<GameObject>($"{holding.gameObject.GetComponent<Image>().sprite.name}");

        if (cropPrefab == null) {
            Debug.LogError($"{holding.gameObject.GetComponent<Image>().sprite.name} prefab not found in Resources folder.");
        }

        if (holding != null && !planted) {
            //  && holding.item.type == ItemType.BuildingBlock
            GameObject newCrop = Instantiate(cropPrefab, transform.position, Quaternion.identity);
            
            IInteractable interactableCrop = newCrop.GetComponent<IInteractable>();
            if (interactableCrop != null) {
                planted = true;
                interactableCrop.receiveInteract(); 
            }
        }
    }

    private bool isPlayerInRange() {
        return Vector3.Distance(transform.position, _playerTransform.position) < INTERACT_RADIUS;
    }
}