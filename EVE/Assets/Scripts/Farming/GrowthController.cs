using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowthController : MonoBehaviour
{
    [SerializeField] public LightingManager lightingManager;
    private List<GameObject> instantiatedStages = new List<GameObject>(); 

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
            Debug.Log("stage" + currentStage);
        }
    }

    // void InitializeStages(List<GameObject> growthStagePrefabs)
    // {
    //     foreach (GameObject prefab in growthStagePrefabs)
    //     {
    //         GameObject stageInstance = Instantiate(prefab, plantSpawnPoint.position, Quaternion.identity);
    //         stageInstance.SetActive(false); 
    //         instantiatedStages.Add(stageInstance);
    //     }
    // }

    public IEnumerator GrowPlant(List<GameObject> growthStagePrefabs, int growthRate)
    {
        float elapsedTime = 0;
        currentStage = 0;
        // InitializeStages(growthStagePrefabs);

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
    }

}