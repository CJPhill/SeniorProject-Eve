using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowthController : MonoBehaviour
{
    [SerializeField] public LightingManager lightingManager;

    private int currentStage = 0;

    void UpdateGrowthStage(List<GameObject> growthStagePrefabs)
    {
        foreach (GameObject stage in growthStagePrefabs)
        {
            stage.SetActive(false);
        }

        if (currentStage < growthStagePrefabs.Count)
        {
            growthStagePrefabs[currentStage].SetActive(true);
            Debug.Log(currentStage);
        }
    }

    public IEnumerator GrowPlant(List<GameObject> growthStagePrefabs, int growthRate)
    {
        float elapsedTime = 0;
        Debug.Log(currentStage);

        // if (Mathf.RoundToInt(lightingManager.TimeOfDay) % (growthRate*2+2) == 0)

        while (currentStage < growthStagePrefabs.Count) {
            Debug.Log(elapsedTime);

            if (elapsedTime >= growthRate) {
                elapsedTime = 0f; 
                currentStage++; 
                UpdateGrowthStage(growthStagePrefabs);
            }

            yield return null;
        }
    }

}