using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HandManager : MonoBehaviour
{
    public Transform slotParent; // Holds 5 empty CardSlot UI objects
    public GameObject cardPrefab;
    public DeckManager deckManager;

    private CardSlot[] slots;

    private void Start()
    {
        deckManager.InitializeDeck();
        slots = slotParent.GetComponentsInChildren<CardSlot>();
        Debug.Log(slots.Length);

        for (int i = 0; i < 5; i++)
        {
            Debug.Log("Drawing card to slot " + i);
            DrawToSlot(i);
        }
       
    }

    public void DrawToSlot(int index)
    {
        CardData drawn = deckManager.DrawCard();
        if (drawn != null)
        {
            GameObject cardGO = Instantiate(cardPrefab, slots[index].transform);
            CardDisplay display = cardGO.GetComponent<CardDisplay>();
            display.Setup(drawn);

            DraggableCard drag = cardGO.GetComponent<DraggableCard>();
            drag.AssignedSlot = slots[index];
            slots[index].currentCard = drag;
        }
        deckManager.UpdateDeckAmount();
    }
}
