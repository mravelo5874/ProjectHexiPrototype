using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexOption
{
    public HexOption(OptionType _option, int _charges, int _cost) // constructor
    {   
        this.option_type = _option;
        this.charges = _charges;
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
    public int charges; // how many times can player execute option (if -1, can be done infinite number of times)
    public int cost; // how much world energy does executing this option cost?

    public string GetText()
    {
        // TODO: format string correctly
        return option_type.ToString();
    }

    public void Execute()
    {
        // return if no charges are left
        if (charges == 0)
        {
            return;
        }

        // at what cost ?
        // only reduce charges if not -1 (infinite)
        if (charges != -1)
        {
            charges--;
        }
        HexWorldManager.instance.ConsumeWorldEnergy(cost);
        
        // TODO: execute option
    }
}
