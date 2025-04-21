using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem.Android;
using System.Runtime.CompilerServices;

public class HandManager : MonoBehaviour
{
    public Transform slotParent; // Holds 5 empty CardSlot UI objects
    public GameObject cardPrefab;
    public DeckManager deckManager;
    public GraveyardManager graveyardManager;

    private CardSlot[] slots;

    private void Start()
    {
        deckManager.InitializeDeck();
        slots = slotParent.GetComponentsInChildren<CardSlot>();
        graveyardManager = GameObject.FindAnyObjectByType<GraveyardManager>();
        Debug.Log(slots.Length);

        for (int i = 0; i < 5; i++)
        {
            Debug.Log("Drawing card to slot " + i);
            DrawToSlot(i);
        }
       
    }

    public void RefreshSlots()
    {
        Debug.Log("Refreshing Hand");
        bool handFull = true;
        if (deckManager.numOfCardsInDeck == 0)
        {
            Debug.Log("Deck is empty, moving cards from graveyard");
            graveyardManager.refreshGraveyard();
            deckManager.UpdateDeckAmount();
        }
        else
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].currentCard == null)
                {

                    DrawToSlot(i);
                    handFull = false;


                }
                else
                {
                    Debug.Log("card in slot: " + i);

                }
            }
        }
            
    }

    public void DrawToSlot(int index)
    {
        CardData drawn = deckManager.DrawCard();
        if (drawn != null)
        {
            GameObject cardGO = Instantiate(cardPrefab, slots[index].transform);
            CardDisplay display = cardGO.GetComponent<CardDisplay>();
            DraggableCard draggableCard = cardGO.GetComponent<DraggableCard>();
            display.Setup(drawn);
            draggableCard.setup(drawn);

            DraggableCard drag = cardGO.GetComponent<DraggableCard>();
            drag.AssignedSlot = slots[index];
            slots[index].currentCard = drag;
        }
        deckManager.UpdateDeckAmount();
    }
}
