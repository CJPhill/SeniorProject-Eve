using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public List<CardData> allPossibleCards;
    private Queue<CardData> deck = new Queue<CardData>();

    public void InitializeDeck()
    {
        List<CardData> tempDeck = new List<CardData>(allPossibleCards);
        Shuffle(tempDeck);

        deck = new Queue<CardData>(tempDeck);
        //for card in cards
        foreach (CardData card in deck)
        {
            Debug.Log("Card in deck: " + card.cardName);
        }
    }

    public CardData DrawCard()
    {
        return deck.Count > 0 ? deck.Dequeue() : null;
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
}
