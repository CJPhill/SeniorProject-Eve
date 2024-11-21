using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowthController : MonoBehaviour
{
    [SerializeField] public LightingManager lightingManager;
    private List<GameObject> instantiatedStages = new List<GameObject>(); 

    private Vector3 plantSpawnPoint;

    private int currentStage = 0;

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

    void InitializeStages(List<GameObject> growthStagePrefabs)
    {
        foreach (GameObject stage in instantiatedStages)
        {
            Destroy(stage);
        }
        instantiatedStages.Clear();

        foreach (GameObject prefab in growthStagePrefabs)
        {
            GameObject stageInstance = Instantiate(prefab, plantSpawnPoint, Quaternion.identity);
            stageInstance.SetActive(false); 
            instantiatedStages.Add(stageInstance);
        }
    }

    public IEnumerator GrowPlant(List<GameObject> growthStagePrefabs, int growthRate, Vector3 newSpawnPoint, System.Action onGrowthComplete)
    {
        plantSpawnPoint = newSpawnPoint;

        InitializeStages(growthStagePrefabs);

        float elapsedTime = 0;
        currentStage = 0;

        plantSpawnPoint = newSpawnPoint;
        // if (Mathf.RoundToInt(lightingManager.TimeOfDay) % (growthRate*2+2) == 0)

        while (currentStage < growthStagePrefabs.Count) {
            elapsedTime += Time.deltaTime;

            
            if (elapsedTime >= growthRate) {
                elapsedTime = 0f; 
                currentStage++; 
                UpdateGrowthStage();
            }

            yield return null;
        }

        onGrowthComplete?.Invoke();
    }

}