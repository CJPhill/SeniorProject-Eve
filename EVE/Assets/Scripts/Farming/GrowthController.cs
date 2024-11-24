using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowthController : MonoBehaviour
{
    [SerializeField] public LightingManager lightingManager;

    private GameObject currentStageInstance = null;
    private Vector3 plantSpawnPoint;

    private int currentStage = 0;

    void UpdateGrowthStage(List<GameObject> growthStagePrefabs)
    {
        if (currentStageInstance != null)
        {
            Destroy(currentStageInstance);
            Debug.Log("Destroyed current stage instance");
        }

        if (currentStage < growthStagePrefabs.Count)
        {
            currentStageInstance = Instantiate(growthStagePrefabs[currentStage], plantSpawnPoint, Quaternion.identity);
        }
    }

    public IEnumerator GrowPlant(List<GameObject> growthStagePrefabs, int growthRate, Vector3 newSpawnPoint, System.Action onGrowthComplete)
    {
        plantSpawnPoint = newSpawnPoint;

        float elapsedTime = 0;
        currentStage = 0;

        while (currentStage < growthStagePrefabs.Count)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= growthRate)
            {
                elapsedTime = 0f;
                UpdateGrowthStage(growthStagePrefabs);
                currentStage++;
            }

            yield return null;
        }

        onGrowthComplete?.Invoke();
    }
}
