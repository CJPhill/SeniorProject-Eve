using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GraveyardManager : MonoBehaviour
{
    public List<CardData> graveyard = new List<CardData>();
    public DeckManager deckManager;
    public TextMeshProUGUI GraveyardAmountText;

    public void Start()
    {
        deckManager = GameObject.FindAnyObjectByType<DeckManager>();
        Debug.Log("Graveyard initialized.");
        UpdateGraveyardAmount();
    }

    public void AddToGraveyard(CardData card)
    {
        graveyard.Add(card);
        Debug.Log("Card added to graveyard: " + card.cardName);
        UpdateGraveyardAmount();
    }

    public void refreshGraveyard()
    {
        deckManager.refreshDeck(graveyard);
        graveyard.Clear();
        UpdateGraveyardAmount();
    }

    public void UpdateGraveyardAmount()
    {
        GraveyardAmountText.text = graveyard.Count.ToString();
    }
}
