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
        Talk(dialogueText, () => 
        {
            inventoryManager.AddItem(present);
        });
    }

    public void Talk(DialogueText dialogueText, System.Action onFinishDialogue)
    {            
        dialogController.gameObject.SetActive(!dialogueActive);
        dialogController.DisplayNextDialog(dialogueText);

        // onFinishDialogue?.Invoke();
    }
}
