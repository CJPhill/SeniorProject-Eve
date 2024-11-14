using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corn : Plant, IFarmable
{
    [SerializeField] public List<GameObject> growthStagePrefabs; 

    public GrowthController GrowthController;
    public int growthRate = 2;

    public override void receiveInteract()
    {
        if(GrowthController.readyToHarvest)
        {
            Farm();
            GrowthController.readyToHarvest = false;
            GrowthController.readyToGrow = true;
            Debug.Log("Harvesting Corn");
        }

        else if(GrowthController.readyToGrow)
        {
            GrowthController.readyToGrow = false;
            Grow();
        }
    }

    public void Grow()
    {
        GrowthController.readyToGrow = false;
        StartCoroutine(GrowthController.GrowPlant(growthStagePrefabs, growthRate));
        Debug.Log("Growing Corn");
    }

    public void Farm(){
        Debug.Log("Farming Corn");
    }
}