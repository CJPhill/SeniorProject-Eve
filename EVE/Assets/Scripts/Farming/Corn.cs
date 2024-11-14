using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corn : Plant, IInteractable
{
    [SerializeField] public List<GameObject> growthStagePrefabs; 

    public GrowthController GrowthController;
    public int growthRate = 2;

    public bool readyToGrow = true;
    public bool readyToHarvest = false;

    public void Start()
    {
        readyToGrow = true;
        // Debug.Log(readyToGrow);
    }

    public override void receiveInteract()
    {
        if(!readyToHarvest)
        {
            readyToGrow = true;
        }

        if(readyToHarvest)
        {
            Farm();
            readyToHarvest = false;
            readyToGrow = true;
            Debug.Log("Harvesting Corn");
        }
        else if(readyToGrow)
        {
            Debug.Log("Planting Corn");
            
            Grow();
        }
    }

    public void Grow()
    {
        readyToGrow = false;
        StartCoroutine(GrowthController.GrowPlant(growthStagePrefabs, growthRate));
        Debug.Log("Growing Corn");
    }

    public void Farm(){
        Debug.Log("Farming Corn");
    }
}