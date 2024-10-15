using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantGrowth : MonoBehaviour
{
    [Header("Growth Settings")]
    [SerializeField] private List<GameObject> growthStagePrefabs; 
    [SerializeField] private Transform plantSpawnPoint; 
    [SerializeField] public LightingManager lightingManager;
    
    private List<GameObject> instantiatedStages = new List<GameObject>(); 
    private int currentStage = 0; 
    private bool isGrowing = false;
    private bool isHarvested = false; 


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
        if (isHarvested)
        {
            ResetPlant();
        }

        if (!isGrowing)
        {        
            if (instantiatedStages.Count > 0)
            {
                instantiatedStages[currentStage].SetActive(true);
            }
            isGrowing = true;
            StartCoroutine(GrowPlant());
        }

        if (currentStage == instantiatedStages.Count - 1)
        {
            // Debug.Log("Plant is fully grown!");
            Harvest();
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
        isGrowing = false;
        instantiatedStages[currentStage].SetActive(false); 
        isHarvested = true; 
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
