using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandExpansion : MonoBehaviour, IInteractable
{
    public int moneyRequired = 100;
    public bool built = false; //Maybe not needed
    public bool hasMoney = true; //Will need to be changed to false when able to check
    public GameObject bridge;


    public void receiveInteract()
    {
     //Check if player or inventory has money
     if (hasMoney)
        {
            if (bridge)
            {
                bridge.SetActive(true);
                Debug.Log("You expanded your island!");
                gameObject.SetActive(false);
            }
        }
     
    }
}
