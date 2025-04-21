using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class CardData : ScriptableObject
{
    public string cardName;
    public Sprite artwork;
    public string description;
    public int manaCost;
    public int rarity;
    public int cardType; // 0 = Attack, 1 = Defense, 2 = Utility
    public int Amount1;
}
