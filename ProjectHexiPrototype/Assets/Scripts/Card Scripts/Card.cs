using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Card")]
public class Card : ScriptableObject
{
    public string card_name;
    public int energy_cost;
    public string card_description;
    
    public List<CardModifier> modifiers;
}
