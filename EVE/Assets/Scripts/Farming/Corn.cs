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
    }

    public override void receiveInteract()
    {
        if(!readyToHarvest)
        {
            readyToGrow = true;
        }

        if(readyToHarvest)
        {
            Debug.Log("Harvesting Corn");
            Farm();
        }
        else if(readyToGrow)
        {
            Grow();
        }
    }

    public void Grow()
    {
        readyToGrow = false;
        StartCoroutine(GrowthController.GrowPlant(growthStagePrefabs, growthRate, transform.position, onGrowthComplete));
    }

    public void Farm(){
        readyToHarvest = false;
        readyToGrow = true;
        // inventoryManager.AddItem(item);
        Debug.Log("Farming Corn");
    }

    private void onGrowthComplete()
    {
        readyToHarvest = true;
    }
}