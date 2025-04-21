using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Animations;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

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
    public TextMeshProUGUI EndturnTxt;
    public Button EndTurnBtn;
    public Button CoreBtn;
    private int coreAmount = 0;

    //Animation
    public Animator controller;
    public Animator EnemyController;

    //Card Bools
    private bool scoutEffect;
    private bool enemyStun;
    private bool ScoutTurn;


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
        ScoutTurn = false;
        coreAmount = 0;
    }


    //Functions to the player turn
    public void startPlayerturn() //Call this function after End Turn button (yet to be implemented)
                                  //Currently attached to EndTurn for testing!!!
    {
        //restock players hand
        if (!ScoutTurn)
        {
            if (!enemyStun)
            {
                playerHealth.takeDamage(5);
            }
            if (playerManaMax < ManaCap)
            {
                playerManaMax++;
            }
        }
        playerMana = playerManaMax;
        PlayerManaTxt.text = playerMana.ToString();
        Debug.Log("Player turn refreshed");
        //Dealing with card refresh
        handManager.RefreshSlots();
        enemyStun = false;
        EndturnTxt.text = "End Turn";
        EndTurnBtn.GetComponent<Image>().color = Color.white;
        //CoreBtn.GetComponent<Image>().color = Color.white;

    }

    //Functions to cards
    public bool canPlayCard(int cardMana)
    {
        if (playerMana >= cardMana)
        {
            playerMana -= cardMana;
            PlayerManaTxt.text = playerMana.ToString();
            if (playerMana <= 0)
            {
                EndTurnBtn.GetComponent<Image>().color = Color.green;
            }
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
            controller.SetTrigger("Attack");
            EnemyController.SetTrigger("Hit");
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
            controller.SetTrigger("Attack");
            EnemyController.SetTrigger("Hit");
        }

    }


    public void PowerCore()
    {
        if (playerMana > 0)
        {
            coreAmount += playerMana;
            Debug.Log("core amount at: " + coreAmount);
            playerMana = 0;
            PlayerManaTxt.text = playerMana.ToString();
            EndTurnBtn.GetComponent<Image>().color = Color.green;


        }
        else
        {
            Debug.Log("No mana to use");
        }
        //UGHHHHH CHEESEBURGER WITH UHHHHHHHHHHH 10 pickels no burger extra bun
        if (coreAmount >= 5)
        {
            Debug.Log("Core Active");
            ScoutTurn = true;
            EndturnTxt.text = "Extra Turn!";
            coreAmount = 0;
            Color neonBlue = new Color(0.3f, 1f, 1f, 1f);
            EndTurnBtn.GetComponent<Image>().color = neonBlue;

        }
    }
}
