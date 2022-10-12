using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardModifier
{
    public enum Modifier
    {
        None,
        DealDamage,
        GainBlock,
        Heal
    }
    public enum DamageType
    {
        None, // not a damage mod
        Slice, // edged weapons like: swords, daggers, axes, and scythes
        Blunt, // blunt weapons like: clubs, maces, hammers, staves, and flails
        Penetrative, // pointed weapons like: spears, pikes, lances, and forks
    }
    public Modifier modifier;
    public DamageType damage_type;
    public int amount;
}
