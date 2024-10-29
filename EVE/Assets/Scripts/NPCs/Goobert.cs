using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goobert : NPC, ITalkable
{
    [SerializeField] private DialogueText dialogueText;
    [SerializeField] private DialogController dialogController;

    private bool dialogueActive;

    public override void receiveInteract()
    {
        Talk(dialogueText);
    }

    public void Talk(DialogueText dialogueText)
    {
        dialogController.gameObject.SetActive(!dialogueActive);
        dialogController.DisplayNextDialog(dialogueText);
    }
}
