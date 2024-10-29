using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class DialogController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI NPCName;
    [SerializeField] private TextMeshProUGUI NPCDialogue;
    [SerializeField] private float typingSpeed = 10f;

    private Queue<string> dialogueLines = new Queue<string>();

    private bool conversationEnded;
    private bool isTyping;

    private string d;

    private Coroutine typeDialogueCoroutine;
    private const float MAX_TYPE_TIME = 0.5f;

    public void DisplayNextDialog(DialogueText dialogueText)
    {
        if(dialogueLines.Count == 0)
        {
            if(!conversationEnded){
                //start conversation
                StartConversation(dialogueText);
            }
            else if(conversationEnded && !isTyping){
                //end conversation
                EndConversation();
                return;
            }
        }

        if(!isTyping){
            d = dialogueLines.Dequeue();

            typeDialogueCoroutine = StartCoroutine(TypeDialogueText(d));
        }

        else{
            FinishTypingEarly();
        }

        NPCDialogue.text = d;

        if(dialogueLines.Count == 0){
            conversationEnded = true;
        }
    }

    private void StartConversation(DialogueText dialogueText){
        if(!gameObject.activeSelf){
            gameObject.SetActive(true);
        }

        NPCName.text = dialogueText.speakerName;

        for(int i = 0; i < dialogueText.dialogueLines.Length; i++){
            dialogueLines.Enqueue(dialogueText.dialogueLines[i]);
        }

    }

    private void EndConversation(){
        if(gameObject.activeSelf){
            gameObject.SetActive(false);
        }
        conversationEnded = true;
    }

    private void FinishTypingEarly(){
        StopCoroutine(typeDialogueCoroutine);

        NPCDialogue.maxVisibleCharacters = d.Length;
        
        isTyping = false;
    }

    private IEnumerator TypeDialogueText(string d)
    {
        isTyping = true;

        int maxVisibleChars = 0;

        NPCDialogue.text = d;
        NPCDialogue.maxVisibleCharacters = maxVisibleChars;        

        foreach (char c in d.ToCharArray())
        {

            maxVisibleChars++;
            NPCDialogue.maxVisibleCharacters = maxVisibleChars;

            yield return new WaitForSeconds(MAX_TYPE_TIME / typingSpeed);
        }

        isTyping = false;
    }


}
