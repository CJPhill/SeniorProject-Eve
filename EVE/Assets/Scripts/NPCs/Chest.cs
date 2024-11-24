using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : NPC
{
    [SerializeField] private Canvas chestCanvas; 

    private bool chestActive = false; 

    public override void receiveInteract()
    {
        chestActive = !chestActive;

        chestCanvas.gameObject.SetActive(chestActive);
    }
}
