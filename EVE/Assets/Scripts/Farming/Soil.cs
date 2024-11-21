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

    private IInteractable interactableCrop;

    private InventoryItem holding = null;
    private GameObject cropPrefab;

    public bool planted = false;
    private GameObject plantedCrop;
    public Item seed;

    private void Start() {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update() {
        if (UserInput.instance.Interact && isPlayerInRange())
        {   
            receiveInteract();
        }
    }

    public void receiveInteract() {
        //plant seed
        holding = inventoryManager.ItemHeld();

        if (holding != null && !planted) {
            //  && holding.item.type == ItemType.BuildingBlock

            cropPrefab = Resources.Load<GameObject>($"{holding.gameObject.GetComponent<Image>().sprite.name}");
            if (cropPrefab == null) {
                Debug.LogError($"{holding.gameObject.GetComponent<Image>().sprite.name} prefab not found in Resources folder.");
            }

            seed = holding.item;
            plantedCrop = Instantiate(cropPrefab, transform.position, Quaternion.identity);
            
            interactableCrop = plantedCrop.GetComponent<IInteractable>();
            if (interactableCrop != null) {
                planted = true;
                interactableCrop.receiveInteract(); 
                inventoryManager.GetSelectedItem();
            }
        }

        //harvest crop
        else if(planted) {
            planted = false;
            interactableCrop.receiveInteract();
            inventoryManager.AddItem(seed);
            seed = null;
            Destroy(plantedCrop);
        }
    }

    private bool isPlayerInRange() {
        return Vector3.Distance(transform.position, _playerTransform.position) < INTERACT_RADIUS;
    }
}