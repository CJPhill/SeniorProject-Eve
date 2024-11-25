using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Soil : MonoBehaviour, IInteractable
{
    private const float INTERACT_RADIUS = 2f;

    private Transform _playerTransform;
    [SerializeField] public InventoryManager inventoryManager;

    private InventoryItem holding = null;
    private GameObject plantedCrop;
    public Item seed;

    public bool planted = false;

    private void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (UserInput.instance.Interact && isPlayerInRange())
        {
            receiveInteract();
        }
    }

    public void receiveInteract()
    {
        // Planting logic
        holding = inventoryManager.ItemHeld();
        if (holding != null && !planted)
        {
            GameObject cropPrefab = Resources.Load<GameObject>($"{holding.gameObject.GetComponent<Image>().sprite.name}");
            if (cropPrefab == null)
            {
                Debug.LogError($"{holding.gameObject.GetComponent<Image>().sprite.name} prefab not found in Resources folder.");
                return;
            }

            plantedCrop = Instantiate(cropPrefab, transform.position, Quaternion.identity);
            Plant plant = plantedCrop.GetComponent<Plant>();

            if (plant != null)
            {
                seed = holding.item;
                planted = true;
                inventoryManager.GetSelectedItem(); // Remove seed from inventory

                // Ensure the plant starts its growth independently
                plant.receiveInteract();
            }
        }

        // Harvesting logic
        else if (planted)
        {
            Plant plant = plantedCrop.GetComponent<Plant>();
            if (plant != null)
            {
                plant.receiveInteract(); // Trigger harvesting
                planted = false;
                inventoryManager.AddItem(seed);
                inventoryManager.AddItem(seed); // Add harvested seed/item
                seed = null;
                Destroy(plantedCrop); // Cleanup
            }
        }
    }

    private bool isPlayerInRange()
    {
        return Vector3.Distance(transform.position, _playerTransform.position) < INTERACT_RADIUS;
    }
}
