using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    public Image artworkImage;
    public TMP_Text nameText;
    public TMP_Text descriptionText;
    public TMP_Text manaCostText;

    private CardData cardData;
    public CardData CardData => cardData;

    public void Setup(CardData data)
    {

        if (data == null)
        {
            Debug.LogError("Card data is null!");
            return;
        }
        if (artworkImage == null || nameText == null || descriptionText == null || manaCostText == null)
        {
            Debug.LogError("One or more UI components are not assigned!");
            return;
        }

        cardData = data;
        artworkImage.sprite = data.artwork;
        nameText.text = data.cardName;
        descriptionText.text = data.description;
        manaCostText.text = data.manaCost.ToString();
    }
}
