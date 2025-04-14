using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public List<CardData> allPossibleCards;
    private Queue<CardData> deck = new Queue<CardData>();
    [HideInInspector] public int numOfCardsInDeck = 0;
    public TextMeshProUGUI DeckAmountText;

    public void InitializeDeck()
    {
        List<CardData> tempDeck = new List<CardData>(allPossibleCards);
        Shuffle(tempDeck);

        deck = new Queue<CardData>(tempDeck);
        DeckAmountText.text = deck.Count.ToString();
        //for card in cards
        numOfCardsInDeck = deck.Count;
        foreach (CardData card in deck)
        {
            Debug.Log("Card in deck: " + card.cardName);
        }
    }

    public CardData DrawCard()
    {
        if (deck.Count > 0)
        {
            numOfCardsInDeck--;
        }
        return deck.Count > 0 ? deck.Dequeue() : null;
    }

    public void UpdateDeckAmount()
    {
        DeckAmountText.text = deck.Count.ToString();
        numOfCardsInDeck = deck.Count;
    }



    private void Shuffle(List<CardData> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            CardData temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    public void refreshDeck(List<CardData> list)
    {
        deck = new Queue<CardData>(list);
        numOfCardsInDeck = deck.Count;
    }
}
