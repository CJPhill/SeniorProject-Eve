using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CutsceneController : MonoBehaviour
{
    public TMP_Text dialogueText;
    public Image flashImage;
    public TMP_Text welcomeText;
    public Image blackOverlay;
    public Image card;
    public Button wakeUpButton;

    [Header("Creepy Flash Images")]
    public Sprite[] flashSprites;

    [Header("Settings")]
    public float flashSpeed = 0.05f;
    public float typeSpeed = 0.05f;

    private void Start()
    {
        dialogueText.text = "";
        flashImage.enabled = false;
        welcomeText.gameObject.SetActive(false);
        blackOverlay.color = Color.black; // start fully black

        StartCoroutine(PlayIntroSequence());
    }

    IEnumerator PlayIntroSequence()
    {
        // Mysterious dialogue
        yield return TypeDialogue("Eve?");
        yield return new WaitForSeconds(1.2f);
        yield return TypeDialogue("Do you read me?");
        yield return new WaitForSeconds(1.2f);
        yield return TypeDialogue("We need to--");
        yield return new WaitForSeconds(0.8f);
        yield return TypeDialogue(" Eve we aren't going to make it you need to-");
        yield return new WaitForSeconds(0.8f);
        yield return TypeDialogue("REMEMBER");
        dialogueText.gameObject.SetActive(false);

        // Sharp flash sequence
        yield return SharpFlashCreepyImages();

        // Sharp cut to white screen
        blackOverlay.color = Color.white;
        flashImage.enabled = false;
        

        // Welcome screen
        welcomeText.gameObject.SetActive(true);
        card.gameObject.SetActive(true);

        yield return TypeWelcome("Goodmorning EVE Unit! \n\nWe will be with you soon... \n\nYou are vital to project EDEN...\n\n--Oscorp Cares");
        yield return new WaitForSeconds(1f);
        wakeUpButton.gameObject.SetActive(true);

    }

    IEnumerator TypeDialogue(string text)
    {
        dialogueText.text = "";
        foreach (char c in text)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }
    }

    IEnumerator TypeWelcome(string text)
    {
        welcomeText.text = "";
        foreach (char c in text)
        {
            welcomeText.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }
    }

    IEnumerator SharpFlashCreepyImages()
    {
        for (int i = 0; i < flashSprites.Length; i++)
        {
            // Flash black
            blackOverlay.color = Color.black;
            flashImage.enabled = false;
            yield return new WaitForSeconds(flashSpeed);

            // Show next image
            flashImage.sprite = flashSprites[i];
            flashImage.rectTransform.anchorMin = Vector2.zero;
            flashImage.rectTransform.anchorMax = Vector2.one;
            flashImage.rectTransform.offsetMin = Vector2.zero;
            flashImage.rectTransform.offsetMax = Vector2.zero;
            flashImage.rectTransform.localPosition = Vector3.zero;
            flashImage.rectTransform.localRotation = Quaternion.identity;
            flashImage.enabled = true;

            blackOverlay.color = new Color(0, 0, 0, 0); // clear
            yield return new WaitForSeconds(flashSpeed);
        }
    }
}
