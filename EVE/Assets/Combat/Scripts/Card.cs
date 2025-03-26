using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
    public string cardName;
    public CardType cardType;
    public int health;
    public int damageMin;
    public int damageMax;
    public DamageType damageType;

    public enum CardType
    {
        Scout,
        Merc,
        Medic
    }

    public enum DamageType
    {
        Scout,
        Merc,
        Medic
    }
}
