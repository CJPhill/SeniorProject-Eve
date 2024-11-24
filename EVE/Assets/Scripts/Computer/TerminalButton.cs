using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TerminalButton : MonoBehaviour, IInteractable
{
    public GameObject ConsoleSystem;


    
    public void receiveInteract()
    {
        //Call GameManager and set Terminal to active
        Debug.Log("Will open terminal");
        if (ConsoleSystem != null) 
        {
            ConsoleSystem.SetActive(true);

        }
    }

    
   
}
