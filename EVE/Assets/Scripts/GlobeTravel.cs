using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobeTravel : MonoBehaviour, IInteractable
{
    public GameObject TravelSystem;



    public void receiveInteract()
    {
        //Call GameManager and set Terminal to active
        Debug.Log("Will open terminal");
        if (TravelSystem != null)
        {
            TravelSystem.SetActive(true);

        }
    }
}
