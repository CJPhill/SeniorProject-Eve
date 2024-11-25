using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corn : Plant, IInteractable
{

    [Header("Dependencies")]
    [SerializeField] public List<GameObject> growthStagePrefabs; 
    [SerializeField] public LightingManager lightingManager;

    public GrowthController GrowthController;

    [Header("Local Variables")]
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

        Debug.Log("Interacting with Corn");
        if(readyToHarvest)
        {            
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
        float currentLightingFactor = lightingManager.TimeOfDay;
        StartCoroutine(GrowthController.GrowPlant(growthStagePrefabs, currentLightingFactor, transform.position, onGrowthComplete));
    }

    public void Farm(){
        readyToHarvest = false;
        readyToGrow = true;
        Debug.Log("Farming Corn");
        GrowthController.harvest();
    }

    private void onGrowthComplete()
    {
        readyToHarvest = true;
    }
}
