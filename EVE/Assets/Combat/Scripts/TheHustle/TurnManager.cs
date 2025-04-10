using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private bool isPlayerTurn;
    public DeckManager deckManager;
    public HandManager handManager;
    private int playerMana;
    private int playerManaMax;

    private void Start()
    {
        isPlayerTurn = true; 
        playerMana = 1;
        playerManaMax = 1;
        deckManager = FindAnyObjectByType<DeckManager>();
        handManager = FindAnyObjectByType<HandManager>();
    }

    public void startPlayerturn() //Call this function after End Turn button (yet to be implemented)
        //Currently attached to EndTurn for testing!!!
    {
        //restock players hand
        playerManaMax++;
        playerMana = playerManaMax;
        Debug.Log("Player turn refreshed");
        //Dealing with card refresh
        handManager.RefreshSlots();


    }

    public bool canPlayCard(int cardMana)
    {
        if (playerMana >= cardMana)
        {
            playerMana -= cardMana;
            return true;
        }
        else
        {
            return false;
        }

    }

    //Debug funcs
    public int PlayerManaCheck()
    {
        return playerMana;
    }

    public int PlayerMaxCheck()
    {
        return playerManaMax;
    }



}
