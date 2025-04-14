using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private bool isPlayerTurn;
    public DeckManager deckManager;
    public HandManager handManager;
    private int playerMana;
    private int playerManaMax;
    private int ManaCap = 5; //Max mana cap for player
    public HealthManager playerHealth;
    public HealthManager enemyHealth;
    public TextMeshProUGUI PlayerManaTxt;

    //Card Bools
    private bool scoutEffect;
    private bool enemyStun;


    private void Start()
    {
        isPlayerTurn = true;
        playerMana = 1;
        playerManaMax = 1;
        deckManager = FindAnyObjectByType<DeckManager>();
        handManager = FindAnyObjectByType<HandManager>();
        PlayerManaTxt.text = playerMana.ToString();
        scoutEffect = false;
        enemyStun = false;
    }


    //Functions to the player turn
    public void startPlayerturn() //Call this function after End Turn button (yet to be implemented)
                                  //Currently attached to EndTurn for testing!!!
    {
        //restock players hand
        if (!enemyStun)
        {
            playerHealth.takeDamage(5);
        }
        if (playerManaMax < ManaCap)
        {
            playerManaMax++;
        }
        playerMana = playerManaMax;
        PlayerManaTxt.text = playerMana.ToString();
        Debug.Log("Player turn refreshed");
        //Dealing with card refresh
        handManager.RefreshSlots();
        enemyStun = false;

    }

    //Functions to cards
    public bool canPlayCard(int cardMana)
    {
        if (playerMana >= cardMana)
        {
            playerMana -= cardMana;
            PlayerManaTxt.text = playerMana.ToString();
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

    public void useCard(CardData card)
    {
        
        Debug.Log("Card used: " + card.cardName);
        if (card.cardType == 0) //Attack
        {
            Debug.Log("Attack");
            enemyHealth.takeDamage(card.Amount1);
        }
        else if (card.cardType == 1) //Healing
        {
            
            Debug.Log("Card type: Heal");
            playerHealth.Heal(card.Amount1);
        }
        else if (card.cardType == 2) //Scan
        {
            Debug.Log("Card type: Scan");
            scoutEffect = true;

        }
        else if (card.cardType == 3) //Sniper
        {
            Debug.Log("Card type: Sniper");
            if (scoutEffect)
            {
                enemyStun = true;
            }
            enemyHealth.takeDamage(10);
        }

    }
}
