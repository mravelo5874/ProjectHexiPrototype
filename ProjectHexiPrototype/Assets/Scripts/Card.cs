using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Card")]
public class Card : ScriptableObject
{
    public string card_name;
    public int energy_cost;
    
    public bool deal_damage;
    public int damage_amount;

    public bool gain_block;
    public int block_amount;
}
