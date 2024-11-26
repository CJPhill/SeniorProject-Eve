using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowthController : MonoBehaviour
{
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

    public IEnumerator GrowPlant(List<GameObject> growthStagePrefabs, float timeOfDay, Vector3 newSpawnPoint, System.Action onGrowthComplete)
    {
        plantSpawnPoint = newSpawnPoint;

        plantSpawnPoint.y += 0.7f;

        float elapsedTime = 0;
        currentStage = 0;

        while (currentStage < growthStagePrefabs.Count)
        {
            // Adjust growth rate based on the stage and time of day
            float adjustedGrowthRate = CalculateGrowthRate(timeOfDay, currentStage);

            while (elapsedTime < adjustedGrowthRate)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            elapsedTime = 0f;
            UpdateGrowthStage(growthStagePrefabs);
            currentStage++;
        }

        onGrowthComplete?.Invoke();
    }

    public void harvest(){
        Destroy(currentStageInstance);
    }

private float CalculateGrowthRate(float timeOfDay, int stage)
{
    float baseGrowthTime = 1;

    float timeOfDayFactor = timeOfDay < 12 ? 1f + (timeOfDay / 12f) : 3f - (timeOfDay / 12f);

    float stageFactor = 1f + (stage * 0.75f);

    return baseGrowthTime * timeOfDayFactor * stageFactor;
}

}
