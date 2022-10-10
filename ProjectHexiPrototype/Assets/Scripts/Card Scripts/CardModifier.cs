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
    public Modifier modifier;
    public int amount;
}
