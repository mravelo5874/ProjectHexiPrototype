using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexOption
{
    public HexOption(OptionType _option, int _cost) // constructor
    {   
        this.option_type = _option;
        this.cost = _cost;
    }

    public enum OptionType
    {
        None, // default option
        StartHex, // starting hex cell tile
        
        // NATURE TILES
        CutDownTree, // only on forest tiles
        FishForFish, // only on pond tiles

        // FRIENDLY TILES
        UpgradeCard,
        RemoveCard,
        AddCard,
        EnterShop,
        CollectLoot,
        MineForLoot,

        // ENEMY TILES
        EnterCastle, // only on castle tiles
        EnterCamp, // only on camp tiles
    }
    public OptionType option_type;
    public bool used = false; // has this option been used?
    public int cost; // how much world energy does executing this option cost?

    public string GetText()
    {
        // TODO: format string correctly
        return option_type.ToString();
    }

    public void Execute()
    {
        // return if no charges are left
        if (used)
        {
            return;
        }
        // at what cost ?
        used = true;
        HexWorldManager.instance.ConsumeWorldEnergy(cost);
    }
}
