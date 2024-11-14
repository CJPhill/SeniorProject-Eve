using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Soil : MonoBehaviour, IInteractable
{
    private const float INTERACT_RADIUS = 5f;

    private Transform _playerTransform;
    [SerializeField] public InventoryManager inventoryManager;

    private InventoryItem holding = null;
    private GameObject cropPrefab;

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
        holding = inventoryManager.ItemHeld();

        // Debug.Log($"{holding.item.name}");

        if (holding != null) {
            //  && holding.item.type == ItemType.BuildingBlock
            // cropPrefab = Resources.Load<GameObject>($"{holding.gameObject.name}");
            cropPrefab = Resources.Load<GameObject>("Corn");

            // if (cropPrefab == null) {
            //     Debug.LogError($"{holding.gameObject.name} prefab not found in Resources folder.");
            // }
            // else {
                GameObject newCrop = Instantiate(cropPrefab, transform.position, Quaternion.identity);

                Plant plantComponent = newCrop.GetComponent<Plant>();
                if (plantComponent != null) {
                    plantComponent.receiveInteract(); 
                }
            // }
        }
        Debug.Log("help");
    }

    private bool isPlayerInRange() {
        return Vector3.Distance(transform.position, _playerTransform.position) < INTERACT_RADIUS;
    }
}