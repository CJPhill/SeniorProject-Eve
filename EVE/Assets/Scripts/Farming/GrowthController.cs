using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowthController : MonoBehaviour
{
    [SerializeField] public LightingManager lightingManager;

    public bool readyToGrow = false;
    public bool readyToHarvest = false;

    private int currentStage = 0;

    void UpdateGrowthStage(List<GameObject> growthStagePrefabs)
    {
        foreach (GameObject stage in growthStagePrefabs
        )
        {
            stage.SetActive(false);
        }

        if (currentStage < growthStagePrefabs.Count)
        {
            growthStagePrefabs[currentStage].SetActive(true);
        }
    }

    public IEnumerator GrowPlant(List<GameObject> growthStagePrefabs, int growthRate)
    {
        if (Mathf.RoundToInt(lightingManager.TimeOfDay) % (growthRate*2+2) == 0)
        {
            currentStage++;
            UpdateGrowthStage(growthStagePrefabs);
        }
        yield return null;
    }

}