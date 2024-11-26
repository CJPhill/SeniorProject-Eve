using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goobert : NPC, ITalkable
{
    [SerializeField] private DialogueText dialogueText;
    [SerializeField] private DialogController dialogController;

    [SerializeField] private InventoryItem present;
    [SerializeField] public InventoryManager inventoryManager;

    private bool dialogueActive;

    public override void receiveInteract()
    {
        Talk(dialogueText);
    }

    public void Talk(DialogueText dialogueText)
    {            
        Debug.Log("Talking to Goobert");
        dialogController.gameObject.SetActive(!dialogueActive);
        dialogController.DisplayNextDialog(dialogueText);

        // onFinishDialogue?.Invoke();
    }
}
