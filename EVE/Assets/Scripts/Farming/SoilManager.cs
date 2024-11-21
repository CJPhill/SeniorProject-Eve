using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlantGrowth : MonoBehaviour
{
    [Header("Growth Settings")]
    [SerializeField] private List<GameObject> growthStagePrefabs; 
    [SerializeField] private Transform plantSpawnPoint; 
    [SerializeField] public LightingManager lightingManager;

    public Item[] seedInSoil;
    private List<GameObject> instantiatedStages = new List<GameObject>(); 
    private int currentStage = 0; 
    private bool isGrowing = false;
    private bool isHarvested = false;
    private InventoryItem holding = null;
    //private bool soilHoed = false;
    public InventoryManager inventoryManager;

    void Start()
    {
        InitializeStages();
    }

    void InitializeStages()
    {
        foreach (GameObject prefab in growthStagePrefabs)
        {
            GameObject stageInstance = Instantiate(prefab, plantSpawnPoint.position, Quaternion.identity);
            stageInstance.SetActive(false); 
            instantiatedStages.Add(stageInstance);
        }
    }

    void OnMouseDown()
    {
        holding = inventoryManager.ItemHeld();
        if (isHarvested)
        {
            
            ResetPlant();
        }

        if (!isGrowing)
        {
            
            if (holding != null && holding.gameObject.GetComponent<Image>().sprite.name == "Seed")
            {
                if (instantiatedStages.Count > 0)
                {
                    instantiatedStages[currentStage].SetActive(true);
                }
                isGrowing = true;
                StartCoroutine(GrowPlant());
            }
            else
            {
                Debug.Log(holding.gameObject.GetComponent<Image>().sprite.name);
            }
            
        }


        if (currentStage == instantiatedStages.Count - 1)
        {
            // Debug.Log("Plant is fully grown!");
            Harvest();
        }
    }

    public void GetItem(Item item)
    {
        inventoryManager.AddItem(item);
    }



    public void UseSelectedItem()
    {
        Item receivedItem = inventoryManager.GetSelectedItem();
        if (receivedItem != null)
        {
            Debug.Log("Used item: " + receivedItem);
        }
        else
        {
            Debug.Log("No item used!");
        }
    }

    IEnumerator GrowPlant()
    {
        while (isGrowing && currentStage < instantiatedStages.Count - 1)
        {
            if (Mathf.RoundToInt(lightingManager.TimeOfDay) % (currentStage*2+2) == 0)
            {
                currentStage++;
                UpdateGrowthStage();
            }
            yield return null;
        }
    }


    void UpdateGrowthStage()
    {
        foreach (GameObject stage in instantiatedStages)
        {
            stage.SetActive(false);
        }

        if (currentStage < instantiatedStages.Count)
        {
            instantiatedStages[currentStage].SetActive(true);
        }
    }

    public void Harvest()
    {
        if (holding != null && holding.gameObject.GetComponent<Image>().sprite.name == "Pickaxe") // will be a hoe later but is not now bc that isn't spawning right away
        {
            GetItem(seedInSoil[0]);
            isGrowing = false;
            instantiatedStages[currentStage].SetActive(false);
            isHarvested = true;
        }

        else
        {
            Debug.Log("Please use a Hoe to harvest a plant!");
        }
        
    }

    void ResetPlant()
    {
        currentStage = 0;
        isGrowing = false;
        isHarvested = false;

        foreach (GameObject stage in instantiatedStages)
        {
            stage.SetActive(false);
        }

        if (instantiatedStages.Count > 0)
        {
            instantiatedStages[currentStage].SetActive(true);
        }

    }
}
