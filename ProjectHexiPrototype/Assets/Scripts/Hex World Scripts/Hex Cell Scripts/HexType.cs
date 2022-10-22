using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HexType
{
    // default tile types
    None,
    Init,

    // NATURE TILES
    Plain,
    Forest,
    Pond,
    Mountain,

    // FRIENDLY TILES
    Village,
    Quarry,
    Chest,

    // ENEMY TILES
    Castle,
    Camp,
}

public static class HexTypeExtensions
{
    public static List<HexOption> GetHexOptions(this HexType type)
    {
        List<HexOption> options = new List<HexOption>();
        switch (type)
        {
            default:
            case HexType.None:
                break;
            case HexType.Init:
                options.Add(new HexOption(HexOption.OptionType.StartHex, 1, 0));
                break;
            case HexType.Plain:
                break;
            case HexType.Forest:
                options.Add(new HexOption(HexOption.OptionType.CutDownTree, 3, 1));
                break;
            case HexType.Pond:
                options.Add(new HexOption(HexOption.OptionType.FishForFish, 1, 2));
                break;
            case HexType.Mountain:
                break;
            case HexType.Village:
                options.Add(new HexOption(HexOption.OptionType.UpgradeCard, 3, 2));
                options.Add(new HexOption(HexOption.OptionType.EnterShop, -1, 0));
                break;
            case HexType.Quarry:
                options.Add(new HexOption(HexOption.OptionType.MineForLoot, 3, 1));
                break;
            case HexType.Chest:
                options.Add(new HexOption(HexOption.OptionType.CollectLoot, 1, 0));
                break;
            case HexType.Castle:
                options.Add(new HexOption(HexOption.OptionType.EnterCastle, 1, 0));
                break;
            case HexType.Camp:
                options.Add(new HexOption(HexOption.OptionType.EnterCamp, 1, 0));
                break;
        }
        return options;
    }
}
