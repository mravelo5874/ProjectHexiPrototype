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
        Slash, // edged weapons like: swords, daggers, axes, and scythes
        Blunt, // blunt weapons like: clubs, maces, hammers, staves, and flails
        Penetrative, // pointed weapons like: spears, pikes, lances, and forks
    }
    public Modifier modifier;
    public DamageType damage_type;
    public int amount;

    public static string GetStringDescription(List<CardModifier> modifiers)
    {
        string description = "";
        foreach (CardModifier mod in modifiers)
        {
            switch (mod.modifier)
            {   
                default:
                case CardModifier.Modifier.None:
                    break;

                case CardModifier.Modifier.DealDamage:
                    description += "Deal " + mod.amount + " " + mod.damage_type.ToString().ToLower() + " damage\n";
                    break;

                case CardModifier.Modifier.GainBlock:
                    description += "Gain " + mod.amount + " block\n";
                    break;

                case CardModifier.Modifier.Heal:
                    description += "Heal for " + mod.amount + "\n";
                    break;
            }
        }

        return description;
    }
}
