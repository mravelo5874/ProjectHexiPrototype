using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Card")]
public class Card : ScriptableObject
{
    public string card_name;
    public int energy_cost;
    public string card_description;

    public enum CardTarget // determines where a card needs to be in order to be played
    {
        Player,
        Enemy,
        Environment
    }
    public CardTarget card_target;

    public List<CardModifier> modifiers;
}
